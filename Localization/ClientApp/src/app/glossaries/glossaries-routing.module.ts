import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GlossariesComponent } from 'src/app/glossaries/components/glossaries/glossaries.component';

const routes: Routes = [
  {
    path: '',
    component: GlossariesComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GlossariesRoutingModule { }
