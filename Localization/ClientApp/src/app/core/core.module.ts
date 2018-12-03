//import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule} from '@angular/router';

import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


import { CoreRoutingModule } from './core-routing.model';

import { HeaderComponent } from './header/header.component';
import { NotFoundComponent } from './not-found/not-found.component';

@NgModule({
    imports: [
        CommonModule,
        CoreRoutingModule,
  // BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
   HttpClientModule,
   FormsModule,

    ],
    declarations: [
  	      NotFoundComponent,
        HeaderComponent
    ],
    exports: [
        RouterModule,
        HeaderComponent
    ],
    providers: []
})
export class CoreModule {}
