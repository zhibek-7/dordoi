import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
  MatMenuModule,
} from '@angular/material';

import { ProjectPageRoutingModule } from './project-page-routing.module';

import { ProjectPageComponent } from 'src/app/project-page/components/project-page/project-page.component';

import { LanguageService } from "../services/languages.service";
import { UserService } from "../services/user.service";
import { WorkTypeService } from "../services/workType.service";
import { UserActionsService } from "../services/userActions.service";
import { ParticipantsService } from "../services/participants.service";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ProjectPageRoutingModule,
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
    MatMenuModule,
    RouterModule,
    HttpClientModule,
  ],
  declarations: [
    ProjectPageComponent,
  ],
  providers: [
    LanguageService,
    UserService,
    WorkTypeService,
    UserActionsService,
    ParticipantsService,
  ],
})
export class ProjectPageModule { }
