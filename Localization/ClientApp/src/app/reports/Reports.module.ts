import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http"; //HTTP_INTERCEPTORS
//import { RequestInterceptorService } from "src/app/services/requestInterceptor.service";

import { ReportsRoutingModule } from "./Reports-routing.module";
import { ReportsComponent } from "./Reports.component";
import { TranslatedWordsComponent } from "./TranslatedWords/TranslatedWords.component";
import { ReportService } from "../services/reports.service";
import { LanguageService } from "../services/languages.service";
import { UserService } from "../services/user.service";
import { TypeOfServiceService } from "../services/type-of-service.service";
import {
  MatIconModule,
  MatTableModule,
  MatInputModule,
  MatSelectModule,
  MatButtonModule
} from "@angular/material";
import { NgxDaterangepickerMd } from "ngx-daterangepicker-material";

import { NgxSpinnerModule } from "ngx-spinner";
import { RequestInterceptorService } from "../services/requestInterceptor.service";

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
    NgxDaterangepickerMd.forRoot(),
    NgxSpinnerModule
  ],
  exports: [],
  providers: [
    ReportService,
    LanguageService,
    UserService,
    TypeOfServiceService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true }
  ],
  declarations: [ReportsComponent, TranslatedWordsComponent]
})
export class ReportsModule {}
