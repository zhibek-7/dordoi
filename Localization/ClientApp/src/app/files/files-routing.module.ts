import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FilesComponent } from './components/files/files.component';

import { AuthenticationGuard } from '../services/authentication.guard';

const routes: Routes = [
  {
    path: '',
    component: FilesComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FilesRoutingModule { }
