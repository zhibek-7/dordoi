import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRegistrationRoutingModule } from './user-registration-routing.module';
import { CoreModule } from '../core/core.module';

@NgModule({
  imports: [
    CommonModule,
    UserRegistrationRoutingModule,
    CoreModule
  ],
  declarations: []
})
export class UserAccountModule { }
