import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationGuard } from 'src/app/services/authentication.guard';
import { CurrentProjectTranslationsComponent } from '../components/current-project-translations/current-project-translations.component';
import { CurrentProjectSettingsComponent } from '../components/current-project-settings/current-project-settings.component';
//import { TranslatorsListComponent } from '../../translators-list/translators-list.component';


const routes: Routes = [
  {
    path: '',
    component: CurrentProjectSettingsComponent,
    canActivate: [AuthenticationGuard],
    children: [
      {
        path: "reports",
        loadChildren: "src/app/reports/Reports.module#ReportsModule",
        canActivate: [AuthenticationGuard]
      },
      {
        path: "stringsTranslationMemory",
        loadChildren:
          "src/app/strings-project/module/strings-project.module#StringsProjectModule",
        canActivate: [AuthenticationGuard]
      },

      {
        path: "Strings",
        loadChildren: "src/app/strings/strings.module#StringsModule",
        canActivate: [AuthenticationGuard]
      },
  {
        path: "Glossaries",
        loadChildren: "src/app/glossaries/glossaries.module#GlossariesModule",
        canActivate: [AuthenticationGuard]
      },
      {
        path: "Participants",
        loadChildren: "src/app/participants/participants.module#ParticipantsModule",
        canActivate: [AuthenticationGuard]
      },
      {
        path: "Files",
        loadChildren: "src/app/files/files.module#FilesModule",
        canActivate: [AuthenticationGuard]
      },

      {
        path: "project",
        loadChildren:
          "src/app/create-project/create-project.module#CreateProjectModule",
        canActivate: [AuthenticationGuard]
      },
      {
        path: "images",
        loadChildren: "src/app/images/images.module#ImagesModule",
        canActivate: [AuthenticationGuard]
      },

      {
        path: "", //path: 'translations',
        component: CurrentProjectTranslationsComponent,
        canActivate: [AuthenticationGuard]
      },

      {
        path: 'translators',
        loadChildren: "src/app/translators-list/module/translators-list.module#TranslatorsListModule",
        //component: TranslatorsListComponent,
        canActivate: [AuthenticationGuard]
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CurrentProjectSettingsRoutingModule { }
