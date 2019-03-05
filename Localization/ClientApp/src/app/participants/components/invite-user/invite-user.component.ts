import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { InvitationsService } from 'src/app/services/invitations.service';
import { GuidsService } from 'src/app/services/guids.service';

import { Role } from 'src/app/models/database-entities/role.type';
import { Invitation } from "src/app/models/database-entities/invitation.type";
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

@Component({
  selector: 'app-invite-user',
  templateUrl: './invite-user.component.html',
  styleUrls: ['./invite-user.component.css']
})
export class InviteUserComponent extends ModalComponent implements OnInit {

  @Input()
  projectId: number;

  @Input()
  roles: Role[];

  selectedRoleId: number = -1;

  invitationId: string = "";

  invitationLink: string = "";

  formGroup: FormGroup;

  constructor(
    private invitationsService: InvitationsService,
    private guidsService: GuidsService,
  ) { super(); }

  ngOnInit() {
    this.formGroup = new FormGroup({
      emailFormControl: new FormControl("", [
        Validators.required,
        Validators.email
      ]),
      invitationMessageFormControl: new FormControl(""),
    });
  }

  show() {
    if (this.roles.length > 0) {
      this.selectedRoleId = this.roles[0].id;
    }
    this.generateNewInvitationLink();
    super.show();
  }

  generateNewInvitationLink() {
    this.guidsService.getNew()
      .subscribe(newGuid => {
        this.invitationId = newGuid;
        this.invitationLink = `${this.getAbsoluteDomainUrl()}/invitation/${this.invitationId}`;
      },
      error => console.log(error));
  }

  public getAbsoluteDomainUrl(): string {
    if (window
      && "location" in window
      && "host" in window.location) {
      return "https://" + window.location.host;
    }
    return null;
  }

  saveInvitation() {
    this.invitationsService.addInvitation(new Invitation(
      this.invitationId,
      this.projectId,
      this.selectedRoleId,
      this.formGroup.controls.emailFormControl.value,
      this.formGroup.controls.invitationMessageFormControl.value
    )).subscribe();
    this.hide();
  }

}
