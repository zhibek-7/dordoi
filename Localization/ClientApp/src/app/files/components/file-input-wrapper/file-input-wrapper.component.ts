import { Component, ViewChild, EventEmitter, Output, ElementRef, Input } from '@angular/core';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-file-input-wrapper',
  template: `
    <input *ngIf="!isFolderInput" type="file" #fileInput style="display: none;" (change)="onInputChange($event)">
    <input *ngIf="isFolderInput" type="file" webkitdirectory #fileInput style="display: none;" (change)="onInputChange($event)">
  `,
})
export class FileInputWrapper {

  node: TreeNode;

  @ViewChild('fileInput') input: ElementRef;

  @Output() fileConfirmed = new EventEmitter<[any, TreeNode]>();

  @Input() isFolderInput: boolean = false;

  onInputChange(args: any) {
    this.fileConfirmed.emit([args, this.node]);
  }

  activate(node: TreeNode) {
    this.node = node;
    this.input.nativeElement.value = '';
    this.input.nativeElement.click();
  }

}
