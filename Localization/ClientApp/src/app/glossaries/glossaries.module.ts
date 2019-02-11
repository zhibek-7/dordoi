import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { GlossariesRoutingModule } from 'src/app/glossaries/glossaries-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { GlossariesService } from 'src/app/services/glossaries.service';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { TranslationService } from 'src/app/services/translationService.service';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

import { GlossariesComponent } from 'src/app/glossaries/components/glossaries/glossaries.component';
import { GlossaryDetailsComponent } from 'src/app/glossaries/components/glossary-details/glossary-details.component';
import { GlossaryTermsComponent } from 'src/app/glossaries/components/glossary-terms/glossary-terms.component';
import { AddTermFormComponent } from 'src/app/glossaries/components/add-term-form-modal/add-term-form-modal.component';
import { DeleteTermConfirmationComponent } from 'src/app/glossaries/components/delete-term-confirmation-modal/delete-term-confirmation-modal.component';
import { DeleteTermsConfirmationComponent } from 'src/app/glossaries/components/delete-terms-confirmation-modal/delete-terms-confirmation-modal.component';
import { EditTermFormComponent } from 'src/app/glossaries/components/edit-term-form-modal/edit-term-form-modal.component';
import { SetLanguagesFormModalComponent } from 'src/app/glossaries/components/set-languages-form-modal/set-languages-form-modal.component';
import { LanguageService } from 'src/app/services/languages.service';
import { TermDetailsEditableComponent } from 'src/app/glossaries/components/term-details-editable/term-details-editable.component';


@NgModule({
  imports: [
    CommonModule,
    GlossariesRoutingModule,
    FormsModule,
    SharedModule,
  ],
  declarations: [
    GlossariesComponent,
    GlossaryDetailsComponent,
    GlossaryTermsComponent,
    AddTermFormComponent,
    DeleteTermConfirmationComponent,
    DeleteTermsConfirmationComponent,
    EditTermFormComponent,
    SetLanguagesFormModalComponent,
    TermDetailsEditableComponent,
  ],
  providers: [
    RequestDataReloadService,
    GlossariesService,
    PartsOfSpeechService,
    LanguageService,
    TranslationService,
  ]
})
export class GlossariesModule { }
