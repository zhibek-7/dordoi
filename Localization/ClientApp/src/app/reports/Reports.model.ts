import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ReportsRoutingModule } from './Reports-routing.module';
import { ReportsComponent } from './Reports.component';
import { TranslatedWordsComponent } from './TranslatedWords/TranslatedWords.component';
import { StatusReportComponent } from './status-report/status-report.component';
import { CostsReportComponent } from './costs-report/costs-report.component';
import { TranslationsCostComponent } from './translations-cost/translations-cost.component';
import { FoulsTranslationComponent } from './fouls-translation/fouls-translation.component';
import { ReportService } from '../services/reports.service';
import { LanguageService } from '../services/languages.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatSortModule } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReportsRoutingModule,
  MatTableModule,
  MatPaginatorModule,
  MatSortModule
  ],
  exports: [
    MatTableModule,
    MatPaginatorModule,
    MatSortModule
  ],
  providers: [ReportService, LanguageService],
  declarations: [
    ReportsComponent,
    TranslatedWordsComponent,
    StatusReportComponent,
    CostsReportComponent,
    TranslationsCostComponent,
    FoulsTranslationComponent
  ]
})
export class ReportsModule { }
