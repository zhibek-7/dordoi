//import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule} from '@angular/router';

import { HttpClientModule } from '@angular/common/http';

import { CoreRoutingModule } from './core-routing.model';

import { HeaderComponent } from './header/header.component';
import { NotFoundComponent } from './not-found/not-found.component';

import { CurrentProjectSettingsComponent } from '../current-project-settings/current-project-settings.component';
import { NewProjectComponent } from '../new-project/new-project.component';
import { UserAccountComponent } from '../user-account/user-account.component';
import { ProjectPageComponent } from '../project-page/project-page.component';



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
  MatMenuModule} from '@angular/material';

  import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
    imports: [
        CommonModule,
        CoreRoutingModule,
  // BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
   HttpClientModule,
   FormsModule,
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
   ReactiveFormsModule
    ],
  declarations: [
    NotFoundComponent,
    HeaderComponent,
    CurrentProjectSettingsComponent,
    NewProjectComponent,
    UserAccountComponent,
    ProjectPageComponent,
    ],
    exports: [
        RouterModule,
        HeaderComponent
    ]
})
export class CoreModule {}
