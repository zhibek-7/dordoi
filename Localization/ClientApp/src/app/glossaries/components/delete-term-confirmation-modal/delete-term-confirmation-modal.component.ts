import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';

@Component({
  selector: 'app-delete-term-confirmation-modal',
  templateUrl: './delete-term-confirmation-modal.component.html',
  styleUrls: ['./delete-term-confirmation-modal.component.css']
})
export class DeleteTermConfirmationComponent extends ModalComponent implements OnInit {

  @Input() term: String;

  @Output() deleteTermConfirmed = new EventEmitter();

  constructor() { super(); }

  ngOnInit() {
  }

  confirmDeleteTerm() {
    this.hide();
    this.deleteTermConfirmed.emit();
  }

}