import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

@Component({
  selector: 'app-confirm-delete-project',
  templateUrl: './confirm-delete-project.component.html',
  styleUrls: ['./confirm-delete-project.component.css']
})
export class ConfirmDeleteProjectComponent extends ModalComponent implements OnInit {
  @Input()
  nameProject: string;

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
