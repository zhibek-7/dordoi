import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
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
  MatMenuModule
} from '@angular/material';

import { ListGlossariesRoutingModule } from '../list-glossaries/list-glossaries-routing.module';

import { ListGlossariesComponent } from '../components/list-glossaries/list-glossaries.component';
import { AddGlossaryFormModalComponent } from '../components/add-glossary-form-modal/add-glossary-form-modal.component';
import { GlossaryEditableDetailsComponent } from '../components/glossary-editable-details/glossary-editable-details.component';

import { GlossaryService } from 'src/app/services/glossary.service';
import { LanguageService } from 'src/app/services/languages.service';
import { ProjectsService } from 'src/app/services/projects.service';


import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations:
  [
    ListGlossariesComponent,
    AddGlossaryFormModalComponent,
    GlossaryEditableDetailsComponent
  ],
  imports:
  [
    FormsModule,
    CommonModule,

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
    ProjectsService
  ]
})
export class ListGlossariesModule { }
