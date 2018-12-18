import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ParticipantsRoutingModule } from 'src/app/participants/participants-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { ParticipantsListComponent } from 'src/app/participants/components/participants-list/participants-list.component';

@NgModule({
  imports: [
    CommonModule,
    ParticipantsRoutingModule,
    SharedModule,
  ],
  declarations: [
    ParticipantsListComponent,
  ],
})
export class ParticipantsModule { }
