import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-confirm-clear-glossary-of-terms',
  templateUrl: './confirm-clear-glossary-of-terms.component.html',
  styleUrls: ['./confirm-clear-glossary-of-terms.component.css']
})
export class ConfirmClearGlossaryOfTermsComponent extends ModalComponent implements OnInit
{
  @Input()
  glossary: Glossary;

  @Output()
  confirmedClear = new EventEmitter<Glossary>();

  constructor() { super(); }

  ngOnInit() {
  }

  clear() {
    this.hide();
    this.confirmedClear.emit(this.glossary);
  }

}
