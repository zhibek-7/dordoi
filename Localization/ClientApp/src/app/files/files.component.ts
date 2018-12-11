import { Component, OnInit, ViewEncapsulation, Predicate } from '@angular/core';
import { TreeNode } from 'primeng/api';

import { FileService } from '../services/file.service';
import { File as FileData } from '../models/database-entities/file.type';
import { catchError, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class FilesComponent implements OnInit {
  files: TreeNode[];

  cols: any[];

  selectedNode: TreeNode;

  isLoading: boolean;

  constructor(private fileService: FileService) { }

  ngOnInit(): void {
    this.cols = [
      { field: 'name', header: 'Name' },
      { field: 'stringsCount', header: 'Strings', width: '100px', textalign: 'right' },
      { width: '50px' },
      { field: 'version', header: 'Version', width: '80px', textalign: 'center' },
      { field: 'priority', header: 'Priority', width: '80px', textalign: 'center' },
      { width: '50px' }
    ];

    this.getFiles();
  }

  // Files
  // Get
  getFiles(): void {
    this.isLoading = true;

    this.fileService.getFiles().subscribe(files => { 
      this.files = files;

      this.isLoading = false;
    });
  };

  // Add
  addFolder(parentNode?: TreeNode): void {
    const folderName = "Folder";

    const parentId = parentNode ? parentNode.data.id : null;

    this.fileService.addFolder(folderName, parentId).subscribe(
      node => this.addNode(node, parentNode),
      error => alert(error.error)
    );
  }

  addFile(file: File, parentNode: TreeNode): void {
    const parentId = parentNode ? parentNode.data.id : null;

    this.fileService.addFile(file, parentId).subscribe(
      node => this.addNode(node, parentNode),
      error => alert(error.error)
    );
  }

  // Add node
  addNode(addedNode: TreeNode, parent: TreeNode): void {
    // Nodes (parent children or root)
    const nodes = parent ? [...parent.children] : [...this.files];

    // Find last index in nodes list
    const lastIndex = this.findLastIndex(nodes, node => node.data.isFolder == addedNode.data.isFolder);

    // 
    addedNode.parent = parent;

    if (parent != null) {
      // Insert node object in founded index position
      parent.children.splice(lastIndex, 0, addedNode);
      
      // Expand parent node
      parent.expanded = true;
    } else {
      // Insert node object in founded index position
      this.files.splice(lastIndex, 0, addedNode);
    }

    this.files = [...this.files];
  }

  // Delete
  deleteNode(node: TreeNode): void {
    this.fileService.deleteNode(node.data).subscribe(response => console.log(response));
  }

  // Update
  updateNode(data: FileData): void {
    this.fileService.updateNode(data).subscribe(response => console.log(response))
  }

  onFileUpload(event: any, parentNode?: TreeNode): void {
    const file = event.target.files[0];
  
    this.addFile(file, parentNode);
    // event.files.forEach(file => this.fileService.addFile(file).subscribe(node => this.files = [...this.files, node]));
  }

  private findLastIndex(nodes: TreeNode[], predicate: Predicate<TreeNode>): number {
    // Find index in reversed nodes by predicate
    const reversedIndex = nodes.reverse().findIndex(predicate);

    // Return nodes length minus founded index
    return reversedIndex < 0 ? 0 : nodes.length - reversedIndex;
  }
}
