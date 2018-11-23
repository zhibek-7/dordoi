import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';

import { CrowdinComponent } from './crowdin.component';
import { CrowdinRoutingModule } from './crowdin-routing.module';
import { FormsModule }   from '@angular/forms';

import { FilterPipe} from './filter.pipe';
import { OrderByPipe } from './order-by.pipe';

@NgModule({
  declarations: [
    CrowdinComponent, FilterPipe, OrderByPipe
  ],
  imports: [
    CrowdinRoutingModule,
    FormsModule,
    HttpModule,
    CommonModule
  ],
  providers: []
})
export class CrowdinModule { }
