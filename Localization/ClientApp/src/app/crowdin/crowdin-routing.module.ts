import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CrowdinComponent } from './crowdin.component';

const routes: Routes = [
  {
    path: '',
    component: CrowdinComponent

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CrowdinRoutingModule { }
