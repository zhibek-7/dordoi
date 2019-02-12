import { Component, OnInit } from '@angular/core';

import { DataType } from '../../models/dataType.type';

import { DataImportService } from 'src/app/services/dataImport.service';
import { DataImportSignalRService } from 'src/app/services/dataImportSignalR.service';

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

  log: string = '';

  constructor(
    private dataImportService: DataImportService,
    private dataImportSignalRService: DataImportSignalRService,
  ) {
    this.dataTypes = [
      new DataType('Локали', 'locales'),
    ];
  }

  ngOnInit() {
    this.dataImportSignalRService.logUpdated
      .asObservable().subscribe(logEntry => this.log += `${logEntry.date.toLocaleString()} ${logEntry.message}\n`);
  }

  fileSelected(fileInputChangeArgs: any) {
    const file = fileInputChangeArgs.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  beginImport() {
    this.dataImportSignalRService.getConnectionId()
      .then(signalrConnectionId => {
        this.dataImportService.importData(
          this.selectedFile,
          this.selectedDataType.entityName,
          signalrConnectionId,
          this.cleanTableBeforeImportFlag)
          .subscribe();
      });
  }

}
