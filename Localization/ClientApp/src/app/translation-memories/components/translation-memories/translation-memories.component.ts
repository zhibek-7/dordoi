import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource, MatSort, MatPaginator } from "@angular/material";

import { TranslationMemoryService } from "src/app/services/translation-memory.service";

import { TranslationMemoryForEditingDTO, TranslationMemoryTableViewDTO } from "src/app/models/DTO/translationMemoryDTO.type";

import { EditTranslationMemoryFormModalComponent } from '../edit-translation-memory-form-modal/edit-translation-memory-form-modal.component';
import { ConfirmDeleteTranslationMemoryComponent } from '../confirm-delete-translation-memory/confirm-delete-translation-memory.component';
import { ConfirmClearTranslationMemoryOfStringComponent } from '../confirm-clear-translation-memory-of-string/confirm-clear-translation-memory-of-string.component';
import { LoadTranslationMemoryFormModalComponent } from "../load-translation-memory-form-modal/load-translation-memory-form-modal.component";

import { Selectable } from "src/app/shared/models/selectable.model";
import { RequestDataReloadService } from "src/app/glossaries/services/requestDataReload.service";
import { settingFileLoad, fileType } from "../../models/settingFileLoad";
import { MainListPageComponent } from "src/app/shared/components/main-list-page/main-list-page.component";
import { ProjectsService } from "src/app/services/projects.service";

@Component({
  selector: 'app-translation-memories',
  templateUrl: './translation-memories.component.html',
  styleUrls: ['./translation-memories.component.css'],
  providers: [TranslationMemoryService]
})
export class TranslationMemoriesComponent extends MainListPageComponent implements OnInit {
  
  dataSource: Selectable<TranslationMemoryTableViewDTO>[] = [];
  isDataSourceLoaded: boolean = true;

  @ViewChild("parent")
  public parent: MainListPageComponent;

  //#region Для обращения к модальным окнам
  @ViewChild("confirmDeleteFormModal")
  public confirmDeleteFormModal: ConfirmDeleteTranslationMemoryComponent;

  @ViewChild("confirmClearFormModal")
  public confirmClearFormModal: ConfirmClearTranslationMemoryOfStringComponent;

  @ViewChild("editFormModal")
  public editFormModal: EditTranslationMemoryFormModalComponent;

  @ViewChild("loadFormModal")
  public loadFormModal: LoadTranslationMemoryFormModalComponent;

  isVisibleEditFormModal: boolean = false;
  isSelectedTranslationMemory: boolean = false;
  //#endregion
  
  translationMemory: TranslationMemoryForEditingDTO = new TranslationMemoryForEditingDTO();

  fileTypes: string;

  private indexSelected: number;

  constructor(
    private translationMemoryService: TranslationMemoryService,
    private projectsService: ProjectsService,
    private requestDataReloadService: RequestDataReloadService
  ) {
    super();
    this.requestDataReloadService.updateRequested
      .subscribe(() => this.loadData());
  }

  ngOnInit() {
    this.loadDropdown();
    this.loadData();
    this.getFileTypes();
  }

  /** Получение списка проектов для фильтра данных таблицы через выпадающий список. */
  loadDropdown() {
    super.loadDropdown();

    this.projectsService.getLocalizationProjectForSelectDTOByUser().subscribe(
      projects => {
        this.parent.filters = projects;

        //this.loadData();
      },
      error => {
        console.error(error);
      }
    );
  }

  /** Получение списка памяти переводов для отображения в таблице. */
  loadData(offset = 0) {
    super.loadData();
    let sortBy = new Array<string>();
    if (this.sortByColumnName) {
      sortBy = [this.sortByColumnName];
    }

    this.translationMemoryService.getAllByUserId(
        this.parent.filterId,
        this.parent.searchString,
        this.parent.pageSize,
        this.parent.currentOffset,
        sortBy,
        this.isSortingAscending
      )
      .subscribe(
        response => {
          this.dataSource = response.body.map(translationMemory =>
            new Selectable<TranslationMemoryTableViewDTO>(translationMemory, false)
          );

          this.parent.totalCount = +response.headers.get("totalCount");
          //this.parent.currentOffset = offset;
        },
        error => {
          this.isDataSourceLoaded = false;
          console.log(error);
        }
      );
  }

