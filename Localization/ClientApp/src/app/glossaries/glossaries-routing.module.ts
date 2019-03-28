import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

//import { GlossariesComponent } from 'src/app/glossaries/components/glossaries/glossaries.component';
import { GlossaryDetailsComponent } from './components/glossary-details/glossary-details.component';

const routes: Routes = [
  //{
  //  path: '',
  //  component: GlossariesComponent
  //},
  {
    path: '', //':id',
    component: GlossaryDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GlossariesRoutingModule { }
