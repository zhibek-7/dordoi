import { Injectable, EventEmitter } from '@angular/core';
import { SignalRService } from './signalR.service';
import { FailedFileParsingModel } from '../models/files/failedFileParsing.type';

@Injectable()
export class FilesSignalRService extends SignalRService {

  public errorReported = new EventEmitter<FailedFileParsingModel>();

  constructor() {
    super("/files/hub");
    this.registerOnServerEvents();
  }

  private registerOnServerEvents(): void {
    this.connection.on("FileParsingFailed", (message: FailedFileParsingModel) => this.errorReported.emit(message));
  }

}
