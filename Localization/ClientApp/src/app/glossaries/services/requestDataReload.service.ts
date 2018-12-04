import { Injectable, EventEmitter } from '@angular/core';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Injectable()
export class RequestDataReloadService {

  updateRequested = new EventEmitter();

  requestUpdate() {
    this.updateRequested.emit();
  }

  constructor() { }

}
