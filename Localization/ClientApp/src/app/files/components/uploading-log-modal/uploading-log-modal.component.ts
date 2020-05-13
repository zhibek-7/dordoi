import { Component, ViewEncapsulation, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";

import { FilesSignalRService } from "../../../services/filesSignalR.service";
import { FailedFileParsingModel } from "../../../models/files/failedFileParsing.type";

export interface UploadingLogModalComponentInputData {
  name: string;
  errorMessage?: string;
}

@Component({
  selector: "app-uploading-log-modal",
  templateUrl: "./uploading-log-modal.component.html",
  styleUrls: ["./uploading-log-modal.component.css"],
  encapsulation: ViewEncapsulation.None
})
export class UploadingLogModalComponent {

  parsingFailInfos: FailedFileParsingModel[] = [];

  constructor(
    public dialogRef: MatDialogRef<UploadingLogModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UploadingLogModalComponentInputData,
    private filesSignalRService: FilesSignalRService,
  ) {
    this.filesSignalRService.errorReported.subscribe(
      (parsingFailInfo: FailedFileParsingModel) => {
        this.parsingFailInfos.push(parsingFailInfo);
      });
    if (data.errorMessage) {
      this.parsingFailInfos.push(new FailedFileParsingModel(
        data.name, data.errorMessage
      ));
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}
