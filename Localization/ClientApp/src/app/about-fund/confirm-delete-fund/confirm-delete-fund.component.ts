import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
@Component({
  selector: 'app-confirm-delete-fund',
  templateUrl: './confirm-delete-fund.component.html',
  styleUrls: ['./confirm-delete-fund.component.css']
})
export class ConfirmDeleteFundComponent extends ModalComponent implements OnInit {

  @Input()
  nameFund: string;

  @Output()
  confirmedDelete = new EventEmitter<boolean>();

  constructor() { super(); }

  ngOnInit() {
  }

  delete() {
    this.hide();
    this.confirmedDelete.emit(true);
  }

}
