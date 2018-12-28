import { Component, OnInit, Input, ViewChild } from '@angular/core';

import { MatSort, MatTableDataSource } from '@angular/material';
//import { MatTableModule } from '@angular/material/table';
//import { MatSortModule } from '@angular/material/sort';

import { GlossaryService } from 'src/app/services/glossary.service';
import { Glossaries, GlossariesDTO } from 'src/app/models/DTO/glossaries.type';

@Component({
  selector: 'app-list-glossaries',
  templateUrl: './list-glossaries.component.html',
  styleUrls: ['./list-glossaries.component.css'],
  providers: [GlossaryService]
})
export class ListGlossariesComponent implements OnInit
{
  glossaries: GlossariesDTO[];
  
  displayedColumns: string[] = ['name', 'localesName', 'localizationProjectsName', 'additionalButtons'];
  dataSource: MatTableDataSource<GlossariesDTO>;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private glossariesService: GlossaryService)
  {
    this.getGlossariesDTO();
    this.dataSource = new MatTableDataSource(this.glossaries);
  }

  ngOnInit()
  {
    //this.getGlossariesDTO();
    //this.dataSource = new MatTableDataSource(this.glossaries);
  }

  ngAfterViewInit()
  {
    this.dataSource.sort = this.sort;
  }
     
  getGlossariesDTO()
  {
    this.glossariesService.getGlossariesDTO()
      .subscribe(glossaries => this.glossaries = glossaries,
      error => console.error(error));
  }

}
