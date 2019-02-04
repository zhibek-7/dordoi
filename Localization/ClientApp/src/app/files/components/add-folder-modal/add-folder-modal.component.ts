import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TreeNode } from 'primeng/api';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { File } from 'src/app/models/database-entities/file.type';

@Component({
  selector: 'app-add-folder-modal',
  templateUrl: './add-folder-modal.component.html',
  styleUrls: ['./add-folder-modal.component.css']
})
export class AddFolderModalComponent extends ModalComponent implements OnInit {

  newFolder: File;

  node: TreeNode;

  @Output() newFolderSubmitted = new EventEmitter<[File, TreeNode]>();

  constructor() {
    super();
    this.resetNewFileModel();
  }

  ngOnInit() {
  }

  showForNode(node: TreeNode) {
    this.node = node;
    this.resetNewFileModel();
    super.show();
  }

  submitNewFolder() {
    this.hide();
    this.newFolderSubmitted.emit([this.newFolder, this.node]);
  }

  resetNewFileModel() {
    this.newFolder = new File();
    this.newFolder.isFolder = true;
    this.newFolder.name = '';
  }

}

    this.newFolder.is_Folder = true;
    this.newFolder.name_text = '';