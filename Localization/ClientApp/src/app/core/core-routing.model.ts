import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ContactsComponent } from 'src/app/contacts/contacts.component';
import { NewProfileComponent } from "../new-profile/new-profile.component";
import { UserAccountComponent } from "../user-account/user-account.component";
import { AboutFundComponent } from 'src/app/about-fund/about-fund/about-fund.component';
import { ForCompaniesComponent } from 'src/app/for-companies/for-companies.component';
import { AuthenticationGuard } from "../services/authentication.guard";
import { ForSeniorsComponent } from 'src/app/for-seniors/for-seniors.component';
import { IndividualComponent } from 'src/app/individual/individual/individual.component';
import { HomeComponent } from 'src/app/home/home.component';
const routes: Routes = [
  {
    path: "administration",
    loadChildren: "../administration/administration.module#AdministrationModule"
  },
  //{ path: 'about-fund', component: AboutFundComponent },
  { path: 'contacts', component: ContactsComponent },
  { path: 'for-companies', component: ForCompaniesComponent },
  { path: 'for-seniors', component: ForSeniorsComponent },
  //{ path: 'individual', component: IndividualComponent },
  { path: 'home', component: HomeComponent },
  {
    path: "Translation/:fileId/:localeId",
    loadChildren: "../work-panel/work-panel.module#WorkPanelModule",
    canActivate: [AuthenticationGuard]
  },
  {
   path: "new-profile",
   component: NewProfileComponent
  },
 
  {
    path: "Projects/:projectId",
    loadChildren: "../current-project-settings/module/current-project-settings.module#CurrentProjectSettingsModule",
    canActivate: [AuthenticationGuard]
  },

  {
    path: "Funds/:fundId",
    loadChildren: "../current-fund-settings/module/current-fund-setting.module#CurrentFundSettingModule",
    canActivate: [AuthenticationGuard]
  },

  {
    path: "Fund",
    loadChildren: "../about-fund/about-fund.module#AboutFundModule"
    //canActivate: [AuthenticationGuard]
  },



  {
    path: "Individual",
    loadChildren: "../individual/individual.module#IndividualModule",
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
