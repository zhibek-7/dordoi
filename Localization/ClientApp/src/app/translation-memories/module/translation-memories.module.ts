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

import { TranslationMemoriesRoutingModule } from "./translation-memories-routing.module";

import { TranslationMemoriesComponent } from '../components/translation-memories/translation-memories.component';
import { AddTranslationMemoryFormModalComponent } from '../components/add-translation-memory-form-modal/add-translation-memory-form-modal.component';
import { EditTranslationMemoryFormModalComponent } from '../components/edit-translation-memory-form-modal/edit-translation-memory-form-modal.component';
import { ConfirmDeleteTranslationMemoryComponent } from '../components/confirm-delete-translation-memory/confirm-delete-translation-memory.component';
import { ConfirmClearTranslationMemoryOfStringComponent } from '../components/confirm-clear-translation-memory-of-string/confirm-clear-translation-memory-of-string.component';
import { TranslationMemoryEditableDetailsComponent } from '../components/translation-memory-editable-details/translation-memory-editable-details.component';
import { LoadTranslationMemoryFormModalComponent } from '../components/load-translation-memory-form-modal/load-translation-memory-form-modal.component';


import { TranslationMemoryService } from 'src/app/services/translation-memory.service';
import { LanguageService } from 'src/app/services/languages.service';
import { ProjectsService } from 'src/app/services/projects.service';


import { SharedModule } from 'src/app/shared/shared.module';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';


@NgModule({
  declarations:
  [
    TranslationMemoriesComponent,
    TranslationMemoryEditableDetailsComponent,
    AddTranslationMemoryFormModalComponent,
    EditTranslationMemoryFormModalComponent,
    ConfirmDeleteTranslationMemoryComponent,
    ConfirmClearTranslationMemoryOfStringComponent,
    LoadTranslationMemoryFormModalComponent
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

    TranslationMemoriesRoutingModule
  ],
  providers:
  [
    TranslationMemoryService,
    LanguageService,
    ProjectsService,
    RequestDataReloadService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
  ]
}) //Модуль для памяти переводов (TranslationMemories)
export class TranslationMemoriesModule { }
