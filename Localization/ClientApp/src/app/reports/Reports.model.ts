import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ReportsRoutingModule } from './Reports-routing.module';
import { ReportsComponent } from"./Reports.component";

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReportsRoutingModule
  ],
  exports: [
  ],
  declarations: [ReportsComponent]
})
export class ReportsModule { }
