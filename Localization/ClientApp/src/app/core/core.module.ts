//import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { CoreRoutingModule } from "./core-routing.model";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AuthenticationGuard } from "../services/authentication.guard";

import { RequestInterceptorService } from "../services/requestInterceptor.service";
import { AuthenticationService } from "../services/authentication.service";
import { UserService } from 'src/app/services/user.service';

import { HeaderComponent } from "./header/header.component";
import { NotFoundComponent } from "./not-found/not-found.component";

//import { CurrentProjectSettingsComponent } from "../current-project-settings/current-project-settings.component";
import { NewProjectComponent } from "../new-project/new-project.component";
import { UserAccountComponent } from "../user-account/user-account.component";
import { UserRegistrationComponent } from "../user-registration/user-registration.component";

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
  MatMenuModule
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
    MatMenuModule
    //
  ],
  declarations: [
    NotFoundComponent,
    HeaderComponent,
    //CurrentProjectSettingsComponent,
    NewProjectComponent,
    UserAccountComponent,
    UserRegistrationComponent
  ],
  exports: [RouterModule, HeaderComponent],
  providers: [AuthenticationService,
              UserService,
              AuthenticationGuard,
              { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
            ]
})
export class CoreModule {}
