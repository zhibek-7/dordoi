import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProjectPageComponent } from 'src/app/project-page/components/project-page/project-page.component';

const routes: Routes = [
  {
    path: '',
    component: ProjectPageComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectPageRoutingModule { }
