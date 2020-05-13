import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { Term } from 'src/app/models/Glossaries/term.type';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

@Component({
  selector: 'app-delete-term-confirmation-modal',
  templateUrl: './delete-term-confirmation-modal.component.html',
  styleUrls: ['./delete-term-confirmation-modal.component.css']
})
export class DeleteTermConfirmationComponent extends ModalComponent implements OnInit {

  @Input() term: Term;

  @Output() deleteTermConfirmed = new EventEmitter();

  constructor() { super(); }

  ngOnInit() {
  }

  confirmDeleteTerm() {
    this.hide();
    this.deleteTermConfirmed.emit();
  }

}
