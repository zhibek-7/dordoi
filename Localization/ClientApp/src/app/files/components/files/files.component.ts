import { Component, OnInit, ViewEncapsulation, Predicate } from '@angular/core';

import { TreeNode, MenuItem } from 'primeng/api';
import { NgxSpinnerService } from 'ngx-spinner';

import { FileService } from 'src/app/services/file.service';
import { ProjectsService } from 'src/app/services/projects.service';

import { File as FileData } from 'src/app/models/database-entities/file.type';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class FilesComponent implements OnInit {
  files: TreeNode[];

  cols: any[];

  pasteMenuItem: MenuItem;

  contextMenuItems: MenuItem[];

  searchFilesNamesString: string = '';

  _selectedNode: TreeNode;
  get selectedNode(): TreeNode {
    return this._selectedNode;
  }
  set selectedNode(value: TreeNode) {
    this._selectedNode = value;
    this.pasteMenuItem.visible = this._selectedNode.data.isFolder;
  }

  cuttedNode: TreeNode;

  isLoading: boolean;

  constructor(
    private fileService: FileService,
    private projectsService: ProjectsService,
    private ngxSpinnerService: NgxSpinnerService,
  ) { }

  ngOnInit(): void {

    console.log('ProjectName=' + sessionStorage.getItem('ProjectName'));
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));
    console.log('Projec=' + sessionStorage.getItem('Projec'));

    this.cols = [
      { field: 'name', header: 'Имя' },
      { field: 'dateOfChange', header: 'Дата изменения' },
      { field: 'stringsCount', header: 'Строки', width: '100px', textalign: 'right' },
      { },
      { field: 'version', header: 'Версия', width: '80px', textalign: 'center' },
      { field: 'priority', header: 'Приоритет', width: '100px', textalign: 'center' },
      { }
    ];

    this.pasteMenuItem = { label: 'Вставить', command: (event) => { this.moveCurrentlyCutted(this.selectedNode) }, disabled: true };
    this.contextMenuItems = [
      { label: 'Toggle', command: (event) => { this.toggleFile(this.selectedNode) } },
      { label: 'Вырезать', command: (event) => { this.pseudoCut(this.selectedNode) } },
      this.pasteMenuItem,
      { label: 'Удалить', command: (event) => { this.deleteFile(this.selectedNode) } },
    ];

    this.getFiles();
  }

  pseudoCut(selectedNode: TreeNode): void {
    this.cuttedNode = selectedNode;
    this.pasteMenuItem.disabled = false;
  }

  moveCurrentlyCutted(selectedNode: TreeNode): void {
    this.fileService.changeParentFolder(this.cuttedNode.data, this.selectedNode.data.id)
      .subscribe(() => {
        this.deleteNode(this.cuttedNode);
        this.addNode(this.cuttedNode, selectedNode);
        this.cuttedNode = null;
        this.pasteMenuItem.disabled = true;
      },
      error => alert(error));
  }

  getFiles(): void {
    this.isLoading = true;

    this.fileService.getFilesByProjectIdAsTree(this.projectsService.currentProjectId, this.searchFilesNamesString).subscribe(files => { 
      this.files = files;

      this.isLoading = false;
    });
  };

  addFolder(newFolder: File, parentNode?: TreeNode): void {
    const parentId = parentNode ? parentNode.data.id : null;

    this.fileService.addFolder(newFolder.name, this.projectsService.currentProjectId, parentId).subscribe(
      node => this.addNode(node, parentNode),
      error => alert(error.error)
    );
  }

  uploadFolder(files, parentNode?: TreeNode): void {
    const parentId = parentNode ? parentNode.data.id : null;
    this.ngxSpinnerService.show();
    this.fileService.uploadFolder(files, this.projectsService.currentProjectId, parentId).subscribe(
      () => {
        this.getFiles();
        this.ngxSpinnerService.hide();
      });
  }

  addFile(file: File, parentNode: TreeNode): void {
    const parentId = parentNode ? parentNode.data.id : null;

    this.fileService.addFile(file, this.projectsService.currentProjectId, parentId).subscribe(
      node => this.addNode(node, parentNode),
      error => alert(error.error)
    );
  }

  updateFileVersion(event: any, oldNode: TreeNode): void {
    const file = event.target.files[0];
    if (file) {
      const parentNode = oldNode.parent;
      const parentId = parentNode ? parentNode.data.id : null;
      this.fileService.updateFileVersion(file, this.projectsService.currentProjectId, parentId)
        .subscribe(newNode => {
          this.deleteNode(oldNode);
          this.addNode(newNode, parentNode);
        })
    }
  }

  addNode(addedNode: TreeNode, parent: TreeNode): void {
    // Nodes (parent children or root)
    const nodes = parent ? [...parent.children] : [...this.files];

    // Find last index in nodes list
    const lastIndex = this.findLastIndex(nodes, node => node.data.isFolder == addedNode.data.isFolder);

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
    this.reloadView();
  }

  deleteFile(node: TreeNode): void {
    this.fileService.deleteNode(node.data)
      .subscribe(response => {
        console.log(response);
        this.deleteNode(node);
      });
  }

  deleteNode(node: TreeNode) {
    let indexOfNodeInRootFiles = this.files.lastIndexOf(node);
    let nodeIsRoot = indexOfNodeInRootFiles > -1;
    if (nodeIsRoot) {
      this.files.splice(indexOfNodeInRootFiles, 1);
    }
    else {
      let parentChildren = node.parent.children;
      parentChildren.splice(parentChildren.lastIndexOf(node), 1);
    }
    this.reloadView();
  }

  reloadView() {
    this.files = [...this.files];
  }

  updateNode(data: FileData): void {
    this.fileService.updateNode(data).subscribe(response => console.log(response))
  }

  onFileUpload(event: any, parentNode?: TreeNode): void {
    const file = event.target.files[0];
    if (file) {
      this.addFile(file, parentNode);
    }
    // event.files.forEach(file => this.fileService.addFile(file).subscribe(node => this.files = [...this.files, node]));
  }

  private findLastIndex(nodes: TreeNode[], predicate: Predicate<TreeNode>): number {
    // Find index in reversed nodes by predicate
    const reversedIndex = nodes.reverse().findIndex(predicate);

    // Return nodes length minus founded index
    return reversedIndex < 0 ? 0 : nodes.length - reversedIndex;
  }

  toggleFile(node) {
    node.expanded = !node.expanded;
    this.files = [...this.files];
  }

  renameNode(node: TreeNode, updatedFile: FileData) {
    node.data.name = updatedFile.name;
    this.fileService.updateNode(node.data)
      .subscribe(() => {
        this.reloadView();
      });
  }

}
