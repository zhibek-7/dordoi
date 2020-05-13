import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { StringsRoutingModule } from 'src/app/strings/strings-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { StringsMainComponent } from 'src/app/strings/components/strings-main/strings-main.component';
import { StringsListComponent } from 'src/app/strings/components/strings-list/strings-list.component';
import { SetLanguagesFormModalComponent } from 'src/app/strings/components/set-languages-form-modal/set-languages-form-modal.component';

import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
import { LanguageService } from 'src/app/services/languages.service';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';

@NgModule({
  imports: [
    CommonModule,
    StringsRoutingModule,
    SharedModule,
    FormsModule,
    HttpClientModule
  ],
  declarations: [
    StringsMainComponent,
    StringsListComponent,
    SetLanguagesFormModalComponent,
  ],
  providers: [
    TranslationSubstringService,
    LanguageService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true }
  ]
})
export class StringsModule { }
