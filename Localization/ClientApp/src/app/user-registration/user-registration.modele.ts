import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { UserRegistrationRoutingModule } from "./user-registration-routing.module";
import { UserRegistrationComponent } from "./user-registration.component";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    UserRegistrationRoutingModule,
    ReactiveFormsModule
  ],
  declarations: [UserRegistrationComponent],
  bootstrap: [UserRegistrationComponent]
})
export class UserRegistrationModule {}
