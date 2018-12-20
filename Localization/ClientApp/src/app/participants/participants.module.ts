import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ParticipantsRoutingModule } from 'src/app/participants/participants-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { ParticipantsListComponent } from 'src/app/participants/components/participants-list/participants-list.component';
import { ParticipantDetailsComponent } from 'src/app/participants/components/participant-details/participant-details.component';
import { InviteUserComponent } from './components/invite-user/invite-user.component';

import { ParticipantsService } from 'src/app/services/participants.service';
import { LanguageService } from 'src/app/services/languages.service';
import { RolesService } from 'src/app/services/roles.service';
import { UserService } from 'src/app/services/user.service';

@NgModule({
  imports: [
    CommonModule,
    ParticipantsRoutingModule,
    SharedModule,
    FormsModule,
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
  ]
})
export class ParticipantsModule { }
