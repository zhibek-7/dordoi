import { NgModule } from '@angular/core';
import { Routes, RouterModule} from '@angular/router';

import { NotFoundComponent } from './not-found/not-found.component';

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
      path: 'TranslatedWordsReport',
      loadChildren: '../reports/TranslatedWords/TranslatedWords.model#TranslatedWordsModule'
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
        path: '**',
        component: NotFoundComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class CoreRoutingModule {}
