import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { WorkPanelRoutingModule } from './work-panel-routing.module';

import { WorkPanelComponent } from './components/work-panel/work-pandel.component';
import { PhrasesComponent } from './components/phrases/phrases.component';
import { CommentsComponent } from './components/comments/comments.component';
import { LanguagesComponent } from './components/languages/languages.component';
import { TranslationComponent } from './components/translation/translation.component';
import { ContextEditModalComponent } from './components/context-edit-modal/context-edit-modal.component';

import { FilterPhrasesPipe } from './pipes/filter-phrases.pipe';
import { SharePhraseService } from './localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from './localServices/share-translated-phrase.service';
import { TranslationService } from '../services/translationService.service';
import { CommentService } from '../services/comment.service';
import { ProjectsService } from '../services/projects.service';

import {
  MatIconModule,  
  MatButtonModule,
  } from '@angular/material';

@NgModule({
    declarations: [
        WorkPanelComponent,
        PhrasesComponent,
        CommentsComponent,
        LanguagesComponent,
        TranslationComponent,
        ContextEditModalComponent,
        FilterPhrasesPipe
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        WorkPanelRoutingModule,
        SharedModule,
        MatIconModule
    ],
    exports: [
      
    ],
    providers: [
        SharePhraseService,
        ShareTranslatedPhraseService,
        TranslationService,
        CommentService,
        ProjectsService
    ],
})
export class WorkPanelModule {}
