import { NgModule } from '@angular/core';
import { Routes, RouterModule} from '@angular/router';

import { NotFoundComponent } from './not-found/not-found.component';
import { CurrentProjectSettingsComponent } from '../current-project-settings/current-project-settings.component';
import { ProjectPageComponent } from '../project-page/project-page.component';
import { NewProjectComponent } from '../new-project/new-project.component';
import { UserAccountComponent } from '../user-account/user-account.component';

const routes: Routes = [
    {
        path: 'Translation',
        loadChildren: '../work-panel/work-panel.module#WorkPanelModule'
    },
    {
      path: 'crowdin',                                          // 1. Поменять название модуля
      loadChildren: '../crowdin/crowdin.module#CrowdinModule'   // 2. Привести организацию модуля к общей структуре(папки, названия и тд)
    },
    {
      path: 'Reports',
      loadChildren: '../reports/Reports.model#ReportsModule'
    },
    {
      path: 'projects/:id',
      component: CurrentProjectSettingsComponent
    },
    {
      path: ':id',
      component: ProjectPageComponent
    },
    {
      path: 'new-project',
      component: NewProjectComponent
    },
   {
      path: 'profile',
      component: UserAccountComponent
    },
    {
      path: 'Glossaries',
      loadChildren: '../glossaries/glossaries.model#GlossariesModule'
    },
    {
        path: '**',
        component: NotFoundComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class CoreRoutingModule {}
