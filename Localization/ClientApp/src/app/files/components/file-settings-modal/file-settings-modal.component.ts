import { Component, OnInit, EventEmitter, Output } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { File } from 'src/app/models/database-entities/file.type';

@Component({
  selector: 'app-file-settings-modal',
  templateUrl: './file-settings-modal.component.html',
  styleUrls: ['./file-settings-modal.component.css']
})
export class FileSettingsModalComponent extends ModalComponent implements OnInit {

  @Output()
  changesApplied = new EventEmitter<File>();

  file: File;

  showForFile(file: File) {
    this.file = file;
    this.show();
  }

  applyChages() {
    this.hide();
    this.changesApplied.emit(this.file);
  }

}
