import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";

import { InvitationsService } from "src/app/services/invitations.service";
import { GuidsService } from "src/app/services/guids.service";

import { Role } from "src/app/models/database-entities/role.type";
import { Invitation } from "src/app/models/database-entities/invitation.type";
import { ModalComponent } from "src/app/shared/components/modal/modal.component";
import { AppConfigService } from "src/services/app-config.service";

@Component({
  selector: "app-invite-user",
  templateUrl: "./invite-user.component.html",
  styleUrls: ["./invite-user.component.css"]
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
  text = "";

  constructor(
    private invitationsService: InvitationsService,
    private guidsService: GuidsService,
    private appConfigService: AppConfigService,
  ) {
    super();
  }

  ngOnInit() {
    this.text =
      "Добрый день.\r\n" +
      "Мы хотели бы пригласить Вас помочь нам в процессе перевода нашего проекта.\r\n" +
      "\r\n" +
      "Для подключения к проекту необходимо авторизоваться в системе или зарегистрироваться.\r\n" +
      "Регистрация простая и бесплатная.";
    this.formGroup = new FormGroup({
      emailFormControl: new FormControl("", [
        Validators.required,
        Validators.email
      ]),
      invitationMessageFormControl: new FormControl(this.text)
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
    this.guidsService.getNew().subscribe(
      newGuid => {
        this.invitationId = newGuid;
        const hostProtocol = this.appConfigService.config.host.protocol;
        const hostName = this.appConfigService.config.host.name;
        this.invitationLink = `${hostProtocol}://${hostName}/invitation/${this.invitationId}`;
      },
      error => console.log(error)
    );
  }

  saveInvitation() {
    this.invitationsService
      .addInvitation(
        new Invitation(
          this.invitationId,
          this.projectId,
          this.selectedRoleId,
          this.formGroup.controls.emailFormControl.value,
          this.formGroup.controls.invitationMessageFormControl.value
        )
      )
      .subscribe();
    this.hide();
  }
}
