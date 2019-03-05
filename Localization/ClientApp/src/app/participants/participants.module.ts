import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RequestInterceptorService } from '../services/requestInterceptor.service';

import { ParticipantsRoutingModule } from 'src/app/participants/participants-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { ParticipantsListComponent } from 'src/app/participants/components/participants-list/participants-list.component';
import { ParticipantDetailsComponent } from 'src/app/participants/components/participant-details/participant-details.component';
import { InviteUserComponent } from './components/invite-user/invite-user.component';

import { ParticipantsService } from 'src/app/services/participants.service';
import { LanguageService } from 'src/app/services/languages.service';
import { RolesService } from 'src/app/services/roles.service';
import { UserService } from 'src/app/services/user.service';
import { InvitationsService } from 'src/app/services/invitations.service';
import { GuidsService } from 'src/app/services/guids.service';

@NgModule({
  imports: [
    CommonModule,
    ParticipantsRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatInputModule,
    MatFormFieldModule,
  ],
  declarations: [
    ParticipantsListComponent,
    ParticipantDetailsComponent,
    InviteUserComponent,
  ],
  providers: [
    ParticipantsService,
    LanguageService,
    RolesService,
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    InvitationsService,
    GuidsService,
  ]
})
export class ParticipantsModule { }
