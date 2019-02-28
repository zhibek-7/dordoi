import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';

import { PaginationComponent } from 'src/app/shared/components/pagination/pagination.component';
import { SetLanguagesComponent } from 'src/app/shared/components/set-languages/set-languages.component';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { FilterSelectableLocalesPipe } from 'src/app/shared/pipes/filterSelectableLocales.pipe';
import { OrderByPipe } from 'src/app/shared/pipes/orderBy.pipe';

import { SetTimeZoneComponent } from './components/set-time-zone/set-time-zone.component';
import { TimeZoneService } from 'src/app/services/time-zone.service';
import { SetLanguagesWithNativeComponent } from './components/set-languages-with-native/set-languages-with-native.component';
import { MainListPageComponent } from './components/main-list-page/main-list-page.component';

@NgModule({
  declarations: [
    PaginationComponent,
    SetLanguagesComponent,
    ModalComponent,
    FilterSelectableLocalesPipe,
    OrderByPipe,
    SetTimeZoneComponent,
    SetLanguagesWithNativeComponent,
    MainListPageComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule
  ],
  providers:
  [
    TimeZoneService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
  ],
  exports: [
    PaginationComponent,
    SetLanguagesComponent,
    ModalComponent,
    SetTimeZoneComponent,
    SetLanguagesWithNativeComponent,
    MainListPageComponent
  ]
})
export class SharedModule { }
