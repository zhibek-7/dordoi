import { NgModule } from '@angular/core';
import { Routes, RouterModule} from '@angular/router';

import { NotFoundComponent } from './not-found/not-found.component';
import { LoginComponent } from './login/login.component';


const routes: Routes = [
    {
        path: 'work-panel',
        loadChildren: '../work-panel/work-panel.module#WorkPanelModule'
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'admin',
        loadChildren: '../admin/admin.model#AdminModule'
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
