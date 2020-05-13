import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FundPageComponent } from './fund-page/fund-page.component';

const routes: Routes = [
  {
    path: "",
    component: FundPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FundPageRoutingModule { }
