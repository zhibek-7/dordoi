import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';

import { GlossaryService } from 'src/app/services/glossary.service';

import { GlossariesForEditing, GlossariesTableViewDTO } from 'src/app/models/DTO/glossariesDTO.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

import { EditGlossaryFormModalComponent } from '../edit-glossary-form-modal/edit-glossary-form-modal.component';
import { ConfirmDeleteGlossaryComponent } from '../confirm-delete-glossary/confirm-delete-glossary.component';
import { ConfirmClearGlossaryOfTermsComponent } from '../confirm-clear-glossary-of-terms/confirm-clear-glossary-of-terms.component';

import { Selectable } from 'src/app/shared/models/selectable.model';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';


@Component({
  selector: 'app-list-glossaries',
  templateUrl: './list-glossaries.component.html',
  styleUrls: ['./list-glossaries.component.css'],
  providers: [GlossaryService]
})
export class ListGlossariesComponent implements OnInit {
  displayedColumns: string[] = ['name', 'localesName', 'localizationProjectsName', 'additionalButtons'];
  private dataSource = new MatTableDataSource<Selectable<GlossariesTableViewDTO>>();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //
  @ViewChild('confirmDeleteGlossaryFormModal')
  public confirmDeleteGlossaryFormModal: ConfirmDeleteGlossaryComponent;

  @ViewChild('confirmClearGlossaryFormModal')
  public confirmClearGlossaryFormModal: ConfirmClearGlossaryOfTermsComponent;

  @ViewChild('editGlossaryFormModal')
  public editGlossaryFormModal: EditGlossaryFormModalComponent;
  //

  isDataSourceLoaded: boolean = true;
  isSelectedGlossary: boolean = false;
  isVisibleEditGlossaryFormModal: boolean = false;

  glossary: Glossary = new Glossary();

  private indexSelected: number;

  constructor(private glossariesService: GlossaryService, private requestDataReloadService: RequestDataReloadService) {
    this.requestDataReloadService.updateRequested.subscribe(() => this.getGlossariesDTO());
  }

  ngOnInit() {
    this.getGlossariesDTO();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  //#region Получение списка глоссарий для отображения в таблице

  getGlossariesDTO() {
    this.glossariesService.getGlossariesDTO()
      .subscribe(glossaries => this.dataSource.data = glossaries.map(glossary => new Selectable<GlossariesTableViewDTO>(glossary, false)),
        error => {
          this.isDataSourceLoaded = false;
          console.error(error);
        });
  }

  //#endregion

  //#region Выбранный глоссарий. Его отметка и возврат.

  selected(element: Selectable<GlossariesTableViewDTO>) {
    if (this.dataSource.data.filter(t => t.isSelected).length > 0) {
      this.dataSource.data[this.indexSelected].isSelected = false;
    }
    element.isSelected = !element.isSelected;
    this.indexSelected = this.dataSource.data.findIndex(t => t.isSelected);
    
    this.isSelectedGlossary = this.indexSelected != undefined && this.indexSelected != null && this.indexSelected >= 0;
  }

  getSelectedGlossary() {
    var selectedElement = this.dataSource.data[this.indexSelected].model;
    this.glossary = new Glossary();
    this.glossary.id = selectedElement.id;
    this.glossary.name = selectedElement.name;
  }

  //#endregion

  //#region Добавление нового глоссария

  addNewGlossary(newGlossary: GlossariesForEditing) {
    this.glossariesService.addNewGlossary(newGlossary)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
  }

  //#endregion

  //#region Редактирование глоссария

  getEditingGlossary() {
    this.getSelectedGlossary();
    this.isVisibleEditGlossaryFormModal = true;
    setTimeout(() => {
      this.editGlossaryFormModal.show();
    })
  }

  editedGlossary(editedGlossary: GlossariesForEditing) {
    this.glossariesService.editSaveGlossary(editedGlossary)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
  }

  //#endregion

  //#region Удаление глоссария

  getConfirmDelete() {
    this.getSelectedGlossary();
    this.confirmDeleteGlossaryFormModal.show();
  }

  deleteGlossary(glossary: Glossary) {
    this.glossariesService.deleteGlossary(glossary.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());

    this.indexSelected = undefined;
    this.isSelectedGlossary = false;
  }

  //#endregion

  //#region Удаление всех терминов глоссария

  getConfirmClear() {
    this.getSelectedGlossary();
    this.confirmClearGlossaryFormModal.show();
  }

  clearGlossary(glossary: Glossary) {
    this.glossariesService.clearGlossary(glossary.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());

    this.indexSelected = undefined;
    this.isSelectedGlossary = false;
  }

  //#endregion
}
