import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { File } from 'src/app/models/database-entities/file.type';

@Component({
  selector: 'app-add-folder-modal',
  templateUrl: './add-folder-modal.component.html',
  styleUrls: ['./add-folder-modal.component.css']
})
export class AddFolderModalComponent extends ModalComponent implements OnInit {

  newFolder: File;

  @Output() newFolderSubmitted = new EventEmitter<File>();

  constructor() {
    super();
    this.resetNewFileModel();
  }

  ngOnInit() {
  }

  show() {
    this.resetNewFileModel();
    super.show();
  }

  submitNewFolder() {
    this.hide();
    this.newFolderSubmitted.emit(this.newFolder);
  }

  resetNewFileModel() {
    this.newFolder = new File();
    this.newFolder.isFolder = true;
    this.newFolder.name = '';
  }

}
