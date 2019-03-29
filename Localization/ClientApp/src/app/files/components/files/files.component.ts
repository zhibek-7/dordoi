import { Component, OnInit, ViewEncapsulation, Predicate } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { MatDialog } from "@angular/material";

import { TreeNode } from "primeng/api";
import { NgxSpinnerService } from "ngx-spinner";
import { saveAs } from "file-saver";

import { FileService } from "src/app/services/file.service";
import { ProjectsService } from "src/app/services/projects.service";
import { FilesSignalRService } from "src/app/services/filesSignalR.service";

import { UploadingLogModalComponent } from "../uploading-log-modal/uploading-log-modal.component";
import { Guid } from "guid-typescript";
import { File as FileData } from "src/app/models/database-entities/file.type";
import { NotifierService } from "angular-notifier";

@Component({
  selector: "app-files",
  templateUrl: "./files.component.html",
  styleUrls: ["./files.component.css"],
  encapsulation: ViewEncapsulation.None
})
export class FilesComponent implements OnInit {
  files: TreeNode[];
  cols: any[];
  searchFilesNamesString: string = "";
  cuttedNode: TreeNode;
  selectedNode: TreeNode;

  isLoading: boolean;

  // Переменные для работы при выборе файлов для перевода
  isSelectionFileForTranslation: boolean = false;
  selectedProjectId: Guid;
  selectedLanguageId: Guid;
  private readonly notifier: NotifierService;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fileService: FileService,
    private projectsService: ProjectsService,
    private ngxSpinnerService: NgxSpinnerService,
    private filesSignalRService: FilesSignalRService,
    public dialog: MatDialog,
    notifierService: NotifierService
  ) {
    this.notifier = notifierService;
  }

  ngOnInit(): void {
    //    console.log("ProjectName=" + sessionStorage.getItem("ProjectName"));
    //    console.log("ProjectID=" + sessionStorage.getItem("ProjectID"));
    //    console.log("Projec=" + sessionStorage.getItem("Projec"));

    // данное условие проверяет на какой вкладке была открыта данная форма.
    // если длинна параметра -1, то это маршрут настроек данного проекта
    // иначе данная форма открыта при выборе языка перевода
    if (this.router.url.indexOf("LanguageFiles") != -1) {
      this.isSelectionFileForTranslation = true;
      this.selectedProjectId = this.activatedRoute.snapshot.params["projectId"];
      this.selectedLanguageId = this.activatedRoute.snapshot.params["localeId"];

      this.cols = [
        { field: "name_text", header: "Имя" },
        { field: "date_of_change", header: "Дата изменения", width: "100px" },
        {
          field: "strings_Count",
          header: "Строки",
          width: "100px",
          textalign: "right"
        },
        {
          field: "percent_Of_Translation",
          header: "% предложенных/подтверждённых переводов",
          width: "170px",
          textalign: "center"
        },
        //        {
        //          field: "percent_Of_Confirmed",
        //          header: "Процент подтверждённых переводов",
        //          width: "150px",
        //          textalign: "center"
        //        },
        {}
      ];
    } else {
      this.cols = [
        { field: "name_text", header: "Имя" },
        { field: "date_of_change", header: "Дата изменения" },
        {
          field: "strings_Count",
          header: "Строки",
          width: "100px",
          textalign: "right"
        },
        {
          field: "version",
          header: "Версия",
          width: "80px",
          textalign: "center"
        },
        //{
        //  field: "priority",
        //  header: "Приоритет",
        //  width: "100px",
        //  textalign: "center"
        //},
        {}
      ];
    }

    //    console.log("ProjectName=" + sessionStorage.getItem("ProjectName"));
    //    console.log("ProjectID=" + sessionStorage.getItem("ProjectID"));
    console.log("ProjecID2=" + this.projectsService.currentProjectId);
    //    console.log("ProjecID3=" + this.selectedProjectId);
    //    console.log("Projec=" + sessionStorage.getItem("Projec"));
    this.getFiles();
  }

  pseudoCut(selectedNode: TreeNode): void {
    this.cuttedNode = selectedNode;
  }

  moveCurrentlyCutted(selectedNode: TreeNode): void {
    this.fileService
      .changeParentFolder(this.cuttedNode.data, selectedNode.data.id)
      .subscribe(
        () => {
          this.deleteNode(this.cuttedNode);
          this.addNode(this.cuttedNode, selectedNode);
          this.cuttedNode = null;
        },
        error => {
          this.alertErro(error, "Ошибка вставки");
        }
      );
  }

  moveNode(nodeToMove: any, newParent: any) {
    const parentId = newParent
      ? newParent.data
        ? newParent.data.id
        : null
      : null;
    this.fileService.changeParentFolder(nodeToMove.data, parentId).subscribe(
      () => {
        this.deleteNode(nodeToMove);
        this.addNode(nodeToMove, newParent);
      },
      error => {
        this.alertErro(error, "Ошибка переноса");
      }
    );
  }

  canDrop(node: any) {
    const nodeIsFolder: boolean = node.data.is_folder;
    return droppedNode => {
      return nodeIsFolder && droppedNode.data.id != node.data.id;
    };
  }

  getFiles(): void {
    this.isLoading = true;

    this.fileService
      .getFilesByProjectIdAsTree(
        this.projectsService.currentProjectId,
        this.searchFilesNamesString
      )
      .subscribe(
        files => {
          this.files = files;
          if (this.isSelectionFileForTranslation) {
            this.files.forEach(file => {
              this.fileService
                .getFileTranslationInfos(file.data)
                .subscribe(translations => {
                  translations.forEach(translation => {
                    if (translation.locale_Id == this.selectedLanguageId) {
                      file.data.percent_of_translation =
                        translation.percent_Of_Translation;
                      file.data.percent_of_confirmed =
                        translation.percent_Of_Confirmed;
                    }
                  });
                });
            });
          }
          this.isLoading = false;
        },
        error => this.alertErro(error, "Ошибка получения списка файлов")
      );
  }

  addFolder(newFolder: FileData, parentNode?: TreeNode): void {
    const parentId = parentNode ? parentNode.data.id : null;
    console.log(" ....addFolder");
    this.notifier.notify("success", "Добавление папки");
    this.fileService
      .addFolder(
        newFolder.name_text,
        this.projectsService.currentProjectId,
        parentId
      )
      .subscribe(
        node => this.addNode(node, parentNode),
        error =>
          this.alertErro(
            error,
            "Ошибка добавления папки " + newFolder.name_text
          )
      );
  }

  async uploadFolder(files, parentNode?: TreeNode): Promise<void> {
    const parentId = parentNode ? parentNode.data.id : null;

    this.ngxSpinnerService.show();
    const uploadedFolderName = files[0].webkitRelativePath.slice(
      0,
      files[0].webkitRelativePath.indexOf("/")
    );
    this.dialog.open(UploadingLogModalComponent, {
      data: { name: uploadedFolderName }
    });

    const signalrConnectionId = await this.filesSignalRService.getConnectionId();
    this.fileService
      .uploadFolder(
        files,
        this.projectsService.currentProjectId,
        signalrConnectionId,
        parentId
      )
      .subscribe(
        null,
        error => {
          this.alertErro(error, "Ошибка загрузки ");
        },
        () => {
          this.getFiles();
          this.ngxSpinnerService.hide();
        }
      );
  }

  addFile(file: File, parentNode: TreeNode): void {
    console.log(" ....addFile");
    console.log(" ....parentNode=" + parentNode);
    const parentId = parentNode ? parentNode.data.id : null;
    console.log(" ....parentId=" + parentId);
    this.ngxSpinnerService.show();
    this.fileService
      .addFile(file, this.projectsService.currentProjectId, parentId)
      .subscribe(
        node => {
          console.log(" ....node=" + node);
          this.addNode(node, parentNode);
          console.log(" ....parentNode=" + parentNode);
          this.ngxSpinnerService.hide();
        },
        error => {
          this.alertErro(error, "Ошибка добавления " + file.name);
          this.ngxSpinnerService.hide();

          //this.dialog.open(UploadingLogModalComponent, {
          //  data: { name: file.name, errorMessage: error.error }
          //});
        }
      );
  }

  updateFileVersion(event: any, oldNode: TreeNode): void {
    const file = event.target.files[0];
    if (file) {
      const parentNode = oldNode.parent;
      const parentId = parentNode ? parentNode.data.id : null;
      this.fileService
        .updateFileVersion(
          file,
          oldNode.data.name_text,
          this.projectsService.currentProjectId,
          parentId
        )
        .subscribe(
          newNode => {
            this.deleteNode(oldNode);
            this.addNode(newNode, parentNode);
          },
          error =>
            this.alertErro(
              error,
              "Ошибка обновления версии " + oldNode.data.name_text
            )
        );
    }
  }

  addNode(addedNode: TreeNode, parent: TreeNode): void {
    //console.log("parent=" + parent);
    //console.log("addedNode=" + addedNode);
    //console.log("parent != null =" + parent.children);
    // Nodes (parent children or root)
    const nodes = parent ? [...parent.children] : [...this.files];

    // Find last index in nodes list
    const lastIndex = this.findLastIndex(
      nodes,
      node => node.data.is_folder == addedNode.data.is_folder
    );

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
    this.fileService.deleteNode(node.data).subscribe(
      response => {
        console.log(response);
        this.deleteNode(node);
      },
      error => this.alertErro(error, "Ошибка удаления " + node.data.name_text)
    );
  }

  deleteNode(node: TreeNode) {
    let indexOfNodeInRootFiles = this.files.lastIndexOf(node);
    let nodeIsRoot = indexOfNodeInRootFiles > -1;
    if (nodeIsRoot) {
      this.files.splice(indexOfNodeInRootFiles, 1);
    } else {
      let parentChildren = node.parent.children;
      parentChildren.splice(parentChildren.lastIndexOf(node), 1);
    }
    this.reloadView();
  }

  reloadView() {
    this.files = [...this.files];
  }

  updateNode(data: FileData): void {
    this.fileService
      .updateNode(data)
      .subscribe(
        response => console.log(response),
        error => this.alertErro(error, "Ошибка обновления " + data.name_text)
      );
  }

  onFileUpload(event: any, parentNode?: TreeNode): void {
    const file = event.target.files[0];
    if (file) {
      this.addFile(file, parentNode);
    }
    // event.files.forEach(file => this.fileService.addFile(file).subscribe(node => this.files = [...this.files, node]));
  }

  private findLastIndex(
    nodes: TreeNode[],
    predicate: Predicate<TreeNode>
  ): number {
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
    node.data.name_text = updatedFile.name_text;
    this.fileService.updateNode(node.data).subscribe(
      () => {
        this.reloadView();
      },
      error =>
        this.alertErro(error, "Ошибка переменования " + node.data.name_text)
    );
  }

  requestFileDownload(node: TreeNode) {
    this.ngxSpinnerService.show();
    this.fileService.downloadFile(node.data, this.selectedLanguageId).subscribe(
      data => {
        let fileName = node.data.name_text;
        if (node.data.download_name && node.data.download_name != "") {
          fileName = node.data.download_name;
        }
        if (node.data.is_folder) {
          fileName = fileName + ".zip";
        }
        saveAs(data, fileName);
        this.ngxSpinnerService.hide();
      },
      error => this.alertErro(error, "Ошибка загрузки " + node.data.name_text)
    );
  }

  translateFileClick(selectedFile: any) {
    if (!selectedFile.is_folder) {
      this.router.navigate([
        "Translation/" + selectedFile.id + "/" + this.selectedLanguageId
      ]);
    }
  }

  alertErro(error: any, text: string) {
    this.notifier.notify(
      "error",
      text //+ " {" + error.message + "} {" + error.error + "}"
    );
    //alert(error);
  }
}
