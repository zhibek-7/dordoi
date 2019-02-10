import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { TreeNode } from "primeng/api";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";
import { File } from "src/app/models/database-entities/file.type";

@Component({
  selector: "app-rename-file-modal",
  templateUrl: "./rename-file-modal.component.html",
  styleUrls: ["./rename-file-modal.component.css"]
})
export class RenameFileModalComponent extends ModalComponent implements OnInit {
  node: TreeNode;

  file: File;

  fileName: string;

  @Output() nameChangeSubmitted = new EventEmitter<[TreeNode, File]>();

  constructor() {
    super();
  }

  ngOnInit() {}

  showNodeFile(node: TreeNode, file: File) {
    this.node = node;
    this.file = file;
    this.fileName = this.file.name_text;
    this.show();
  }

  submitNameChange() {
    this.hide();
    this.file.name_text = this.fileName;
    this.nameChangeSubmitted.emit([this.node, this.file]);
  }
}
