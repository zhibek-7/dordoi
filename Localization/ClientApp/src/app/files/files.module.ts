import { ErrorHandler, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";

import { TreeTableModule } from "primeng/treetable";
import { NgxSpinnerModule } from "ngx-spinner";
import {
  MatMenuModule,
  MatListModule,
  MatIconModule,
  MatButtonModule
} from "@angular/material";
import { MatProgressBarModule } from "@angular/material/progress-bar";
import { MatDialogModule } from "@angular/material/dialog";
import { DndModule } from "../ng2-dnd-src/dnd.module";

import { FilesRoutingModule } from "src/app/files/files-routing.module";
import { SharedModule } from "src/app/shared/shared.module";

import { FilesComponent } from "src/app/files/components/files/files.component";
import { AddFolderModalComponent } from "src/app/files/components/add-folder-modal/add-folder-modal.component";
import { SetLanguagesModalComponent } from "src/app/files/components/set-languages-modal/set-languages-modal.component";
import { RenameFileModalComponent } from "src/app/files/components/rename-file-modal/rename-file-modal.component";
import { TranslationProgressModalComponent } from "src/app/files/components/translation-progress-modal/translation-progress-modal.component";
import { FileSettingsModalComponent } from "src/app/files/components/file-settings-modal/file-settings-modal.component";
import { UploadingLogModalComponent } from "./components/uploading-log-modal/uploading-log-modal.component";

import { FileService } from "src/app/services/file.service";
import { LanguageService } from "src/app/services/languages.service";
import { FilesSignalRService } from "src/app/services/filesSignalR.service";
import { RequestInterceptorService } from "src/app/services/requestInterceptor.service";
import { FileInputWrapper } from "./components/file-input-wrapper/file-input-wrapper.component";
import { NotifierModule } from "angular-notifier";
import { ErrorsHandler } from "src/app/errors-handler";

@NgModule({
  declarations: [
    FilesComponent,
    AddFolderModalComponent,
    SetLanguagesModalComponent,
    RenameFileModalComponent,
    FileInputWrapper,
    TranslationProgressModalComponent,
    FileSettingsModalComponent,
    UploadingLogModalComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    HttpClientModule,
    TreeTableModule,
    FilesRoutingModule,
    SharedModule,
    NgxSpinnerModule,
    MatMenuModule,
    MatListModule,
    DndModule.forRoot(),
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule,
    MatDialogModule,
    NotifierModule.withConfig({
      position: {
        horizontal: {
          position: "right"
        }
      }
    })
  ],
  providers: [
    FileService,
    LanguageService,
    FilesSignalRService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptorService,
      multi: true
    },
    {
      provide: ErrorHandler,
      useClass: ErrorsHandler
    }
  ],
  entryComponents: [UploadingLogModalComponent]
})
export class FilesModule {}
