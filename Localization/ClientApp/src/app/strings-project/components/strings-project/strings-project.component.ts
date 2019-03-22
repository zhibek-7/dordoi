import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";

import { TranslationMemoryService } from "src/app/services/translation-memory.service";
import { TranslationSubstringService } from "../../../services/translationSubstring.service";

import { TranslationSubstringForEditingDTO, TranslationSubstringTableViewDTO } from "../../../models/DTO/TranslationSubstringDTO.type";

import { AppointTranslationMemoriesProjectFormModalComponent } from "../appoint-translation-memories-project-form-modal/appoint-translation-memories-project-form-modal.component";
import { EditStringProjectFormModalComponent } from "../edit-string-project-form-modal/edit-string-project-form-modal.component";
import { ConfirmDeleteStringProjectFormModalComponent } from "../confirm-delete-string-project-form-modal/confirm-delete-string-project-form-modal.component"
import { LoadStringProjectFormModalComponent } from "../load-string-project-form-modal/load-string-project-form-modal.component";

import { Selectable } from "src/app/shared/models/selectable.model";
import { RequestDataReloadService } from "src/app/glossaries/services/requestDataReload.service";
import { settingFileLoad, fileType } from "../../models/settingFileLoad";
import { ProjectsService } from "src/app/services/projects.service";
import { MainListPageComponent } from "src/app/shared/components/main-list-page/main-list-page.component";
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-strings-project',
  templateUrl: './strings-project.component.html',
  styleUrls: ['./strings-project.component.css']
})
export class StringsProjectComponent extends MainListPageComponent implements OnInit {

  dataSource: Selectable<TranslationSubstringTableViewDTO>[] = [];
  isDataSourceLoaded: boolean = true;
  
  @ViewChild("parent")
  public parent: MainListPageComponent;

  //#region Для обращения к модальным окнам
  @ViewChild("confirmDeleteFormModal")
  public confirmDeleteFormModal: ConfirmDeleteStringProjectFormModalComponent;

  @ViewChild("editFormModal")
  public editFormModal: EditStringProjectFormModalComponent;

  @ViewChild("loadFormModal")
  public loadFormModal: LoadStringProjectFormModalComponent;

  @ViewChild("appointFormModal")
  public appointFormModal: AppointTranslationMemoriesProjectFormModalComponent;

  isVisibleEditFormModal: boolean = false;
  isVisibleAppointFormModal: boolean = false;
  //#endregion

  /** checkbox для всё/ничего. */
  @ViewChild("checkboxHead")
  public checkboxHead: ElementRef;
  
  
  /** Для одиночного выбора (для последней колонки с доп. кнопками). */
  translationSubstring: TranslationSubstringForEditingDTO;
  
  /** Для множественного выбора (для кнопок над таблицей). */
  translationSubstrings: TranslationSubstringForEditingDTO[] = [];

  fileTypes: string;

  /** Список строк на удаления, для окна подтверждения. */
  translationSubstringsSubstringToTranslate: string;
  
  currentProjectId: Guid;
  
  //#region Init
  constructor(
    private translationSubstringService: TranslationSubstringService,
    private projectsService: ProjectsService,
    private translationMemoryService: TranslationMemoryService,
    private requestDataReloadService: RequestDataReloadService
  ) {
    super();
    this.requestDataReloadService.updateRequested.subscribe(() =>
      this.loadData()
    );
  }

  ngOnInit() {
    this.currentProjectId = this.projectsService.currentProjectId;
    this.loadDropdown();
    this.loadData();
    this.getFileTypes();
  }
  //#endregion
  
  /** Получение списка памятей переводов текущего проекта для фильтра данных таблицы через выпадающий список. */
  loadDropdown() {
    super.loadDropdown();

    this.translationMemoryService.getForSelectByProjectAsync(this.currentProjectId).subscribe(
      translationMemories => {
        this.parent.filters = translationMemories;

        //this.loadData();
      },
        error => {
          console.error(error);
        }
      );
  }

