import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ListGlossariesComponent } from '../components/list-glossaries/list-glossaries.component';

const routes: Routes = [
  {
    path: '',
    component: ListGlossariesComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ListGlossariesRoutingModule { }
