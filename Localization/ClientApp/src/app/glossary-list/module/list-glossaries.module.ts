import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  //Убрать не нужные
  MatIconModule,
  MatInputModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatRadioModule,
  MatSelectModule,
  MatOptionModule,
  MatCheckboxModule,
  MatTableModule,
  MatDividerModule,
  MatExpansionModule,
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

      //Убрать не нужные
      MatIconModule,
      MatInputModule,
      MatButtonModule,
      MatButtonToggleModule,
      MatRadioModule,
      MatOptionModule,
      MatSelectModule,
      MatCheckboxModule,
      MatTableModule,
      MatDividerModule,
      MatExpansionModule,
      MatPaginatorModule,
      //

      MatNativeDateModule,
      MatSortModule,
      MatMenuModule,

      ListGlossariesRoutingModule,

      SharedModule
    ],
  providers:
    [
      GlossaryService,
      LanguageService,
      ProjectsService,
      RequestDataReloadService
    ]
})
export class ListGlossariesModule { }
