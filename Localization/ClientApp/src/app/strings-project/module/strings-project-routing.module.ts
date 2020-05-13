import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { StringsProjectComponent } from "../components/strings-project/strings-project.component";

const routes: Routes = [
  {
    path: '',
    component: StringsProjectComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StringsProjectRoutingModule { }
