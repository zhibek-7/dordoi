import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CurrentFundSettingRoutingModule } from './current-fund-setting-routing.module';
import { CurrentFundSettingComponent } from '../current-fund-setting/current-fund-setting.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatButtonModule, MatTableModule, MatSortModule, MatMenuModule } from '@angular/material';
import { NotifierModule } from 'angular-notifier';
import { LanguageService } from '../../services/languages.service';
import { UserService } from '../../services/user.service';
import { RequestInterceptorService } from '../../services/requestInterceptor.service';

@NgModule({
  declarations: [CurrentFundSettingComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
    MatTableModule,
    MatSortModule,
    MatMenuModule,
    NotifierModule,
    CurrentFundSettingRoutingModule
  ],
  exports: [CurrentFundSettingComponent, CurrentFundSettingRoutingModule],

  providers: [
    LanguageService,
    UserService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptorService,
      multi: true
    }
  ]
})
export class CurrentFundSettingModule { }
