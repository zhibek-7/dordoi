import { Injectable, EventEmitter } from '@angular/core';
import { LogEntry } from 'src/app/models/data-import/logEntry.type';
import { SignalRService } from './signalR.service';
import * as signalR from "@aspnet/signalr";

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

  public async getConnectionId(): Promise<string> {
    let startPromise: Promise<void> = new Promise<void>(resolve => resolve());
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      startPromise = this.connection.start()
        .catch(error => {
          console.log(error);
          alert(error);
        });
    }
    await startPromise;
    return this.connection.invoke<string>("GetConnectionId");
  }

}
