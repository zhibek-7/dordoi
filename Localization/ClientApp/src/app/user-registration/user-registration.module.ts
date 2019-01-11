import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { UserRegistrationRoutingModule } from './user-registration-routing.module';
import { UserRegistrationService } from 'src/app/services/user-registration.service';
import { UserRegistrationComponent } from './user-registration.component';

@NgModule({
  imports: [CommonModule, FormsModule, UserRegistrationRoutingModule, ReactiveFormsModule],
  declarations: [UserRegistrationComponent],
  bootstrap: [UserRegistrationComponent]
})

export class UserRegistrationModule {}
