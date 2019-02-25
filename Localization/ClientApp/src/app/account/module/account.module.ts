import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material';


////// Import the library Dropzone;
import { DropzoneModule } from 'ngx-dropzone-wrapper';
import { DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { DropzoneConfigInterface } from 'ngx-dropzone-wrapper';

const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
  // Change this to your upload POST address:
  //url: '',
  //maxFilesize: 1,
  //acceptedFiles: 'image/*'
};
//////


import { SharedModule } from 'src/app/shared/shared.module';

import { LanguageService } from 'src/app/services/languages.service';

import { AccountRoutingModule } from './account-routing.module';

import { RegistrationComponent } from '../components/registration/registration.component';
import { LoginComponent } from '../components/login/login.component';
import { ProfileComponent } from '../components/profile/profile.component';
import { RecoverPasswordComponent } from '../components/recover-password/recover-password.component';

@NgModule({
  declarations: [
    RegistrationComponent,
    LoginComponent,
    ProfileComponent,
    RecoverPasswordComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,

    SharedModule,

    AccountRoutingModule,

    // Import the library Dropzone
    DropzoneModule,
  ],
  providers:
    [
      LanguageService,
      // Import the library Dropzone
      {
        provide: DROPZONE_CONFIG,
        useValue: DEFAULT_DROPZONE_CONFIG
      }
    ]
})
export class AccountModule { }
