import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContactRoutingModule } from './contact-routing.module';
import { AddContactComponent } from '../add-contact/add-contact.component';
import { EditableContactComponent } from '../editable-contact/editable-contact.component';
import { EditContactComponent } from '../edit-contact/edit-contact.component';

@NgModule({
  declarations: [AddContactComponent, EditableContactComponent, EditContactComponent],
  imports: [
    CommonModule,
    ContactRoutingModule
  ]
})
export class ContactModule { }
