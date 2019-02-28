import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
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
import { MatPaginatorModule } from '@angular/material/paginator';

import { ProjectPageRoutingModule } from './project-page-routing.module';

import { ProjectPageComponent } from 'src/app/project-page/components/project-page/project-page.component';
import { ActivityListComponent } from './components/activity-list/activity-list.component';

import { LanguageService } from "../services/languages.service";
import { UserService } from "../services/user.service";
import { WorkTypeService } from "../services/workType.service";
import { UserActionsService } from "../services/userActions.service";
import { ParticipantsService } from "../services/participants.service";
import { RequestInterceptorService } from '../services/requestInterceptor.service';

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
    MatPaginatorModule,
  ],
  declarations: [
    ProjectPageComponent,
    ActivityListComponent,
  ],
  providers: [
    LanguageService,
    UserService,
    WorkTypeService,
    UserActionsService,
    ParticipantsService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true }
  ],
})
export class ProjectPageModule { }
