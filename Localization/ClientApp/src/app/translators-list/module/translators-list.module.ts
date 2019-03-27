import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  MatIconModule,
  MatButtonModule,
  MatPaginatorModule,
  MatOptionModule,
  MatSelectModule,
  MatCheckboxModule,
  MatDialogModule,

  MatFormFieldModule,
  MatInputModule
} from '@angular/material';

import { TranslatorsListComponent } from '../components/translators-list/translators-list.component';
import { DialogInviteTranslatorComponent } from '../components/dialog-invite-translator/dialog-invite-translator.component';

import { TranslationTopicService } from 'src/app/services/translation-topic.service';
import { TypeOfServiceService } from 'src/app/services/type-of-service.service';
import { UserService } from 'src/app/services/user.service';
import { LanguageService } from 'src/app/services/languages.service';
import { ProjectsService } from 'src/app/services/projects.service';

import { ItemsSortBy } from 'src/app/translators-list/itemsSortBy.pipe';

import { SharedModule } from 'src/app/shared/shared.module';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';

import { TranslatorsListRoutingModule } from './translators-list-routing.module';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';

import { RolesService } from 'src/app/services/roles.service';
import { InvitationsService } from 'src/app/services/invitations.service';
import { GuidsService } from 'src/app/services/guids.service';


@NgModule({
  declarations:
    [
      TranslatorsListComponent,
      DialogInviteTranslatorComponent,
      ItemsSortBy
    ],
  imports:
    [
      FormsModule,
      ReactiveFormsModule,
      CommonModule,
      HttpClientModule,
      
      MatIconModule,
      MatButtonModule,
      MatPaginatorModule,
      MatOptionModule,
      MatSelectModule,
      MatCheckboxModule,
      MatDialogModule,
      
      MatInputModule,
      MatFormFieldModule,

      SharedModule,

      TranslatorsListRoutingModule
    ],
  providers:
    [
      TranslationTopicService,
      TypeOfServiceService,
      UserService,
      LanguageService,
      ProjectsService,

      InvitationsService,
      GuidsService,
      RolesService,

      RequestDataReloadService,
      { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    ]
})
export class TranslatorsListModule { }
