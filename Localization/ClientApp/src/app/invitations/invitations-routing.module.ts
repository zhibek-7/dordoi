import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InvitationComponent } from './components/invitation/invitation.component';

const routes: Routes = [
  {
    path: ':invitationId',
    component: InvitationComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvitationsRoutingModule { }
