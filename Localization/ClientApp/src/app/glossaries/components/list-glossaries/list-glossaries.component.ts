import { Component, OnInit, Input, ViewChild } from '@angular/core';

import { MatSort, MatTableDataSource } from '@angular/material';

import { GlossaryService } from 'src/app/services/glossary.service';
import { Glossaries, GlossariesDTO } from 'src/app/models/DTO/glossaries.type';

@Component({
  selector: 'app-list-glossaries',
  templateUrl: './list-glossaries.component.html',
  styleUrls: ['./list-glossaries.component.css'],
  providers: [GlossaryService]
})
export class ListGlossariesComponent
{
  //glossaries: GlossariesDTO[];

  displayedColumns: string[] = ['name', 'localesName', 'localizationProjectsName', 'additionalButtons'];
  private dataSource = new MatTableDataSource();
  @ViewChild(MatSort) sort: MatSort;

  isDataSourceLoaded: boolean = true;

  constructor(private glossariesService: GlossaryService)
  {
    this.getGlossariesDTO();
  }

  ngAfterViewInit()
  {
    this.dataSource.sort = this.sort;
  }

  getGlossariesDTO()
  {
    //this.glossariesService.getGlossariesDTO()
    //  .subscribe(glossaries => this.glossaries = glossaries,
    //  error => console.error(error));
    
    this.glossariesService.getGlossariesDTO()
      .subscribe(glossaries => this.dataSource.data = glossaries,
      error =>
      {
        this.isDataSourceLoaded = false;
        console.error(error);
      });
  }

}
