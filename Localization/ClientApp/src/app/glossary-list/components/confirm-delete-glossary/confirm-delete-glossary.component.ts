import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-confirm-delete-glossary',
  templateUrl: './confirm-delete-glossary.component.html',
  styleUrls: ['./confirm-delete-glossary.component.css']
})
export class ConfirmDeleteGlossaryComponent extends ModalComponent implements OnInit
{
  @Input()
  glossary: Glossary;

  @Output()
  confirmedDelete = new EventEmitter<Glossary>();

  constructor() { super(); }

  ngOnInit() {
  }

  delete() {
    this.hide();
    this.confirmedDelete.emit(this.glossary);
  }

}
