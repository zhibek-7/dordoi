import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IndividualRoutingModule } from './individual-routing.module';
import { AddIndividualComponent } from './add-individual/add-individual.component';
import { EditableIndividualComponent } from './editable-individual/editable-individual.component';
import { EditIndividualComponent } from './edit-individual/edit-individual.component';
import { ConfirmDeleteIndividualComponent } from './confirm-delete-individual/confirm-delete-individual.component';
import { IndividualComponent } from './individual/individual.component';
import { SharedModule } from '../shared/shared.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatIconModule, MatRadioModule, MatCheckboxModule, MatButtonModule, MatInputModule, MatFormFieldModule } from '@angular/material';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { IndividualService } from '../services/individual.service';
import { RequestInterceptorService } from '../services/requestInterceptor.service';

@NgModule({
  declarations: [AddIndividualComponent, EditableIndividualComponent, EditIndividualComponent,  ConfirmDeleteIndividualComponent, IndividualComponent],
  imports: [
    CommonModule,

    FormsModule,
    ReactiveFormsModule,

    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatRadioModule,
    MatIconModule,

    HttpClientModule,

    SharedModule,

      IndividualRoutingModule
  ],
  providers:
    [
      IndividualService,
      { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true }
    ]
})
export class IndividualModule { }
