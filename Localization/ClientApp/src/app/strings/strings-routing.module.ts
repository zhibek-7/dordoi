import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { StringsMainComponent } from 'src/app/strings/components/strings-main/strings-main.component';

import { AuthenticationGuard } from '../services/authentication.guard';

const routes: Routes = [
  {
    path: 'byFileId/:fileId',
    component: StringsMainComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: '',
    component: StringsMainComponent,
    canActivate: [AuthenticationGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StringsRoutingModule { }
