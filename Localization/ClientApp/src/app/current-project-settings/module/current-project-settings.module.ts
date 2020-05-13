import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NotifierModule } from 'angular-notifier';

import {
  MatIconModule,
  MatButtonModule,
  MatTableModule,
  MatSortModule,
  MatMenuModule
} from "@angular/material";

import { RequestInterceptorService } from "src/app/services/requestInterceptor.service";

import { LanguageService } from "src/app/services/languages.service";
import { UserService } from "src/app/services/user.service";

import { CurrentProjectTranslationsComponent } from '../components/current-project-translations/current-project-translations.component';

import { CurrentProjectSettingsRoutingModule } from './current-project-settings-routing.module';

import { CurrentProjectSettingsComponent } from '../components/current-project-settings/current-project-settings.component';


@NgModule({
  declarations: [
    CurrentProjectSettingsComponent,
    CurrentProjectTranslationsComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
    MatTableModule,
    MatSortModule,
    MatMenuModule,
     NotifierModule,
    CurrentProjectSettingsRoutingModule
  ],
  exports: [
    CurrentProjectSettingsRoutingModule,
    CurrentProjectSettingsComponent
  ],
  providers: [
    LanguageService,
    UserService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptorService,
      multi: true
    }
  ]
})
export class CurrentProjectSettingsModule {}
