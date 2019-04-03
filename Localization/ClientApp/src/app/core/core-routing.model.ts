import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { NewProfileComponent } from "../new-profile/new-profile.component";
import { UserAccountComponent } from "../user-account/user-account.component";

import { AuthenticationGuard } from "../services/authentication.guard";

const routes: Routes = [
  {
    path: "administration",
    loadChildren: "../administration/administration.module#AdministrationModule"
  },
  {
    path: "Translation/:fileId/:localeId",
    loadChildren: "../work-panel/work-panel.module#WorkPanelModule",
    canActivate: [AuthenticationGuard]
  },
  {
   path: "new-profile",
   component: NewProfileComponent
  },
  //{
  //  path: "Projects/:id/reports",
  //  loadChildren: "../reports/Reports.module#ReportsModule",
  //  canActivate: [AuthenticationGuard]
  //},
  //{
  //  path: "Projects/:id/stringsTranslationMemory",
  //  loadChildren: "../strings-project/module/strings-project.module#StringsProjectModule",
  //  canActivate: [AuthenticationGuard]
  //},
  {
    path: "Projects/:projectId",
    loadChildren: "../current-project-settings/module/current-project-settings.module#CurrentProjectSettingsModule",
    canActivate: [AuthenticationGuard]
  },
  {
    path: "Profile",
    component: UserAccountComponent,
    canActivate: [AuthenticationGuard]
  },
//  {
//    path: "Glossaries",
//    loadChildren: "../glossaries/glossaries.module#GlossariesModule",
//    canActivate: [AuthenticationGuard]
//  },
  {
    path: "list-glossaries",
    loadChildren:
      "../glossary-list/module/list-glossaries.module#ListGlossariesModule",
    canActivate: [AuthenticationGuard]
  },
//  {
//    path: "Files",
//    loadChildren: "../files/files.module#FilesModule",
//    canActivate: [AuthenticationGuard]
//  },
  {
    path: "Translation_project/:projectId/LanguageFiles/:localeId",
    loadChildren: "../files/files.module#FilesModule",
    canActivate: [AuthenticationGuard]
  },
//  {
//    path: "Participants",
//    loadChildren: "../participants/participants.module#ParticipantsModule",
//    canActivate: [AuthenticationGuard]
//  },
//  {
//    path: "Strings",
//    loadChildren: "../strings/strings.module#StringsModule",
//    canActivate: [AuthenticationGuard]
//  },
  {
    path: "Translation_project/:projectId",
    loadChildren: "../project-page/project-page.module#ProjectPageModule",
    canActivate: [AuthenticationGuard]
  },
  {
    path: "translation-memories",
    loadChildren:
      "../translation-memories/module/translation-memories.module#TranslationMemoriesModule",
    canActivate: [AuthenticationGuard]
  },
  {
    path: "account",
    loadChildren: "../account/module/account.module#AccountModule"
  },
  {
    path: "invitation",
    loadChildren: "../invitations/invitations.module#InvitationsModule",
  },
  {
    path: 'translators',
    loadChildren: "src/app/translators-list/module/translators-list.module#TranslatorsListModule",
    //component: TranslatorsListComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: "project",
    loadChildren: "src/app/project/module/project.module#ProjectModule",
    canActivate: [AuthenticationGuard]
  },
  {
    path: "**",
    loadChildren: "../account/module/account.module#AccountModule",
    canActivate: [AuthenticationGuard]
    //component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule {}
