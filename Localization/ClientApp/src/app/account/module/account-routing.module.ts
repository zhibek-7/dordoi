import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RegistrationComponent } from '../components/registration/registration.component';
import { LoginComponent } from '../components/login/login.component';
import { ProfileComponent } from '../components/profile/profile.component';
import { RecoverPasswordComponent } from '../components/recover-password/recover-password.component';

const routes: Routes = [
  {
    path: '',
    component: LoginComponent,    
  },
  {
    path: 'recover-password',
    component: RecoverPasswordComponent
  },
  {
    path: 'registration',
    component: RegistrationComponent
  },
  {
    path: 'profile',
    component: ProfileComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
