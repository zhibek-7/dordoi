import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';

import { GlossaryService } from 'src/app/services/glossary.service';

import { GlossariesForEditing, GlossariesTableViewDTO } from 'src/app/models/DTO/glossariesDTO.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

import { Selectable } from 'src/app/shared/models/selectable.model';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

@Component({
  selector: 'app-list-glossaries',
  templateUrl: './list-glossaries.component.html',
  styleUrls: ['./list-glossaries.component.css'],
  providers: [GlossaryService]
})
export class ListGlossariesComponent implements OnInit
{
  displayedColumns: string[] = ['name', 'localesName', 'localizationProjectsName', 'additionalButtons'];
  private dataSource = new MatTableDataSource<Selectable<GlossariesTableViewDTO>>();
  @ViewChild(MatSort) sort: MatSort;

  isDataSourceLoaded: boolean = true;

  newGlossary: GlossariesForEditing = new GlossariesForEditing(); //переименовать в editableGlossary
  glossaryForConfirm: Glossary = new Glossary();

  private indexSelected: number;

  constructor(private glossariesService: GlossaryService, private requestDataReloadService: RequestDataReloadService)
  {
    this.requestDataReloadService.updateRequested.subscribe(() => this.getGlossariesDTO());
  }

  ngOnInit()
  {
    this.getGlossariesDTO();
  }

  ngAfterViewInit()
  {
    this.dataSource.sort = this.sort;
  }

  getGlossariesDTO()
  {    
    this.glossariesService.getGlossariesDTO()
      .subscribe(glossaries => this.dataSource.data = //glossaries,
        glossaries.map(glossary => new Selectable<GlossariesTableViewDTO>(glossary, false)),
      error =>
      {
        this.isDataSourceLoaded = false;
        console.error(error);
      });
  }

  selected(element: Selectable<GlossariesTableViewDTO>)
  {
    if (this.dataSource.data.filter(t => t.isSelected).length > 0)
    {
      this.dataSource.data[this.indexSelected].isSelected = false;
    }
    element.isSelected = !element.isSelected;
    this.indexSelected = this.dataSource.data.findIndex(t => t.isSelected);
  }

  addNewGlossary(newGlossary: GlossariesForEditing)
  {
    this.glossariesService.addNewGlossary(newGlossary)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
  }

  getConfirm()
  {
    var selectedElement = this.dataSource.data[this.indexSelected].model;
    this.glossaryForConfirm = new Glossary();
    this.glossaryForConfirm.id = selectedElement.id;
    this.glossaryForConfirm.name = selectedElement.name;
  }

  deleteGlossary(glossary: Glossary)
  {
    this.glossariesService.deleteGlossary(glossary.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
  }

  clearGlossary(glossary: Glossary) {
    this.glossariesService.clearGlossary(glossary.id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
  }

}
