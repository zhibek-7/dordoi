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

  translator_name: string = '';

  download_name: string = '';

  file: File;

  showForFile(file: File) {
    this.file = file;
    this.translator_name = this.file.translator_name;
    this.download_name = this.file.download_name;
    this.show();
  }

  applyChages() {
    this.hide();
    this.file.translator_name = this.translator_name;
    this.file.download_name = this.download_name;
    this.changesApplied.emit(this.file);
  }

}
