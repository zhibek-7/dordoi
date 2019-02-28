import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import {
  MatIconModule,
  MatButtonModule,
  MatTableModule,
  MatNativeDateModule,
  MatSortModule,
  MatPaginatorModule,
  MatMenuModule
} from '@angular/material';

import { ListGlossariesRoutingModule } from './list-glossaries-routing.module';

import { ListGlossariesComponent } from '../components/list-glossaries/list-glossaries.component';
import { GlossaryEditableDetailsComponent } from '../components/glossary-editable-details/glossary-editable-details.component';
import { AddGlossaryFormModalComponent } from '../components/add-glossary-form-modal/add-glossary-form-modal.component';
import { EditGlossaryFormModalComponent } from '../components/edit-glossary-form-modal/edit-glossary-form-modal.component';
import { ConfirmDeleteGlossaryComponent } from '../components/confirm-delete-glossary/confirm-delete-glossary.component';
import { ConfirmClearGlossaryOfTermsComponent } from '../components/confirm-clear-glossary-of-terms/confirm-clear-glossary-of-terms.component';


import { GlossaryService } from 'src/app/services/glossary.service';
import { LanguageService } from 'src/app/services/languages.service';
import { ProjectsService } from 'src/app/services/projects.service';


import { SharedModule } from 'src/app/shared/shared.module';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';

@NgModule({
  declarations:
    [
      ListGlossariesComponent,
      GlossaryEditableDetailsComponent,
      AddGlossaryFormModalComponent,
      EditGlossaryFormModalComponent,
      ConfirmDeleteGlossaryComponent,
      ConfirmClearGlossaryOfTermsComponent
    ],
  imports:
    [
      FormsModule,
      CommonModule,

      MatTableModule,
      MatIconModule,
      MatButtonModule,
      MatNativeDateModule,
      MatSortModule,
      MatMenuModule,
      MatPaginatorModule,

      SharedModule,
      HttpClientModule,

      ListGlossariesRoutingModule
    ],
  providers:
    [
      GlossaryService,
      LanguageService,
      ProjectsService,
      RequestDataReloadService,
      { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    ]
}) //Модуль для глоссария (Glossaries)
export class ListGlossariesModule { }
