import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';

import { GlossaryService } from 'src/app/services/glossary.service';

import { Glossaries, GlossariesDTO } from 'src/app/models/DTO/glossaries.type';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

@Component({
  selector: 'app-list-glossaries',
  templateUrl: './list-glossaries.component.html',
  styleUrls: ['./list-glossaries.component.css'],
  providers: [GlossaryService]
})
export class ListGlossariesComponent implements OnInit
{
  //glossaries: GlossariesDTO[];

  displayedColumns: string[] = ['name', 'localesName', 'localizationProjectsName', 'additionalButtons'];
  private dataSource = new MatTableDataSource();
  @ViewChild(MatSort) sort: MatSort;

  isDataSourceLoaded: boolean = true;

  constructor(private glossariesService: GlossaryService,
    private requestDataReloadService: RequestDataReloadService)
  {
    this.requestDataReloadService.updateRequested.subscribe(() => this.getGlossariesDTO());
    //this.getGlossariesDTO();
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
      .subscribe(glossaries => this.dataSource.data = glossaries,
      error =>
      {
        this.isDataSourceLoaded = false;
        console.error(error);
      });
  }

  addNewGlossary(newGlossary: Glossaries) {
    this.glossariesService.addNewGlossary(newGlossary)
      .subscribe(
      () => this.requestDataReloadService.requestUpdate()
      );
  }
}
