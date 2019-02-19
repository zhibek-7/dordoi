import { Injectable, EventEmitter } from '@angular/core';
import { LogEntry } from 'src/app/models/data-import/logEntry.type';
import { SignalRService } from './signalR.service';

@Injectable()
export class DataImportSignalRService extends SignalRService {

  public logUpdated = new EventEmitter<LogEntry>();

  constructor() {
    super("/dataimport/hub");
    this.registerOnServerEvents();
  }

  private registerOnServerEvents(): void {
    this.connection.on("LogUpdated", (logEntry: LogEntry) => this.logUpdated.emit(logEntry));
  }

}
