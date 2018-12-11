import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Create_projectComponent } from './Create_project.component';


import { SettingsComponent } from './components/settings/settings.component';


const routes: Routes = [

  {
    path: '',
    component: Create_projectComponent

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


export class Create_projectRoutingModule { }
