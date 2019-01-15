import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GlossariesComponent } from 'src/app/glossaries/components/glossaries/glossaries.component';
import { GlossaryDetailsComponent } from './components/glossary-details/glossary-details.component';

//
//import { ListGlossariesComponent } from 'src/app/glossaries/components/list-glossaries/list-glossaries.component';
//

const routes: Routes = [
//
  //{
  //  path: 'list-glossaries',
  //  component: ListGlossariesComponent
  //},
//
  {
    path: '',
    component: GlossariesComponent
  },
  {
    path: ':id',
    component: GlossaryDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GlossariesRoutingModule { }
