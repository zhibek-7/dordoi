import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { TranslatedWordsRoutingModule } from './TranslatedWords-routing.module';
import { TranslatedWordsComponent } from './TranslatedWords.component';

@NgModule({
  imports: [
    CommonModule,
    TranslatedWordsRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  declarations: [TranslatedWordsComponent]
})
export class TranslatedWordsModule { }
