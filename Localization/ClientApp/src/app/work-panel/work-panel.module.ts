import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { WorkPanelRoutingModule } from './work-panel-routing.module';

import { WorkPanelComponent } from './components/work-panel/work-pandel.component';
import { PhrasesComponent } from './components/phrases/phrases.component';
import { CommentsComponent } from './components/comments/comments.component';
import { LanguagesComponent } from './components/languages/languages.component';
import { TranslationComponent } from './components/translation/translation.component';

import { FilterPhrasesPipe } from './pipes/filter-phrases.pipe';
import { SharePhraseService } from './localServices/share-phrase.service';
import { ShareTranslatedPhraseService } from './localServices/share-translated-phrase.service';
import { TranslationService } from '../services/translationService.service';
import { CommentService } from '../services/comment.service';
import { ProjectsService } from '../services/projects.service';

import {
  MatIconModule,
  MatInputModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatRadioModule,
  MatSelectModule,
  MatOptionModule,
  MatCheckboxModule,
  MatTableModule} from '@angular/material';

@NgModule({
    declarations: [
        WorkPanelComponent,
        PhrasesComponent,
        CommentsComponent,
        LanguagesComponent,
        TranslationComponent,
        FilterPhrasesPipe
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        WorkPanelRoutingModule,
        MatIconModule,
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
