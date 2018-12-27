import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from '@angular/common/http';

import { TreeTableModule } from 'primeng/treetable';

import { FilesRoutingModule } from 'src/app/files/files-routing.module';

import { FilesComponent } from 'src/app/files/components/files/files.component';

import { FileService } from 'src/app/services/file.service';

@NgModule({
  declarations: [
    FilesComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    HttpClientModule,
    TreeTableModule,
    FilesRoutingModule,
  ],
  providers: [
    FileService,
  ]
})
export class FilesModule { }