  //#region Выбранная памятьи переводов. Его отметка и возврат.
  
  /**
   * Фиксация выбранного элемента.
   * @param element Выбранный элемент пользователем.
   */
  selected(element: Selectable<TranslationMemoryTableViewDTO>) {
    if (this.dataSource.filter(t => t.isSelected).length > 0) {
      this.dataSource[this.indexSelected].isSelected = false;
    }
    element.isSelected = !element.isSelected;
    this.indexSelected = this.dataSource.findIndex(t => t.isSelected);

    this.isSelectedTranslationMemory =
      this.indexSelected != undefined &&
      this.indexSelected != null &&
      this.indexSelected >= 0;
  }

  /**
   * Возвращает выбранный элемент пользователем.
   */
  getSelectedTranslationMemoriesy() {
    var selectedElement = this.dataSource[this.indexSelected].model;
    this.translationMemory = new TranslationMemoryForEditingDTO();
    this.translationMemory.id = selectedElement.id;
    this.translationMemory.name_text = selectedElement.name_text;
  }

  //#endregion

  /**
   * Добавление новой памяти переводов.
   * @param newTranslationMemory Новая память переводов.
   */
  add(newTranslationMemory: TranslationMemoryForEditingDTO) {
    this.translationMemoryService
      .create(newTranslationMemory)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedTranslationMemory = false;
  }

  //#region Редактирование памяти переводов

  /**
   * Отображает диалог для редактирования.
   */
  getEditing() {
    this.getSelectedTranslationMemoriesy();
    this.isVisibleEditFormModal = true;
    setTimeout(() => {
      this.editFormModal.show();
    });
  }

  /**
   * Сохранение изменений.
   * @param editedTranslationMemory
   */
  edited(editedTranslationMemory: TranslationMemoryForEditingDTO) {
    this.translationMemoryService
      .editSave(editedTranslationMemory)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedTranslationMemory = false;
  }

  //#endregion

  //#region Удаление памяти переводов.

  /**
   * Отображает диалог для получения подтверждения.
   */
  getConfirmDelete() {
    this.getSelectedTranslationMemoriesy();
    this.confirmDeleteFormModal.show();
  }

  /**
   * Удаление памяти переводов.
   * @param translationMemory Выбранная память переводов.
   */
  delete(translationMemory: TranslationMemoryForEditingDTO) {
    this.translationMemoryService
      .delete(translationMemory.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedTranslationMemory = false;
  }

  //#endregion

  //#region Удаление всех строк памяти переводов.

  /**
   * Отображает диалог для получения подтверждения.
   */
  getConfirmClear() {
    this.getSelectedTranslationMemoriesy();
    this.confirmClearFormModal.show();
  }

  /**
   * Удаление всех строк памяти переводов
   * @param translationMemory Выбранная память переводов.
   */
  clear(translationMemory: TranslationMemoryForEditingDTO) {
    this.translationMemoryService
      .clear(translationMemory.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedTranslationMemory = false;
  }

  //#endregion

  //#region unload - Загрузить

  getFileTypes() {
    let types: string = ""; 
    for (var type in fileType) {
      if (isNaN(Number(type)))
        types += "." + type.toString() + ",";
    }
    this.fileTypes = types.substring(0, types.length - 1);
  }

   unload(files: FileList) {
    let file = files.item(0);

    if ((file.size/1024/1024) > 200)
      alert("Невозможно загрузить файл. Превышен допустимый объем - 200 Мб.");

     let fileType = file.name.substring(file.name.lastIndexOf(".") + 1);
     if (fileType != "csv" && fileType != "tmx")
       alert("Невозможно загрузить файл. Выбранный файл не соответствует допустимым форматам - *.tmx, *.csv.");


     //TODO вставить функцию для загрузки файла

  }

  //#endregion
  
  //#region load - Скачать

  /**
   * Отображает диалог для получения подтверждения.
   */
  getLoad() {
    this.getSelectedTranslationMemoriesy();
    setTimeout(() => {
      this.loadFormModal.show();
    });
  }

  load(setting: settingFileLoad) {
    //console.log("load Submitted: ", setting);

    //TODO вставить функцию для скачивания

  }

  //#endregion
}
