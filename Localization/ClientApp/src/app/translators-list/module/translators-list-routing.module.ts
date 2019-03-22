import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TranslatorsListComponent } from '../components/translators-list/translators-list.component';

const routes: Routes = [
  {
    path: '',
    component: TranslatorsListComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TranslatorsListRoutingModule { }
