import { Component, OnInit } from '@angular/core';
import * as signalR from "@aspnet/signalr";

import { DataType } from '../../models/dataType.type';
import { LogEntry } from 'src/app/models/data-import/logEntry.type';

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

  log: string = '';

  connection: signalR.HubConnection = null;

  constructor(
    private dataImportService: DataImportService,
  ) {
    this.dataTypes = [
      new DataType('Локали', 'locales'),
    ];
  }

  ngOnInit() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/dataimport/hub")
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.connection.on("LogUpdated",
      (logEntry: LogEntry) => {
        this.log += `${logEntry.date.toLocaleString()} ${logEntry.message}\n`;
      });
  }

  fileSelected(fileInputChangeArgs: any) {
    const file = fileInputChangeArgs.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  beginImport() {
    this.connection.start()
      .catch(error => {
        console.log(error);
        alert(error);
      })
      .then(() => {
        this.connection.invoke<string>("GetConnectionId")
          .then(connectionId => {
            this.dataImportService.importData(
                this.selectedFile,
                this.selectedDataType.entityName,
                connectionId,
                this.cleanTableBeforeImportFlag)
              .subscribe();
          });
      });
  }

}
