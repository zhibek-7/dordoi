import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PaginationComponent } from 'src/app/shared/components/pagination/pagination.component';
import { SetLanguagesComponent } from 'src/app/shared/components/set-languages/set-languages.component';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { FilterSelectableLocalesPipe } from 'src/app/shared/pipes/filterSelectableLocales.pipe';
import { OrderByPipe } from 'src/app/shared/pipes/orderBy.pipe';

import { SetTimeZoneComponent } from './components/set-time-zone/set-time-zone.component';
import { TimeZoneService } from 'src/app/services/time-zone.service';
import { SetLanguagesWithNativeComponent } from './components/set-languages-with-native/set-languages-with-native.component';

@NgModule({
  declarations: [
    PaginationComponent,
    SetLanguagesComponent,
    ModalComponent,
    FilterSelectableLocalesPipe,
    OrderByPipe,
    SetTimeZoneComponent,
    SetLanguagesWithNativeComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  providers:
  [
    TimeZoneService
  ],
  exports: [
    PaginationComponent,
    SetLanguagesComponent,
    ModalComponent,
    SetTimeZoneComponent,
    SetLanguagesWithNativeComponent,
  ]
})
export class SharedModule { }
