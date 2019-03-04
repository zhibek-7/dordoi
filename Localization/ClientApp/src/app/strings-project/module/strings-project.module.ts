import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  MatIconModule,
  MatButtonModule,
  MatTableModule,
  MatNativeDateModule,
  MatSortModule,
  MatPaginatorModule,
  MatMenuModule
} from '@angular/material';

import { StringsProjectRoutingModule } from "./strings-project-routing.module";

import { StringsProjectComponent } from "../components/strings-project/strings-project.component";
import { EditStringProjectFormModalComponent } from '../components/edit-string-project-form-modal/edit-string-project-form-modal.component';
import { ConfirmDeleteStringProjectFormModalComponent } from '../components/confirm-delete-string-project-form-modal/confirm-delete-string-project-form-modal.component';
import { LoadStringProjectFormModalComponent } from '../components/load-string-project-form-modal/load-string-project-form-modal.component';
import { AppointTranslationMemoriesProjectFormModalComponent } from '../components/appoint-translation-memories-project-form-modal/appoint-translation-memories-project-form-modal.component';

import { TranslationSubstringService } from "../../services/translationSubstring.service";

import { TranslationMemoryService } from 'src/app/services/translation-memory.service';
import { LanguageService } from 'src/app/services/languages.service';
import { ProjectsService } from 'src/app/services/projects.service';
import { ProjectTranslationMemoryService } from 'src/app/services/projectTranslationMemory.service';


import { SharedModule } from 'src/app/shared/shared.module';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';
import { TranslationService } from 'src/app/services/translationService.service';


@NgModule({
  declarations:
    [
      StringsProjectComponent,
      EditStringProjectFormModalComponent,
      ConfirmDeleteStringProjectFormModalComponent,
      LoadStringProjectFormModalComponent,
      AppointTranslationMemoriesProjectFormModalComponent
    ],
  imports:
    [
      FormsModule,
      CommonModule,
      HttpClientModule,

      MatTableModule,
      MatIconModule,
      MatButtonModule,
      MatNativeDateModule,
      MatSortModule,
      MatMenuModule,
      MatPaginatorModule,

      SharedModule,

      StringsProjectRoutingModule
    ],
  providers:
    [
      TranslationSubstringService,
      TranslationMemoryService,
      LanguageService,
      ProjectsService,
      ProjectTranslationMemoryService,
      TranslationService,
      RequestDataReloadService,
      { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    ]
}) //Модуль для работы со строками текущего проекта (Project-TranslationMemory-TranslationSubstring)
export class StringsProjectModule { }
