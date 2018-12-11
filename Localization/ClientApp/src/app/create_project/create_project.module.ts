import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';

import { Create_projectComponent } from './Create_project.component';
import { Create_projectRoutingModule } from './create_project-routing.module';
import { FormsModule }   from '@angular/forms';

import { FilterPipe} from './filter.pipe';
import { OrderByPipe } from './order-by.pipe';

import { SettingsComponent } from './components/settings/settings.component';
@NgModule({
  declarations: [
    Create_projectComponent, FilterPipe, OrderByPipe, SettingsComponent
  ],
  imports: [
    Create_projectRoutingModule,
    FormsModule,
    HttpModule,
    CommonModule
  ],
  providers: []
})
export class Create_projectModule { }
