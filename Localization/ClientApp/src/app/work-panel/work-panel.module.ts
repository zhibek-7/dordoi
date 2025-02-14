import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { SharedModule } from '../shared/shared.module';
import { WorkPanelRoutingModule } from './work-panel-routing.module';

import { WorkPanelComponent } from './components/work-panel/work-pandel.component';
import { PhrasesComponent } from './components/phrases/phrases.component';
import { CommentsComponent } from './components/comments/comments.component';
import { LanguagesComponent } from './components/languages/languages.component';
import { TranslationComponent } from './components/translation/translation.component';
import { EditTermFormComponent } from '../glossaries/components/edit-term-form-modal/edit-term-form-modal.component';
import { TermDetailsEditableComponent } from '../glossaries/components/term-details-editable/term-details-editable.component';
import { ContextEditModalComponent } from './components/context-edit-modal/context-edit-modal.component';
import { SelectedWordModalComponent } from './components/selected-word-modal/selected-word-modal.component';
import { ShowImageModalComponent } from './components/show-image-modal/show-image-modal';

import { FilterPhrasesPipe } from './pipes/filter-phrases.pipe';
import { FilterTermsPipe } from './pipes/filter-terms.pipe';
import { SortArrayPipe } from './pipes/sort-array.pipe';
import { SafeHtmlPipe } from './pipes/safeHtml.pipe';

import { SharePhraseService } from './localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from './localServices/share-translated-phrase.service';
import { ShareWordFromModalService } from './localServices/share-word-from-modal.service';
import { TranslationService } from '../services/translationService.service';
import { CommentService } from '../services/comment.service';
import { ProjectsService } from '../services/projects.service';
import { GlossariesService } from '../services/glossaries.service';
import { RequestDataReloadService } from '../glossaries/services/requestDataReload.service';
import { PartsOfSpeechService } from '../services/partsOfSpeech.service';
import { RequestInterceptorService } from '../services/requestInterceptor.service';
import { EditTermFormTranslComponent } from 'src/app/glossaries/components/edit-term-form-modal_transl/edit-term-form-modal.component';


import {
  MatIconModule,
  MatDialogModule,  
  } from '@angular/material';

@NgModule({
    declarations: [
        WorkPanelComponent,
        PhrasesComponent,
        CommentsComponent,
        LanguagesComponent,
        TranslationComponent,
        ContextEditModalComponent,
        SelectedWordModalComponent,
        ShowImageModalComponent,
        TermDetailsEditableComponent,
        FilterPhrasesPipe,
        FilterTermsPipe,
        SortArrayPipe, 
        SafeHtmlPipe,
       EditTermFormTranslComponent
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        WorkPanelRoutingModule,
        SharedModule,
        MatIconModule,
        MatDialogModule
    ],
    exports: [
      
    ],
    entryComponents: [
        SelectedWordModalComponent,
        ShowImageModalComponent
    ],
    providers: [
        SharePhraseService,
        ShareTranslatedPhraseService,
        ShareWordFromModalService,
        TranslationService,
        CommentService,
        ProjectsService,
        GlossariesService,
        RequestDataReloadService,
        PartsOfSpeechService,
        { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    ],
})
export class WorkPanelModule {}
