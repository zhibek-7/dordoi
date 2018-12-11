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
import { UserService } from '../services/user.service';
import { MatIconModule, MatTableModule, MatInputModule, MatSelectModule, MatButtonModule, } from '@angular/material';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReportsRoutingModule,
    MatTableModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule,
  NgxDaterangepickerMd
  ],
  exports: [
  ],
  providers: [ReportService, LanguageService, UserService],
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
