import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource, MatSort, MatPaginator } from "@angular/material";

import { GlossaryService } from "src/app/services/glossary.service";

import {
  GlossariesForEditing,
  GlossariesTableViewDTO
} from "src/app/models/DTO/glossariesDTO.type";
import { Glossary } from "src/app/models/database-entities/glossary.type";

import { EditGlossaryFormModalComponent } from "../edit-glossary-form-modal/edit-glossary-form-modal.component";
import { ConfirmDeleteGlossaryComponent } from "../confirm-delete-glossary/confirm-delete-glossary.component";
import { ConfirmClearGlossaryOfTermsComponent } from "../confirm-clear-glossary-of-terms/confirm-clear-glossary-of-terms.component";

import { Selectable } from "src/app/shared/models/selectable.model";
import { RequestDataReloadService } from "src/app/glossaries/services/requestDataReload.service";
import { MainListPageComponent } from "src/app/shared/components/main-list-page/main-list-page.component";
import { ProjectsService } from "src/app/services/projects.service";

@Component({
  selector: "app-list-glossaries",
  templateUrl: "./list-glossaries.component.html",
  styleUrls: ["./list-glossaries.component.css"],
  providers: [GlossaryService]
})
export class ListGlossariesComponent extends MainListPageComponent implements OnInit {
  
  dataSource: Selectable<GlossariesTableViewDTO>[] = [];
  isDataSourceLoaded: boolean = true;

  @ViewChild("parent")
  public parent: MainListPageComponent;

  //#region Для обращения к модальным окнам
  @ViewChild("confirmDeleteGlossaryFormModal")
  public confirmDeleteGlossaryFormModal: ConfirmDeleteGlossaryComponent;

  @ViewChild("confirmClearGlossaryFormModal")
  public confirmClearGlossaryFormModal: ConfirmClearGlossaryOfTermsComponent;

  @ViewChild("editGlossaryFormModal")
  public editGlossaryFormModal: EditGlossaryFormModalComponent;

  isVisibleEditGlossaryFormModal: boolean = false;
  //#endregion

  isSelectedGlossary: boolean = false;
  
  glossary: Glossary = new Glossary();

  private indexSelected: number;

  constructor(
    private glossariesService: GlossaryService,
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
  
  /** Получение списка глоссарий для отображения в таблице. */
  loadData(offset = 0) {
    super.loadData();
    let sortBy = new Array<string>();
    if (this.sortByColumnName) {
      sortBy = [this.sortByColumnName];
    }

    this.glossariesService.getAllByUserId(
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
            new Selectable<GlossariesTableViewDTO>(translationMemory, false)
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

  //#region Выбранный глоссарий. Его отметка и возврат.

  /**
   * Фиксация индекса выбранного элемента.
   * @param element Выбранный элемент пользователем.
   */
  selected(element: Selectable<GlossariesTableViewDTO>) {
    if (this.dataSource.filter(t => t.isSelected).length > 0) {
      this.dataSource[this.indexSelected].isSelected = false;
    }
    element.isSelected = !element.isSelected;
    this.indexSelected = this.dataSource.findIndex(t => t.isSelected);

    this.isSelectedGlossary =
      this.indexSelected != undefined &&
      this.indexSelected != null &&
      this.indexSelected >= 0;
  }

  /**
   * Возвращает выбранный элемент пользователем.
   */
  getSelectedGlossary() {
    var selectedElement = this.dataSource[this.indexSelected].model;
    this.glossary = new Glossary();
    this.glossary.id = selectedElement.id;
    this.glossary.name_text = selectedElement.name;
  }

  //#endregion

  /**
   * Добавление нового глоссария.
   * @param newGlossary Новый глоссарий.
   */
  addNewGlossary(newGlossary: GlossariesForEditing) {
    this.glossariesService
      .addNewGlossary(newGlossary)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedGlossary = false;
  }

  //#region Редактирование глоссария

  /**
   * Отображает диалог для редактирования.
   */
  getEditingGlossary() {
    this.getSelectedGlossary();
    this.isVisibleEditGlossaryFormModal = true;
    setTimeout(() => {
      this.editGlossaryFormModal.show();
    });
  }

  /**
   * Сохранение изменений.
   * @param editedGlossary
   */
  editedGlossary(editedGlossary: GlossariesForEditing) {
    this.glossariesService
      .editSaveGlossary(editedGlossary)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedGlossary = false;
  }

  //#endregion

  //#region Удаление глоссария.

  /**
   * Отображает диалог для получения подтверждения.
   */
  getConfirmDelete() {
    this.getSelectedGlossary();
    this.confirmDeleteGlossaryFormModal.show();
  }

  /**
   * Удаление глоссария.
   * @param glossary Выбранный глоссарий.
   */
  deleteGlossary(glossary: Glossary) {
    this.glossariesService
      .deleteGlossary(glossary.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedGlossary = false;
  }

  //#endregion

  //#region Удаление всех терминов глоссария.

  /**
   * Отображает диалог для получения подтверждения.
   */
  getConfirmClear() {
    this.getSelectedGlossary();
    this.confirmClearGlossaryFormModal.show();
  }

  /**
   * Удаление всех терминов глоссария
   * @param glossary Выбранный глоссарий.
   */
  clearGlossary(glossary: Glossary) {
    this.glossariesService
      .clearGlossary(glossary.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
    //Сброс выбранного элемента.
    this.indexSelected = undefined;
    this.isSelectedGlossary = false;
  }

  //#endregion
}
