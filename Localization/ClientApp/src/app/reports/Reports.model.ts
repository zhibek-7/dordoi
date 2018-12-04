import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ReportsRoutingModule } from './Reports-routing.module';
import { ReportsComponent } from "./Reports.component";
import { TranslatedWordsComponent } from "./TranslatedWords/TranslatedWords.component";
import { ReportService  } from "../services/reports.service";

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReportsRoutingModule
  ],
  exports: [
  ],
  providers: [ReportService],
  declarations: [ReportsComponent, TranslatedWordsComponent]
})
export class ReportsModule { }
