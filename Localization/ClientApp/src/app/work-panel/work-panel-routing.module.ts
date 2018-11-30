import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WorkPanelComponent } from './components/work-panel/work-pandel.component';
 
const routes: Routes = [
    {
        path: '',
        component: WorkPanelComponent
    }
];

@NgModule({
    imports: [ RouterModule.forChild(routes) ],
    exports: [ RouterModule ]
})
export class WorkPanelRoutingModule {}