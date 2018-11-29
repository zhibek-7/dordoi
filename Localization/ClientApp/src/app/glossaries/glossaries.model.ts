import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GlossariesRoutingModule } from './glossaries-routing.module';
import { GlossariesComponent } from './glossaries.component';
import { GlossaryTermsComponent } from './glossary-terms/glossary-terms.component';

@NgModule({
  imports: [
    CommonModule,
    GlossariesRoutingModule
  ],
  declarations: [
    GlossariesComponent,
    GlossaryTermsComponent
  ]
})
export class GlossariesModule { }
