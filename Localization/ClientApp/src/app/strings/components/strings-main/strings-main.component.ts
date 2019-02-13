import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, ParamMap } from "@angular/router";
import { switchMap } from "rxjs/operators";
import { TreeNode } from "primeng/api";

import { TranslationSubstringService } from "src/app/services/translationSubstring.service";
import { LanguageService } from "src/app/services/languages.service";
import { TranslationSubstring } from "src/app/models/database-entities/translationSubstring.type";
import { FileService } from "src/app/services/file.service";
import { FileViewModel } from "src/app/strings/models/file.viewmodel";
import { SortingArgs } from "src/app/shared/models/sorting.args";
import { ProjectsService } from "src/app/services/projects.service";

@Component({
  selector: "app-strings-main",
  templateUrl: "./strings-main.component.html",
  styleUrls: ["./strings-main.component.css"]
})
export class StringsMainComponent implements OnInit {
  translationStrings: TranslationSubstring[] = [];

  pageSize: number = 10;

  currentOffset: number = 0;

  totalCount: number = 0;

  sortByColumnName: string = null;

  stringSearchString: string = "";

  filesViewModels: FileViewModel[] = [];

  selectedFileId: number | null = null;

  private get projectId(): number {
    return this.projectsService.currentProjectId;
  }

  isSortingAscending: boolean = true;

  constructor(
    private projectsService: ProjectsService,
    private fileService: FileService,
    private translationSubstringService: TranslationSubstringService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.fileService.getFilesByProjectIdAsTree(this.projectId).subscribe(
      filesTree => {
        this.filesViewModels = this.filesTreeToFilesList(filesTree);
        this.route.paramMap.subscribe(
          paramMap => {
            const fileIdParamName = "fileId";
            if (paramMap.has(fileIdParamName)) {
              const fileIdParam = +paramMap.get(fileIdParamName);
              if (
                this.filesViewModels.some(value => value.id === fileIdParam)
              ) {
                this.selectedFileId = fileIdParam;
              }
            }
            this.loadStrings();
          },
          error => console.log(error)
        );
      },
      error => console.log(error)
    );
  }

  loadStrings(offset = 0) {
    let sortBy = new Array<string>();
    if (this.sortByColumnName) {
      sortBy = [this.sortByColumnName];
    }
    this.translationSubstringService
      .getStringsByProjectId(
        this.projectId,
        this.selectedFileId,
        this.stringSearchString,
        this.pageSize,
        offset,
        sortBy,
        this.isSortingAscending
      )
      .subscribe(
        response => {
          let strings = response.body;
          this.translationStrings = strings;
          let totalCount = +response.headers.get("totalCount");
          this.totalCount = totalCount;
          this.currentOffset = offset;
        },
        error => console.log(error)
      );
  }

  filesTreeToFilesList(filesTree: TreeNode[]): FileViewModel[] {
    return filesTree
      .map(fileTreeNode =>
        this.filesTreeNodeToFilesListRecursive("", fileTreeNode)
      )
      .reduce(
        (previousValue, currentValue) => previousValue.concat(currentValue),
        new Array<FileViewModel>()
      );
  }

  filesTreeNodeToFilesListRecursive(
    path: string,
    fileTreeNode: TreeNode
  ): FileViewModel[] {
    let pathForCurrentNode = path + "/" + fileTreeNode.data.name_text;
    if (!fileTreeNode.data.is_folder) {
      return [new FileViewModel(pathForCurrentNode, fileTreeNode.data.id)];
    } else {
      return fileTreeNode.children
        .map(child =>
          this.filesTreeNodeToFilesListRecursive(pathForCurrentNode, child)
        )
        .reduce(
          (previousValue, currentValue) => previousValue.concat(currentValue),
          new Array<FileViewModel>()
        );
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
