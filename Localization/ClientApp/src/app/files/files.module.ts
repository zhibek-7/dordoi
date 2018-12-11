import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from '@angular/common/http';

import { TreeTableModule } from 'primeng/treetable';

import { FilesComponent } from './files.component';
import { FileService } from '../services/file.service';
import { FilesRoutingModule } from './files-routing.module';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    FilesComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    HttpClientModule,
    TreeTableModule,
    FilesRoutingModule
  ],
  providers: [FileService]
})
export class FilesModule { }
