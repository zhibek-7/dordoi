import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TreeNode } from "primeng/api";

import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
import { LanguageService } from 'src/app/services/languages.service';
import { Selectable } from 'src/app/shared/models/selectable.model';
import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { File } from 'src/app/models/database-entities/file.type';
import { FileService } from 'src/app/services/file.service';
import { FileViewModel } from 'src/app/strings/models/file.viewmodel';
import { forkJoin } from 'rxjs';
import { SortingArgs } from 'src/app/shared/models/sorting.args';

@Component({
  selector: 'app-strings-main',
  templateUrl: './strings-main.component.html',
  styleUrls: ['./strings-main.component.css']
})
export class StringsMainComponent implements OnInit {

  selectableStrings: Selectable<TranslationSubstring>[] = [];

  pageSize: number = 10;

  currentOffset: number = 0;

  totalCount: number = 0;

  sortByColumnName: string = null;

  stringSearchString: string = '';

  filesViewModels: FileViewModel[] = [];

  selectedFileId: number|null = null;

  private get projectId(): number {
    return +sessionStorage.getItem('ProjecID');
  }

  isSortingAscending: boolean = true;

  constructor(
    private fileService: FileService,
    private translationSubstringService: TranslationSubstringService,
    private languageService: LanguageService,
  ) { }

  ngOnInit() {
    this.loadStrings();
    this.loadFiles();
  }

  loadStrings(offset = 0) {
    let sortBy = new Array<string>();
    if (this.sortByColumnName) {
      sortBy = [this.sortByColumnName];
    }
    this.translationSubstringService.getStringsByProjectId(this.projectId, this.selectedFileId, this.stringSearchString, this.pageSize, offset, sortBy, this.isSortingAscending)
      .subscribe(
        response => {
          let strings = response.body;
          this.selectableStrings = strings.map(translationSubstring => new Selectable<TranslationSubstring>(translationSubstring, false));
          let totalCount = +response.headers.get('totalCount');
          this.totalCount = totalCount;
          this.currentOffset = offset;
        },
        error => console.log(error));
  }

  loadFiles() {
    this.fileService.getFilesByProjectIdAsTree(this.projectId)
      .subscribe(
        filesTree => this.filesViewModels = this.filesTreeToFilesList(filesTree),
        error => console.log(error));
  }

  filesTreeToFilesList(filesTree: TreeNode[]): FileViewModel[] {
    return filesTree
      .map(fileTreeNode => this.filesTreeNodeToFilesListRecursive('', fileTreeNode))
      .reduce((previousValue, currentValue) => previousValue.concat(currentValue), new Array<FileViewModel>());
  }

  filesTreeNodeToFilesListRecursive(path: string, fileTreeNode: TreeNode): FileViewModel[] {
    let pathForCurrentNode = path + '/' + fileTreeNode.data.name;
    if (!fileTreeNode.data.isFolder) {
      return [new FileViewModel(pathForCurrentNode, fileTreeNode.data.id)];
    }
    else {
      return fileTreeNode.children
        .map(child => this.filesTreeNodeToFilesListRecursive(pathForCurrentNode, child))
        .reduce((previousValue, currentValue) => previousValue.concat(currentValue), new Array<FileViewModel>());
    }
  }

  onPageChanged(newOffset: number) {
    this.loadStrings(newOffset);
  }

  applySorting(args: SortingArgs) {
    this.sortByColumnName = args.columnName;
    this.isSortingAscending = args.isAscending;
  }

}
