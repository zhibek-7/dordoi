import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GlossaryDetailsComponent } from './components/glossary-details/glossary-details.component';

const routes: Routes = [
  {
    path: '', 
    component: GlossaryDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GlossariesRoutingModule { }
