import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

//import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { Example2RoutingModule } from './example2-routing.module';
import { Example2Component } from './example2.component';

@NgModule({
  imports: [
    CommonModule,
    Example2RoutingModule,
  //  BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,

  ],
  declarations: [Example2Component]
})
export class Example2Module { }
