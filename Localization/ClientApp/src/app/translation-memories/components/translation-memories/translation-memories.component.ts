import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource, MatSort, MatPaginator } from "@angular/material";

import { TranslationMemoryService } from "src/app/services/translation-memory.service";

import { TranslationMemoryForEditingDTO, TranslationMemoryTableViewDTO } from "src/app/models/DTO/translationMemoryDTO.type";

import { EditTranslationMemoryFormModalComponent } from '../edit-translation-memory-form-modal/edit-translation-memory-form-modal.component';
import { ConfirmDeleteTranslationMemoryComponent } from '../confirm-delete-translation-memory/confirm-delete-translation-memory.component';
import { ConfirmClearTranslationMemoryOfStringComponent } from '../confirm-clear-translation-memory-of-string/confirm-clear-translation-memory-of-string.component';

import { Selectable } from "src/app/shared/models/selectable.model";
import { RequestDataReloadService } from "src/app/glossaries/services/requestDataReload.service";

@Component({
  selector: 'app-translation-memories',
  templateUrl: './translation-memories.component.html',
  styleUrls: ['./translation-memories.component.css'],
  providers: [TranslationMemoryService]
})
export class TranslationMemoriesComponent implements OnInit {

  //#region для настройки таблицы (mat-table)
  displayedColumns: string[] = [
    "name_text",
    "string_count",
    "localesName",
    "localizationProjectsName",
    "additionalButtons"
  ];
  private dataSource = new MatTableDataSource<Selectable<TranslationMemoryTableViewDTO>>();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  //#endregion

  //#region Для обращения к модальным окнам
  @ViewChild("confirmDeleteFormModal")
  public confirmDeleteFormModal: ConfirmDeleteTranslationMemoryComponent;

  @ViewChild("confirmClearFormModal")
  public confirmClearFormModal: ConfirmClearTranslationMemoryOfStringComponent;

  @ViewChild("editFormModal")
  public editFormModal: EditTranslationMemoryFormModalComponent;
  //#endregion

  isVisibleEditFormModal: boolean = false;

  isSelectedTranslationMemory: boolean = false;

  isDataSourceLoaded: boolean = true;

  translationMemory: TranslationMemoryForEditingDTO = new TranslationMemoryForEditingDTO();

  private indexSelected: number;

  constructor(
    private translationMemoryService: TranslationMemoryService,
    private requestDataReloadService: RequestDataReloadService
  ) {
    this.requestDataReloadService.updateRequested.subscribe(() =>
      this.getAllDTO()
    );
  }

  ngOnInit() {
    this.getAllDTO();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  /**
   * Получение списка памяти переводов для отображения в таблице.
   */
  getAllDTO() {
    this.translationMemoryService.getAllDTO().subscribe(
      translationMemories =>
        (this.dataSource.data = translationMemories.map(
          translationMemory => new Selectable<TranslationMemoryTableViewDTO>(translationMemory, false)
        )),
      error => {
        this.isDataSourceLoaded = false;
        console.error(error);
      }
    );
  }

  //#region Выбранная памятьи переводов. Его отметка и возврат.

  /**
   * Фиксация индекса выбранного элемента.
   * @param element Выбранный элемент пользователем.
   */
  selected(element: Selectable<TranslationMemoryTableViewDTO>) {
    if (this.dataSource.data.filter(t => t.isSelected).length > 0) {
      this.dataSource.data[this.indexSelected].isSelected = false;
    }
    element.isSelected = !element.isSelected;
    this.indexSelected = this.dataSource.data.findIndex(t => t.isSelected);

    this.isSelectedTranslationMemory =
      this.indexSelected != undefined &&
      this.indexSelected != null &&
      this.indexSelected >= 0;
  }

  /**
   * Возвращает выбранный элемент пользователем.
   */
  getSelectedTranslationMemoriesy() {
    var selectedElement = this.dataSource.data[this.indexSelected].model;
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

}
