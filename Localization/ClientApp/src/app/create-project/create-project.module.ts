import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RequestInterceptorService } from '../services/requestInterceptor.service';

import { CreateProjectComponent } from './create-project.component';
import { CreateProjectRoutingModule } from './create-project-routing.module';
import { FormsModule }   from '@angular/forms';

import { FilterPipe} from './filter.pipe';
import { OrderByPipe } from './order-by.pipe';

import { SettingsComponent } from './components/settings/settings.component';
import { DeleteProjectComponent } from './components/delete-project/delete-project.component';
import { AllSettingsComponent } from './components/all-settings/all-settings.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
//import { MenuProjectComponent } from './components/menu-project/menu-project.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material';
import {
  MatInputModule, MatCheckboxModule,
  MatSelectModule,
  MatRadioModule,
  MatMenuModule,
  MatToolbarModule,
  MatListModule,
  MatGridListModule,
  MatCardModule,
  MatTabsModule,
  MatButtonModule,
  MatChipsModule,
  MatIconModule,
  MatProgressSpinnerModule,
  MatProgressBarModule,
  MatDialogModule,
  MatTooltipModule,
  MatSnackBarModule,
  MatSlideToggleModule,
  MatTableModule,
  MatPaginatorModule,
  MatSortModule,
  MatAutocompleteModule,
  MatExpansionModule
} from '@angular/material';

import { EditProjectComponent } from './components/edit-project/edit-project.component';
import { LanguageService } from '../services/languages.service';
import { ProjectsLocalesService } from '../services/projectsLocales.service';
import { ProjectsService } from '../services/projects.service';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    CreateProjectComponent
    , FilterPipe
    , OrderByPipe
    , SettingsComponent
    , DeleteProjectComponent,
    AllSettingsComponent,
    NotificationsComponent,
  //  , MenuProjectComponent

    EditProjectComponent
  ],
  imports: [
    CreateProjectRoutingModule,
    FormsModule,
    HttpModule,
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatSelectModule,
    MatRadioModule,
    MatMenuModule,
    MatToolbarModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatTabsModule,
    MatButtonModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatDialogModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatAutocompleteModule,
    MatExpansionModule,
    HttpClientModule,

    SharedModule
  ],
  providers: [
    ProjectsService,
    LanguageService,
    ProjectsLocalesService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
  ]
})
export class CreateProjectModule { }
