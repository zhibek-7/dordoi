import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { GlossariesRoutingModule } from 'src/app/glossaries/glossaries-routing.module';

import { GlossariesService } from 'src/app/services/glossaries.service';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

import { GlossariesComponent } from 'src/app/glossaries/components/glossaries/glossaries.component';
import { GlossaryTermsComponent } from 'src/app/glossaries/components/glossary-terms/glossary-terms.component';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { AddTermFormComponent } from 'src/app/glossaries/components/add-term-form-modal/add-term-form-modal.component';
import { DeleteTermConfirmationComponent } from 'src/app/glossaries/components/delete-term-confirmation-modal/delete-term-confirmation-modal.component';
import { DeleteTermsConfirmationComponent } from 'src/app/glossaries/components/delete-terms-confirmation-modal/delete-terms-confirmation-modal.component';
import { EditTermFormComponent } from 'src/app/glossaries/components/edit-term-form-modal/edit-term-form-modal.component';

@NgModule({
  imports: [
    CommonModule,
    GlossariesRoutingModule,
    FormsModule,
  ],
  declarations: [
    GlossariesComponent,
    GlossaryTermsComponent,
    AddTermFormComponent,
    ModalComponent,
    DeleteTermConfirmationComponent,
    DeleteTermsConfirmationComponent,
    EditTermFormComponent
  ],
  providers: [
    RequestDataReloadService,
    GlossariesService,
    PartsOfSpeechService
  ]
})
export class GlossariesModule { }
