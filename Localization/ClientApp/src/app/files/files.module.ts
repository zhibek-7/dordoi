import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from '@angular/common/http';

import { TreeTableModule } from 'primeng/treetable';
import { NgxSpinnerModule } from 'ngx-spinner';

import { FilesRoutingModule } from 'src/app/files/files-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { FilesComponent } from 'src/app/files/components/files/files.component';
import { AddFolderModalComponent } from 'src/app/files/components/add-folder-modal/add-folder-modal.component';

import { FileService } from 'src/app/services/file.service';

@NgModule({
  declarations: [
    FilesComponent,
    AddFolderModalComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    HttpClientModule,
    TreeTableModule,
    FilesRoutingModule,
    SharedModule,
    NgxSpinnerModule,
  ],
  providers: [
    FileService,
  ]
})
export class FilesModule { }
