import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AddProjectComponent } from '../components/add-project/add-project.component';
import { EditProjectComponent } from '../components/edit-project/edit-project.component';
import { EditableProjectComponent } from '../components/editable-project/editable-project.component';
import { ConfirmDeleteProjectComponent } from '../components/confirm-delete-project/confirm-delete-project.component';


import { ProjectsService } from 'src/app/services/projects.service';
import { ProjectsLocalesService } from "src/app/services/projectsLocales.service";
import { LanguageService } from 'src/app/services/languages.service';
import { SharedModule } from 'src/app/shared/shared.module';

import { ProjectRoutingModule } from './project-routing.module';

import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatFormFieldModule,
  MatInputModule,
  MatButtonModule,
  MatCheckboxModule,
  MatRadioModule,
  MatIconModule
} from '@angular/material';

@NgModule({
  declarations:
    [
      AddProjectComponent,
      EditProjectComponent,
      EditableProjectComponent,
      ConfirmDeleteProjectComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatRadioModule,
    MatIconModule,
    
    HttpClientModule,

    SharedModule,

    ProjectRoutingModule
  ],
  providers:
  [
    ProjectsService,
    ProjectsLocalesService,
    LanguageService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true }
  ]
})
export class ProjectModule { }
