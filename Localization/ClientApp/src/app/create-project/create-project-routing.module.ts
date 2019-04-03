import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateProjectComponent } from './create-project.component';

import { SettingsComponent } from './components/settings/settings.component';
import { DeleteProjectComponent } from './components/delete-project/delete-project.component';
import { AllSettingsComponent } from './components/all-settings/all-settings.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
const routes: Routes = [

  {
    path: '',
    component: CreateProjectComponent

  },
  {
    path: 'settings',
    component: SettingsComponent

  },
  {//  path: 'Projects/:id',
    path: 'delete/:id',
    component: DeleteProjectComponent

  }
  ,
  {
    path: 'all-settings',
    component: AllSettingsComponent

  }
  ,
  {
    path: 'notifications',
    component: NotificationsComponent

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})


export class CreateProjectRoutingModule { }
