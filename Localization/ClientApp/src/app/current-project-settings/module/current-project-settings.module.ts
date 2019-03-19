import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import {
  MatIconModule,
  MatButtonModule,
  MatTableModule,
  MatSortModule,
  MatMenuModule,
  MatSelectModule,
  MatCheckboxModule,
  MatPaginatorModule,
  MatDialogModule
} from "@angular/material";

import { RequestInterceptorService } from "src/app/services/requestInterceptor.service";

import { LanguageService } from "src/app/services/languages.service";
import { UserService } from "src/app/services/user.service";

import { CurrentProjectTranslationsComponent } from '../components/current-project-translations/current-project-translations.component';
import { CurrentProjectSettingsRoutingModule } from './current-project-settings-routing.module';
import { CurrentProjectSettingsComponent } from '../components/current-project-settings/current-project-settings.component';
import { TranslatorsListComponent, DialogInviteTranslator } from '../../translators-list/translators-list.component';
import { TranslatorsService } from 'src/app/services/translators.service';
import { ItemsSortBy } from 'src/app/translators-list/itemsSortBy.pipe';


@NgModule({
  declarations: [
    CurrentProjectSettingsComponent,
    CurrentProjectTranslationsComponent,
    TranslatorsListComponent,
    DialogInviteTranslator,
    ItemsSortBy
  ],
  entryComponents: [DialogInviteTranslator],
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
    MatSelectModule,
    MatCheckboxModule,
    MatPaginatorModule,
    MatDialogModule,

    CurrentProjectSettingsRoutingModule
  ],
  exports: [
    CurrentProjectSettingsRoutingModule,
    CurrentProjectSettingsComponent,
    TranslatorsListComponent
  ],
  providers: [
    LanguageService,
    UserService,
    TranslatorsService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptorService,
      multi: true
    }
  ]
})
export class CurrentProjectSettingsModule {}
