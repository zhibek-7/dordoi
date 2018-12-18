import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PaginationComponent } from 'src/app/shared/components/pagination/pagination.component';
import { SetLanguagesComponent } from 'src/app/shared/components/set-languages/set-languages.component';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

import { FilterSelectableLocalesPipe } from 'src/app/shared/pipes/filterSelectableLocales.pipe';
import { OrderByPipe } from 'src/app/shared/pipes/orderBy.pipe';

@NgModule({
  declarations: [
    PaginationComponent,
    SetLanguagesComponent,
    ModalComponent,
    FilterSelectableLocalesPipe,
    OrderByPipe,
  ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  exports: [
    PaginationComponent,
    SetLanguagesComponent,
    ModalComponent,
  ]
})
export class SharedModule { }
