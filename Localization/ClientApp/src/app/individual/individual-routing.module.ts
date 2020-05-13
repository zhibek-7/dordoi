import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddIndividualComponent } from './add-individual/add-individual.component';

const routes: Routes = [
  { path: 'create',
  component: AddIndividualComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndividualRoutingModule { }
