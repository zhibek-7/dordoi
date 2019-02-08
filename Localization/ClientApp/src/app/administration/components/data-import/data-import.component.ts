import { Component, OnInit } from '@angular/core';
import { DataType } from '../../models/dataType.type';

import { DataImportService } from 'src/app/services/dataImport.service';

@Component({
  selector: 'app-data-import',
  templateUrl: './data-import.component.html',
  styleUrls: ['./data-import.component.css']
})
export class DataImportComponent implements OnInit {

  cleanTableBeforeImportFlag: boolean = false;

  selectedFile: any;

  dataTypes: DataType[];

  selectedDataType: DataType = null;

  constructor(
    private dataImportService: DataImportService,
  ) {
    this.dataTypes = [
      new DataType('Локали', 'locales'),
    ];
  }

  ngOnInit() {
  }

  fileSelected(fileInputChangeArgs: any) {
    const file = fileInputChangeArgs.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  beginImport() {
    this.dataImportService.importData(this.selectedFile, this.selectedDataType.entityName, this.cleanTableBeforeImportFlag)
      .subscribe();
  }

}
