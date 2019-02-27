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

import { File as FileData } from "src/app/models/database-entities/file.type";

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

  isLoading: boolean;

  // Переменные для работы при выборе файлов для перевода
  isSelectionFileForTranslation: boolean = false;
  selectedProjectId: number;
  selectedLanguageId: number;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fileService: FileService,
    private projectsService: ProjectsService,
    private ngxSpinnerService: NgxSpinnerService,
    private filesSignalRService: FilesSignalRService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    console.log("ProjectName=" + sessionStorage.getItem("ProjectName"));
    console.log("ProjecID=" + sessionStorage.getItem("ProjecID"));
    console.log("Projec=" + sessionStorage.getItem("Projec"));

    if (this.router.url.indexOf("LanguageFiles") != -1) {
      this.isSelectionFileForTranslation = true;
      this.selectedProjectId = this.activatedRoute.snapshot.params["projectId"];
      this.selectedLanguageId = this.activatedRoute.snapshot.params["localeId"];

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
          field: "percent_Of_Translation",
          header: "Процент предложенных переводов",
          width: "130px",
          textalign: "center"
        },
        {
          field: "percent_Of_Confirmed",
          header: "Процент подтверждённых переводов",
          width: "150px",
          textalign: "center"
        },
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
        {
          field: "priority",
          header: "Приоритет",
          width: "100px",
          textalign: "center"
        },
        {}
      ];
    }

    console.log("ProjectName=" + sessionStorage.getItem("ProjectName"));
    console.log("ProjecID=" + sessionStorage.getItem("ProjecID"));
    console.log("Projec=" + sessionStorage.getItem("Projec"));
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
        error => alert(error)
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
      error => alert(error)
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
        error => alert(error)
      );
  }

  addFolder(newFolder: FileData, parentNode?: TreeNode): void {
    const parentId = parentNode ? parentNode.data.id : null;

    console.log(" ....addFolder");
    this.fileService
      .addFolder(
        newFolder.name_text,
        this.projectsService.currentProjectId,
        parentId
      )
      .subscribe(
        node => this.addNode(node, parentNode),
        error => alert(error.error)
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
        error => alert(error),
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
          this.addNode(node, parentNode);
          this.ngxSpinnerService.hide();
        },
        error => alert(error.error)
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
          error => alert(error)
        );
    }
  }

  addNode(addedNode: TreeNode, parent: TreeNode): void {
    console.log("parent=" + parent);
    console.log("addedNode=" + addedNode);
    console.log("parent != null =" + parent.children);
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
      error => alert(error)
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
      .subscribe(response => console.log(response), error => alert(error));
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
      error => alert(error)
    );
  }

  requestFileDownload(node: TreeNode) {
    this.ngxSpinnerService.show();
    this.fileService.downloadFile(node.data).subscribe(
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
      error => alert(error)
    );
  }

  translateFileClick(selectedFile: any) {
    this.router.navigate(["Translation/" + selectedFile.id]);
  }
}
