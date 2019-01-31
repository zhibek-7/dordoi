import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { StringsMainComponent } from 'src/app/strings/components/strings-main/strings-main.component';

const routes: Routes = [
  {
    path: 'byFileId/:fileId',
    component: StringsMainComponent
  },
  {
    path: '',
    component: StringsMainComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StringsRoutingModule { }
