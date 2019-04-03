import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AddProjectComponent } from '../components/add-project/add-project.component';
import { EditProjectComponent } from '../components/edit-project/edit-project.component';

const routes: Routes = [
  {
    path: 'create',
    component: AddProjectComponent
  },
  {
    path: 'edit',
    component: EditProjectComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})//Модуль для работы с проектом локализации
export class ProjectRoutingModule { }
