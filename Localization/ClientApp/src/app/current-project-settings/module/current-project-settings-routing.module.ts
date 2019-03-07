import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationGuard } from 'src/app/services/authentication.guard';
import { CurrentProjectTranslationsComponent } from '../components/current-project-translations/current-project-translations.component';
import { CurrentProjectSettingsComponent } from '../components/current-project-settings/current-project-settings.component';


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
        loadChildren: "src/app/strings-project/module/strings-project.module#StringsProjectModule",
        canActivate: [AuthenticationGuard]
      },
  {
    path: "Strings",
    loadChildren: "src/app/strings/strings.module#StringsModule",
    canActivate: [AuthenticationGuard]
  },

  {
    path: "Glossaries",
    loadChildren: "../glossaries/glossaries.module#GlossariesModule",
    canActivate: [AuthenticationGuard]
  },
  {
    path: "Participants",
    loadChildren: "../participants/participants.module#ParticipantsModule",
    canActivate: [AuthenticationGuard]
  },
  {
    path: "Files",
    loadChildren: "../files/files.module#FilesModule",
    canActivate: [AuthenticationGuard]
  },
  {
    path: "create-project",
    loadChildren: "../create-project/create-project.module#CreateProjectModule",
    canActivate: [AuthenticationGuard]
  },

      {
        path: '', //path: 'translations',
        component: CurrentProjectTranslationsComponent,
        canActivate: [AuthenticationGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CurrentProjectSettingsRoutingModule { }
