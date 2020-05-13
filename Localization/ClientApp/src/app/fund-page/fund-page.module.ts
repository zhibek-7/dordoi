import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FundPageRoutingModule } from './fund-page-routing.module';
import { FundPageComponent } from './fund-page/fund-page.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule, MatMenuModule, MatSortModule, MatNativeDateModule, MatExpansionModule, MatDividerModule, MatTableModule, MatCheckboxModule, MatOptionModule, MatSelectModule, MatRadioModule, MatButtonToggleModule, MatButtonModule, MatInputModule, MatIconModule } from '@angular/material';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { LanguageService } from '../services/languages.service';
import { UserService } from '../services/user.service';
import { WorkTypeService } from '../services/workType.service';
import { UserActionsService } from '../services/userActions.service';
import { ParticipantsService } from '../services/participants.service';
import { RequestInterceptorService } from '../services/requestInterceptor.service';

@NgModule({
  declarations: [FundPageComponent],
     imports: [
      CommonModule,
      FundPageRoutingModule,
    FormsModule,
    ReactiveFormsModule,
       FundPageRoutingModule,
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
  
  providers: [
    LanguageService,
    UserService,
    WorkTypeService,
    UserActionsService,
    ParticipantsService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true }
  ],




})
export class FundPageModule { }
