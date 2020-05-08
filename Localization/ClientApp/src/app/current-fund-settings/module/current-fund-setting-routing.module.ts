import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationGuard } from 'src/app/services/authentication.guard';
import { CurrentFundSettingComponent } from '../../current-fund-settings/current-fund-setting/current-fund-setting.component';

const routes: Routes = [
  {
    path: '',
    component: CurrentFundSettingComponent,
    canActivate: [AuthenticationGuard],
    children: [
      {
        path: "fund",
        loadChildren: "src/app/about-fund/about-fund.module#AboutFundModule",
        canActivate: [AuthenticationGuard]
      }]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CurrentFundSettingRoutingModule { }
