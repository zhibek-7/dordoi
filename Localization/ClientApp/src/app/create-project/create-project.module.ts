import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';

import { CreateProjectComponent } from './create-project.component';
import { CreateProjectRoutingModule } from './create-project-routing.module';
import { FormsModule }   from '@angular/forms';

import { FilterPipe} from './filter.pipe';
import { OrderByPipe } from './order-by.pipe';

import { SettingsComponent } from './components/settings/settings.component';
import { DeleteProjectComponent } from './components/delete-project/delete-project.component';
@NgModule({
  declarations: [
    CreateProjectComponent, FilterPipe, OrderByPipe, SettingsComponent, DeleteProjectComponent
  ],
  imports: [
    CreateProjectRoutingModule,
    FormsModule,
    HttpModule,
    CommonModule
  ],
  providers: []
})
export class CreateProjectModule { }
