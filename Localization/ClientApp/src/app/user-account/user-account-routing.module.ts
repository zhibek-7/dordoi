import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserAccountComponent } from './user-account.component';

import { AuthenticationGuard } from '../services/authentication.guard';

const routes: Routes = [
  {
  path: '',
    component: UserAccountComponent,
    canActivate: [AuthenticationGuard]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserAccountRoutingModule { }


