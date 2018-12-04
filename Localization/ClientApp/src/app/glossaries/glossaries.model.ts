import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GlossariesRoutingModule } from './glossaries-routing.module';
import { GlossariesComponent } from './glossaries.component';
import { GlossaryTermsComponent } from './glossary-terms/glossary-terms.component';
import { GlossariesService } from 'src/app/services/glossaries.service';

@NgModule({
  imports: [
    CommonModule,
    GlossariesRoutingModule
  ],
  declarations: [
    GlossariesComponent,
    GlossaryTermsComponent
  ],
  providers: [
    GlossariesService,
  ]
})
export class GlossariesModule { }
