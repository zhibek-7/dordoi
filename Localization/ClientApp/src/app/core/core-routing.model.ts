import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { NotFoundComponent } from "./not-found/not-found.component";
import { CurrentProjectSettingsComponent } from "../current-project-settings/current-project-settings.component";
import { NewProjectComponent } from "../new-project/new-project.component";
import { UserAccountComponent } from "../user-account/user-account.component";

import { UserRegistrationComponent } from "../user-registration/user-registration.component";

const routes: Routes = [
  {
    path: "administration",
    loadChildren: "../administration/administration.module#AdministrationModule"
  },
  {
    path: "Translation/:fileId",
    loadChildren: "../work-panel/work-panel.module#WorkPanelModule"
  },
  {
    path: "Projects/:id/reports",
    loadChildren: "../reports/Reports.module#ReportsModule"
  },
  {
    path: "create-project",
    loadChildren: "../create-project/create-project.module#CreateProjectModule"
  },
  {
    path: "Projects/:id",
    component: CurrentProjectSettingsComponent
  },
  {
    path: "New-project",
    component: NewProjectComponent
  },
  {
    path: "Profile",
    component: UserAccountComponent
  },
  {
    path: "Glossaries",
    loadChildren: "../glossaries/glossaries.module#GlossariesModule"
  },
  {
    path: "list-glossaries",
    loadChildren:
      "../glossary-list/module/list-glossaries.module#ListGlossariesModule"
  },
  {
    path: "Files",
    loadChildren: "../files/files.module#FilesModule"
  },
  {
    path: "Translation_project/:projectId/LanguageFiles/:localeId",
    loadChildren: "../files/files.module#FilesModule"
  },
  {
    path: "Participants",
    loadChildren: "../participants/participants.module#ParticipantsModule"
  },
  {
    path: "Strings",
    loadChildren: "../strings/strings.module#StringsModule"
  },
  {
    path: "Translation_project/:id",
    loadChildren: "../project-page/project-page.module#ProjectPageModule"
  },
  {
    path: "account",
    loadChildren: "../account/module/account.module#AccountModule"
  },
  {
    path: "user-registration",
    component: UserRegistrationComponent
    /*loadChildren: '../user-registration/user-registration.modele.ts#UserRegistrationModule'*/
  },
  {
    path: "**",
    component: UserAccountComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule {}
