import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { TreeNode } from 'primeng/api';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { File } from 'src/app/models/database-entities/file.type';

@Component({
  selector: 'app-rename-file-modal',
  templateUrl: './rename-file-modal.component.html',
  styleUrls: ['./rename-file-modal.component.css']
})
export class RenameFileModalComponent extends ModalComponent implements OnInit {

  node: TreeNode;

  file: File;

  @Output() nameChangeSubmitted = new EventEmitter<[TreeNode, File]>();

  constructor() {
    super();
  }

  ngOnInit() {
  }

  showNodeFile(node: TreeNode, file: File) {
    this.node = node;
    this.file = file;
    this.show();
  }

  submitNameChange() {
    this.hide();
    this.nameChangeSubmitted.emit([this.node, this.file]);
  }

}
