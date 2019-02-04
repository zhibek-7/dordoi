import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NotFoundComponent } from './not-found/not-found.component';
import { CurrentProjectSettingsComponent } from '../current-project-settings/current-project-settings.component';
import { ProjectPageComponent } from '../project-page/project-page.component';
import { NewProjectComponent } from '../new-project/new-project.component';
import { UserAccountComponent } from '../user-account/user-account.component';

import { UserRegistrationComponent } from '../user-registration/user-registration.component';
//import { DeleteProjectComponent } from '../create-project/components/delete-project/delete-project.component';

const routes: Routes = [
  {
    path: 'Translation',
    loadChildren: '../work-panel/work-panel.module#WorkPanelModule'
  },
  {
    path: 'Projects/:id/reports',
    loadChildren: '../reports/Reports.model#ReportsModule'
  },
  {
    path: 'create-project',
    loadChildren: '../create-project/create-project.module#CreateProjectModule'
  },
  {
    path: 'Projects/:id',
    component: CurrentProjectSettingsComponent
  },
  {
    path: 'New-project',
    component: NewProjectComponent
  },
  {
    path: 'Profile',
    component: UserAccountComponent
  },
  {
    path: 'Glossaries',
    loadChildren: '../glossaries/glossaries.model#GlossariesModule'
  },
  {
    path: 'list-glossaries',
    loadChildren: '../glossary-list/module/list-glossaries.module#ListGlossariesModule'
  },
  {
    path: 'example',
    loadChildren: '../example/example.model#ExampleModule'
  },
  {
    path: 'example2',
    loadChildren: '../example2/example2.model#Example2Module'
  },
  {
    path: 'Files',
    loadChildren: '../files/files.module#FilesModule'
  },
  {
    path: 'LanguageFiles',
    loadChildren: '../files/files.module#FilesModule'
  },
  {
    path: 'Participants',
    loadChildren: '../participants/participants.module#ParticipantsModule'
  },
  {
    path: 'Strings',
    loadChildren: '../strings/strings.module#StringsModule'
  },
  {
    path: 'Project/:id',
    component: ProjectPageComponent
  },
  //{
  //  path: 'delete/:id',
  //  component: DeleteProjectComponent
  //},
  {
    path: 'account',
    loadChildren: '../account/module/account.module#AccountModule'
  },
  {
    path: 'user-registration',
    component: UserRegistrationComponent
    /*loadChildren: '../user-registration/user-registration.modele.ts#UserRegistrationModule'*/
  },

  {
    path: '**',
    component: UserAccountComponent
    //component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class CoreRoutingModule { }
