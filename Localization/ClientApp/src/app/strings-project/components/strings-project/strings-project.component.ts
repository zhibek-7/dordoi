import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource, MatSort, MatPaginator } from "@angular/material";

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

@Component({
  selector: 'app-strings-project',
  templateUrl: './strings-project.component.html',
  styleUrls: ['./strings-project.component.css']
})
export class StringsProjectComponent implements OnInit {


  //#region для настройки таблицы (mat-table)
  displayedColumns: string[] = [
    "select",
    "substring_to_translate",
    "translation_memories_name",
    "additionalButtons"
  ];
  private dataSource = new MatTableDataSource<Selectable<TranslationSubstringTableViewDTO>>();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  //#endregion

  //#region Для обращения к модальным окнам
  @ViewChild("confirmDeleteFormModal")
  public confirmDeleteFormModal: ConfirmDeleteStringProjectFormModalComponent;

  @ViewChild("editFormModal")
  public editFormModal: EditStringProjectFormModalComponent;

  @ViewChild("loadFormModal")
  public loadFormModal: LoadStringProjectFormModalComponent;

  @ViewChild("appointFormModal")
  public appointFormModal: AppointTranslationMemoriesProjectFormModalComponent;
  //#endregion

  isVisibleEditFormModal: boolean = false;
  isVisibleAppointFormModal: boolean = false;

  isSelectedAll: boolean = false;
  isSelected: boolean = false;

  isDataSourceLoaded: boolean = true;

  translationSubstring: TranslationSubstringForEditingDTO;
  translationSubstrings: TranslationSubstringForEditingDTO[] = [];

  fileTypes: string;

  translationSubstringsSubstringToTranslate: string;

  private idsSelected: number[] = [];
  private indexSelected: number;


  constructor(
    private translationSubstringService: TranslationSubstringService,
    private projectsService: ProjectsService,
    private requestDataReloadService: RequestDataReloadService
  ) {
    this.requestDataReloadService.updateRequested.subscribe(() =>
      this.getAllDTO()
    );
  }

  ngOnInit() {
    this.getAllDTO();
    this.getFileTypes();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  /**
   * Получение списка строк памяти переводов текущего проекта для отображения в таблице.
   */
  getAllDTO() {
    this.translationSubstringService.getAllWithTranslationMemoryByProject(this.projectsService.currentProjectId).subscribe(
      translationSubstrings => {
        this.dataSource.data = translationSubstrings.map(
          translationSubstring => new Selectable<TranslationSubstringTableViewDTO>(translationSubstring, false));
      },
      error => {
        this.isDataSourceLoaded = false;
        console.error(error);
      }
    );
  }

  //#region Выбранная строка. Его отметка и возврат.

  /**
   * Фиксация индекса выбранных элементов.
   * @param element Выбранный элемент пользователем.
   */
  selected(element: Selectable<TranslationSubstringTableViewDTO>) {
    element.isSelected = !element.isSelected;
    
    if (element.isSelected) {
      this.idsSelected.push(element.model.id);
    }
    if (element.isSelected == false) {
      let index = this.idsSelected.findIndex(id => id == element.model.id);
      this.idsSelected.splice(index, 1);
    }
    //this.isSelected = this.dataSource.data.filter(t => t.isSelected).length > 0;
    this.isSelected =
      this.idsSelected != undefined &&
      this.idsSelected != null &&
      this.idsSelected.length > 0;
  }

  allSelected() {
    if (this.isSelected)
      this.dataSource.data.forEach(t => t.isSelected = false);
    else
    if (!this.isSelected)
      this.dataSource.data.forEach(t => t.isSelected = true);

    this.isSelected = !this.isSelected;
  }

  /**
   * Возвращает выбранные элементы пользователем.
   */
  getSelected() {
    this.translationSubstrings = this.dataSource.data.filter(t => this.idsSelected.find(id => id == t.model.id) != null)
      .map(t => new TranslationSubstringForEditingDTO(t.model.id, t.model.substring_to_translate));
  }

  /**
   * Возвращает выбранный элемент пользователем.
   */
  getCurrentRowSelected() {
    //var selectedElement = this.dataSource.data[this.indexSelected].model;
    //this.translationSubstring = new TranslationSubstringForEditingDTO(selectedElement.id, selectedElement.substring_to_translate);
  }
  //#endregion

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
    //this.getCurrentRowSelected();
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
  edited(//editedTranslationSubstring: TranslationSubstringForEditingDTO
        ) {
    //this.translationSubstringService
    //  .editSave(editedTranslationSubstring)
    //  .subscribe(() => this.requestDataReloadService.requestUpdate());

    this.requestDataReloadService.requestUpdate();

    //Сброс выбранного элемента.
    this.idsSelected = [];
    this.isSelected = false;
  }

  //#endregion

  //#region Удаление строк.

  /**
   * Отображает диалог для получения подтверждения удаления нескольких строк.
   */
  getConfirmSelectedDelete() {
    this.getSelected();
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
    this.idsSelected = [];
    this.isSelected = false;
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
    this.getSelected();
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
