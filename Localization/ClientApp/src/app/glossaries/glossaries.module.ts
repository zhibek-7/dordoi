import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';
import { MatButtonModule } from '@angular/material';
import { MatDialogModule } from '@angular/material/dialog';

import { GlossariesRoutingModule } from 'src/app/glossaries/glossaries-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { GlossariesService } from 'src/app/services/glossaries.service';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { TranslationService } from 'src/app/services/translationService.service';
import { GlossaryService } from "src/app/services/glossary.service";
import { ProjectsService } from "src/app/services/projects.service";
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

import { GlossaryDetailsComponent } from 'src/app/glossaries/components/glossary-details/glossary-details.component';
import { GlossaryTermsComponent } from 'src/app/glossaries/components/glossary-terms/glossary-terms.component';
import { AddTermFormComponent } from 'src/app/glossaries/components/add-term-form-modal/add-term-form-modal.component';
import { DeleteTermConfirmationComponent } from 'src/app/glossaries/components/delete-term-confirmation-modal/delete-term-confirmation-modal.component';
import { DeleteTermsConfirmationComponent } from 'src/app/glossaries/components/delete-terms-confirmation-modal/delete-terms-confirmation-modal.component';
import { EditTermFormComponent } from 'src/app/glossaries/components/edit-term-form-modal/edit-term-form-modal.component';
import { SetLanguagesFormModalComponent } from 'src/app/glossaries/components/set-languages-form-modal/set-languages-form-modal.component';
import { LanguageService } from 'src/app/services/languages.service';
import { TermDetailsEditableComponent } from 'src/app/glossaries/components/term-details-editable/term-details-editable.component';
import { SetProjectsModalComponent } from "src/app/glossaries/components/set-projects-modal/set-projects-modal.component";


@NgModule({
  imports: [
    CommonModule,
    GlossariesRoutingModule,
    FormsModule,
    SharedModule,
    MatButtonModule,
    MatDialogModule,
    HttpClientModule
  ],
  declarations: [
    GlossaryDetailsComponent,
    GlossaryTermsComponent,
    AddTermFormComponent,
    DeleteTermConfirmationComponent,
    DeleteTermsConfirmationComponent,
    EditTermFormComponent,
    SetLanguagesFormModalComponent,
    TermDetailsEditableComponent,
    SetProjectsModalComponent,
  ],
  providers: [
    RequestDataReloadService,
    GlossariesService,
    PartsOfSpeechService,
    GlossaryService,
    ProjectsService,
    LanguageService,
    TranslationService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
  ],      
  entryComponents: [
    SetProjectsModalComponent,
  ]
})
export class GlossariesModule { }
