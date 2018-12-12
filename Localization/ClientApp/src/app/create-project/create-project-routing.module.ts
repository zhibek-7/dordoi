import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateProjectComponent } from './create-project.component';


import { SettingsComponent } from './components/settings/settings.component';


const routes: Routes = [

  {
    path: '',
    component: CreateProjectComponent

  },
  {
    path: 'settings',
    component: SettingsComponent

  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})


export class CreateProjectRoutingModule { }
