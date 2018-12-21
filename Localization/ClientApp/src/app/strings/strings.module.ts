import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { StringsRoutingModule } from 'src/app/strings/strings-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { StringsMainComponent } from 'src/app/strings/components/strings-main/strings-main.component';
import { StringsListComponent } from 'src/app/strings/components/strings-list/strings-list.component';

import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
import { LanguageService } from 'src/app/services/languages.service';

@NgModule({
  imports: [
    CommonModule,
    StringsRoutingModule,
    SharedModule,
    FormsModule,
  ],
  declarations: [
    StringsMainComponent,
    StringsListComponent,
  ],
  providers: [
    TranslationSubstringService,
    LanguageService,
  ]
})
export class StringsModule { }
