import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddFundComponent } from './add-fund/add-fund.component';
import { EditFundComponent } from './edit-fund/edit-fund.component';
import { AboutFundComponent } from './about-fund/about-fund.component';
import { CreateFundComponent } from './create-fund/create-fund.component';



const routes: Routes = [
  {
    path: 'funds',
    component: AboutFundComponent
  },
  {
    path: 'create',
    component: AddFundComponent
  },
  {
    path: 'edit',
    component: EditFundComponent
  },
  {
    path: 'add',
    component: CreateFundComponent
  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AboutFundRoutingModule { }
