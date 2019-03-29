//import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { CoreRoutingModule } from "./core-routing.model";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AuthenticationGuard } from "../services/authentication.guard";

import { RequestInterceptorService } from "../services/requestInterceptor.service";
import { AuthenticationService } from "../services/authentication.service";
import { UserService } from 'src/app/services/user.service';
import { LanguageService } from 'src/app/services/languages.service';
import { AppInitService } from "src/app/services/app-init.service";

import { HeaderComponent } from "./header/header.component";
import { NotFoundComponent } from "./not-found/not-found.component";

//import { CurrentProjectSettingsComponent } from "../current-project-settings/current-project-settings.component";
import { NewProjectComponent } from "../new-project/new-project.component";
import { NewProfileComponent } from "../new-profile/new-profile.component";
import { UserAccountComponent } from "../user-account/user-account.component";
//import { UserRegistrationComponent } from "../user-registration/user-registration.component";

import {
  MatIconModule,
  MatInputModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatRadioModule,
  MatSelectModule,
  MatOptionModule,
  MatCheckboxModule,
  MatTableModule,
  MatDividerModule,
  MatExpansionModule,
  MatNativeDateModule,
  MatSortModule,
  MatMenuModule,
  MatSlideToggleModule
} from "@angular/material";


@NgModule({
  imports: [
    CommonModule,
    CoreRoutingModule,
    // BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatRadioModule,
    MatOptionModule,
    MatSelectModule,
    MatCheckboxModule,
    MatTableModule,
    MatDividerModule,
    MatExpansionModule,
    ReactiveFormsModule,

    //
    MatNativeDateModule,
    MatSortModule,
    MatMenuModule,
    MatSlideToggleModule
    //
  ],
  declarations: [
    NotFoundComponent,
    HeaderComponent,
    //CurrentProjectSettingsComponent,
    NewProjectComponent,
    UserAccountComponent,
    NewProfileComponent
    //UserRegistrationComponent
  ],
  exports: [RouterModule, HeaderComponent],
  providers: [
    AuthenticationService,
    UserService,
    LanguageService,
    AuthenticationGuard,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    {
      provide: APP_INITIALIZER,
      useFactory: (appInitService: AppInitService) => () => appInitService.initApp(),
      deps: [AppInitService],
      multi: true
    },
  ]
})
export class CoreModule {}
