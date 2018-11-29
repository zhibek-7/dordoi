import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GlossariesRoutingModule } from './glossaries-routing.module';
import { GlossariesComponent } from './glossaries.component';

@NgModule({
  imports: [
    CommonModule,
    GlossariesRoutingModule
  ],
  declarations: [GlossariesComponent]
})
export class GlossariesModule { }
