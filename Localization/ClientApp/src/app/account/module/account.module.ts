import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
//import { FormControl, Validators, AbstractControl, FormGroup } from '@angular/forms';
//import { Router } from '@angular/router';

import { LanguageService } from 'src/app/services/languages.service';

import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatStepperModule
} from '@angular/material';

import { AccountRoutingModule } from './account-routing.module';

import { SharedModule } from 'src/app/shared/shared.module';

import { UserService } from 'src/app/services/user.service';

import { RegistrationComponent } from '../components/registration/registration.component';
import { LoginComponent } from '../components/login/login.component';
import { ProfileComponent } from '../components/profile/profile.component';

@NgModule({
  declarations: [RegistrationComponent, LoginComponent, ProfileComponent],
  imports: [
    CommonModule,
    
    FormsModule,
    ReactiveFormsModule,

    //FormControl,
    //Validators,
    //AbstractControl,
    //FormGroup,

    //MaterialModule,

    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatStepperModule,

    AccountRoutingModule,

    SharedModule
  ],
  providers:
    [
      UserService,
      LanguageService
    ]
})
export class AccountModule { }
