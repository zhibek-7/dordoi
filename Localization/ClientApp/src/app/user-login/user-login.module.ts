import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { UserLoginRoutingModule } from './user-login-routing.module';
import { UserLoginService } from 'src/app/services/user-login.service';
import { UserLoginComponent } from './user-login.component';

@NgModule({
  imports: [CommonModule, FormsModule, UserLoginRoutingModule, ReactiveFormsModule],
  declarations: [UserLoginComponent],
  bootstrap: [UserLoginComponent]
})

export class UserLoginModule { }
