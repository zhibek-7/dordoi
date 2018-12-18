import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ParticipantsListComponent } from 'src/app/participants/components/participants-list/participants-list.component';

const routes: Routes = [
  {
    path: '',
    component: ParticipantsListComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ParticipantsRoutingModule { }
