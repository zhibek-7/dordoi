import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  MatIconModule,
  MatButtonModule,//+
  MatTableModule,
  MatNativeDateModule,
  MatSortModule,
  MatPaginatorModule,
  MatMenuModule,
  //ниже точно нужные, выше перебрать
  MatOptionModule,
  MatSelectModule,
  MatCheckboxModule,
  MatDialogModule
} from '@angular/material';

import { TranslatorsListComponent } from '../components/translators-list/translators-list.component';
import { DialogInviteTranslatorComponent } from '../components/dialog-invite-translator/dialog-invite-translator.component';

import { TranslationTopicService } from 'src/app/services/translation-topic.service';
import { TypeOfServiceService } from 'src/app/services/type-of-service.service';
//import { TranslatorsService } from 'src/app/services/translators.service';
import { UserService } from 'src/app/services/user.service';
import { LanguageService } from 'src/app/services/languages.service';
import { ItemsSortBy } from 'src/app/translators-list/itemsSortBy.pipe';

import { SharedModule } from 'src/app/shared/shared.module';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

import { TranslatorsListRoutingModule } from './translators-list-routing.module';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';


@NgModule({
  declarations:
    [
      TranslatorsListComponent,
      DialogInviteTranslatorComponent,
      ItemsSortBy
    ],
  imports:
    [
      FormsModule,
      ReactiveFormsModule,
      CommonModule,
      HttpClientModule,

      MatTableModule,
      MatIconModule,
      MatButtonModule,
      MatNativeDateModule,
      MatSortModule,
      MatMenuModule,
      MatPaginatorModule,
      //
      MatOptionModule,
      MatSelectModule,
      MatCheckboxModule,
      MatDialogModule,

      SharedModule,

      TranslatorsListRoutingModule
    ],
  providers:
    [
      TranslationTopicService,
      TypeOfServiceService,
      //TranslatorsService,
      UserService,
      LanguageService,
      RequestDataReloadService,
      { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    ]
})
export class TranslatorsListModule { }
