import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { File } from 'src/app/models/database-entities/file.type';

@Component({
  selector: 'app-rename-file-modal',
  templateUrl: './rename-file-modal.component.html',
  styleUrls: ['./rename-file-modal.component.css']
})
export class RenameFileModalComponent extends ModalComponent implements OnInit {

  @Input() file: File;

  @Output() nameChangeSubmitted = new EventEmitter<File>();

  constructor() {
    super();
  }

  ngOnInit() {
  }

  show() {
    super.show();
  }

  submitNameChange() {
    this.hide();
    this.nameChangeSubmitted.emit(this.file);
  }

}