  /** Получение списка строк памяти переводов текущего проекта для отображения в таблице. */
  loadData(offset = 0) {
    super.loadData();
    let sortBy = new Array<string>();
    if (this.sortByColumnName) {
      sortBy = [this.sortByColumnName];
    }
    
    this.translationSubstringService.getAllWithTranslationMemoryByProject(
        this.currentProjectId,
        this.parent.filterId,
        this.parent.searchString,
        this.parent.pageSize,
        this.parent.currentOffset,
        sortBy,
        this.isSortingAscending
      )
      .subscribe(
      response => {
          this.dataSource = response.body.map(translationSubstring =>
            new Selectable<TranslationSubstringTableViewDTO>(
              translationSubstring,
              this.isSelected
                ? this.translationSubstrings.some(t => t.id == translationSubstring.id)
                : false)
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
  
  //#region Выбранная строка. Его отметка и возврат.

  /** Возвращает выбраны ли все записи таблицы на текущей страницы.
   *  Установка неопределенного состояния (indeterminate) при не полном выборе.
   */
  get isSelectedAllCurrentPage(): boolean {
    if (this.dataSource.every(t => t.isSelected)) {
      this.checkboxHead.nativeElement.indeterminate = false;
      return true;
    }
    if (this.dataSource.every(t => !t.isSelected)) {
      this.checkboxHead.nativeElement.indeterminate = false;
      return false;
    }

    this.checkboxHead.nativeElement.indeterminate = true;
    return false;
  }

  /** Возвращает есть ли хотя бы один выбранный элемент. */
  get isSelected(): boolean {
    return (this.translationSubstrings != undefined &&
      this.translationSubstrings != null &&
      this.translationSubstrings.length > 0);
  }

  /**
   * Фиксация выбранного элемента.
   * @param element Выбранный элемент пользователем.
   */
  selected(element: Selectable<TranslationSubstringTableViewDTO>) {
    element.isSelected = !element.isSelected;
    
    if (element.isSelected) {
      this.translationSubstrings.push(new TranslationSubstringForEditingDTO(element.model.id, element.model.substring_to_translate));
    }
    if (element.isSelected == false) {
      let index = this.translationSubstrings.findIndex(t => t.id == element.model.id);
      this.translationSubstrings.splice(index, 1);
    }
  }

  /** Фиксирует выбор всех строк/снятие выбора со всех строк на текущей странице. */
  allSelectedCurrentPage() {
    if (!this.isSelectedAllCurrentPage) {
      this.dataSource.filter(t => !t.isSelected).forEach(t => this.translationSubstrings.push(new TranslationSubstringForEditingDTO(t.model.id, t.model.substring_to_translate)));
      this.dataSource.forEach(t => t.isSelected = true);
    }
    else
    {
      this.dataSource.forEach(t => {
        let index = this.translationSubstrings.findIndex(s => s.id == t.model.id);
          if (index != -1)
            this.translationSubstrings.splice(index, 1);
        }
      );
      this.dataSource.forEach(t => t.isSelected = false);
    }

    console.log("allSelected: ", this.dataSource);
  }

  //#endregion

  /**
   * Отображает диалог для назначения памяти переводов на текущий проект.
   */
  appointFormModalShow() {
    this.isVisibleAppointFormModal = true;
    setTimeout(() => {
      this.appointFormModal.show();
    });
  }

  //#region Редактирование строки

  /**
   * Отображает диалог для редактирования.
   */
  getEditing(currentRow: TranslationSubstringTableViewDTO) {
    this.translationSubstring = new TranslationSubstringForEditingDTO(currentRow.id, currentRow.substring_to_translate, "", []);
    this.isVisibleEditFormModal = true;
    setTimeout(() => {
      this.editFormModal.show();
    });
  }

  /**
   * Сохранение изменений.
   * @param editedTranslationSubstring
   */
  edited() {
    this.requestDataReloadService.requestUpdate();

    //Сброс выбранного элемента.
    this.translationSubstrings = []; 
  }

  //#endregion

  //#region Удаление строк.

  /**
   * Отображает диалог для получения подтверждения удаления нескольких строк.
   */
  getConfirmSelectedDelete() {
    setTimeout(() => {
      this.confirmDeleteFormModal.show();
    });
  }

  /**
   * Отображает диалог для получения подтверждения удаления строки.
   */
  getConfirmDelete(currentRow: TranslationSubstringTableViewDTO) {
    let translationSubstring = new TranslationSubstringForEditingDTO(currentRow.id, currentRow.substring_to_translate);
    this.translationSubstrings = [];
    this.translationSubstrings.push(translationSubstring);
    setTimeout(() => {
      this.confirmDeleteFormModal.show();
    });
  }

  /**
   * Удаление строк.
   * @param confirm подтверждение удаления.
   */
  delete(confirm: boolean) {
    let ids = this.translationSubstrings.map(t => t.id);

    this.translationSubstringService
      .deleteRange(ids)
      .subscribe(() => this.requestDataReloadService.requestUpdate());

    //Сброс выбранного элемента.
    this.translationSubstrings = []; 
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

    if ((file.size / 1024 / 1024) > 200)
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
    //this.getSelected();
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
