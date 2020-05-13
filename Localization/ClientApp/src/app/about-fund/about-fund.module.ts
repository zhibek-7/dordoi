import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AboutFundRoutingModule } from './about-fund-routing.module';
import { AboutFundComponent } from './about-fund/about-fund.component';
import { EditableFundComponent } from './editable-fund/editable-fund.component';
import { EditFundComponent } from './edit-fund/edit-fund.component';
import { AddFundComponent } from './add-fund/add-fund.component';
import { FundService } from '../services/fund.service';


import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from '../services/requestInterceptor.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule, MatInputModule, MatButtonModule, MatCheckboxModule, MatRadioModule, MatIconModule } from '@angular/material';
import { SharedModule } from '../shared/shared.module';
import { CreateFundComponent } from './create-fund/create-fund.component';
import { ConfirmDeleteFundComponent } from './confirm-delete-fund/confirm-delete-fund.component';

@NgModule({
  declarations: [AboutFundComponent, EditableFundComponent, EditFundComponent, AddFundComponent, CreateFundComponent, ConfirmDeleteFundComponent],
  
  imports: [
    CommonModule,
    
    FormsModule,
    ReactiveFormsModule,

    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatRadioModule,
    MatIconModule,

    HttpClientModule,

    SharedModule,

    AboutFundRoutingModule
  ],
  providers:
    [
      FundService,
      { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true }
    ]
})
export class AboutFundModule { }
