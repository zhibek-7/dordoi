import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';

@Component({
  selector: 'app-delete-terms-confirmation-modal',
  templateUrl: './delete-terms-confirmation-modal.component.html',
  styleUrls: ['./delete-terms-confirmation-modal.component.css']
})
export class DeleteTermsConfirmationComponent extends ModalComponent implements OnInit {

  @Output() deleteTermsConfirmed = new EventEmitter();

  constructor() { super(); }

  ngOnInit() {
  }

  confirmDeleteTerms() {
    this.hide();
    this.deleteTermsConfirmed.emit();
  }

}
