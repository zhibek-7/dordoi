import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { InvitationsRoutingModule } from './invitations-routing.module';

import { InvitationComponent } from './components/invitation/invitation.component';

import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';
import { InvitationsService } from 'src/app/services/invitations.service';

@NgModule({
  declarations: [
    InvitationComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    HttpClientModule,
    InvitationsRoutingModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    InvitationsService,
  ],
})
export class InvitationsModule { }
