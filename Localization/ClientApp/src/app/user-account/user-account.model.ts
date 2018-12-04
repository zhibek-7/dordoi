import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserAccountRoutingModule } from './user-account-routing.module';
import { UserAccountComponent } from './user-account.component';
import { CoreModule } from '../core/core.module';

@NgModule({
  imports: [
    CommonModule,
    UserAccountRoutingModule,
    CoreModule
  ],
  declarations: [UserAccountComponent]
})
export class UserAccountModule { }
