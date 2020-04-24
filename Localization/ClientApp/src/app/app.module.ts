import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app/app.component';
import { CoreModule } from './core/core.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { NgxSpinnerModule } from 'ngx-spinner';

import { ContactsComponent } from './contacts/contacts.component';
import { ForCompaniesComponent } from './for-companies/for-companies.component';
import { ForSeniorsComponent } from './for-seniors/for-seniors.component';
import { HomeComponent } from './home/home.component';
import { IndividualComponent } from './individual/individual.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

@NgModule({
  declarations: [
    AppComponent,
   
    ContactsComponent,
    ForCompaniesComponent,
    ForSeniorsComponent,
    HomeComponent,
    IndividualComponent,
    NavMenuComponent,
  ],
  imports: [
    BrowserModule,
    CoreModule,
    BrowserAnimationsModule,
    HttpClientModule,
    NgxSpinnerModule,
  ],
  providers: [
  ],
  bootstrap: [AppComponent],
})
export class AppModule {

    constructor() {}

 }
