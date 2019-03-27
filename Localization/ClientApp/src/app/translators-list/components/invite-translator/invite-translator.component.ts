import { Component, OnInit, Input } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";

import { InvitationsService } from "src/app/services/invitations.service";
import { GuidsService } from "src/app/services/guids.service";

import { Role } from "src/app/models/database-entities/role.type";
import { Invitation } from "src/app/models/database-entities/invitation.type";
import { ModalComponent } from "src/app/shared/components/modal/modal.component";
import { AppConfigService } from "src/services/app-config.service";
import { Guid } from 'guid-typescript';
import { Translator } from "src/app/models/Translators/translator.type";
import { LocalizationProjectForSelectDTO } from "src/app/models/DTO/localizationProjectForSelectDTO.type";
import { RolesService } from "src/app/services/roles.service";

@Component({
  selector: 'app-invite-translator',
  templateUrl: './invite-translator.component.html',
  styleUrls: ['./invite-translator.component.css']
})
export class InviteTranslatorComponent extends ModalComponent implements OnInit {

  @Input()
  user: Translator;

  @Input()
  projects: LocalizationProjectForSelectDTO[];

  
  roles: Role[];

  selectedRoleId: Guid;

  invitations: Invitation[] = [];
  
  invitationLinks: string[] = [];

  formGroup: FormGroup;
  text = "";

  constructor(
    private invitationsService: InvitationsService,
    private rolesService: RolesService,
    private guidsService: GuidsService,
    private appConfigService: AppConfigService
  ) {
    super();
  }

  ngOnInit() {
    this.loadRoles();


    this.text =
      "Добрый день.\r\n" +
      "Мы хотели бы пригласить Вас помочь нам в процессе перевода нашего проекта.\r\n" +
      "\r\n" +
      "Для подключения к проекту необходимо авторизоваться в системе или зарегистрироваться.\r\n" +
      "Регистрация простая и бесплатная.";
    this.formGroup = new FormGroup({
      emailFormControl: new FormControl(this.user.user_email, [
        Validators.required,
        Validators.email
      ]),
      invitationMessageFormControl: new FormControl(this.text)
    });
  }

  reset(){
    //this.roles = [];
    //this.selectedRoleId = null;
    this.invitations = [];
    this.invitationLinks = [];
    this.text = "";
}

  loadRoles() {
    this.rolesService.getAllRoles().subscribe(
      roles => {
        this.roles = roles;
        console.log(roles);
      },
      error => console.log(error)
    );
  }

  show() {
    this.reset();

    if (this.roles.length > 0) {
      this.selectedRoleId = this.roles[0].id;
    }
    this.generateNewInvitationLink();
    super.show();
  }

  generateNewInvitationLink() {

    this.projects.forEach(project => {
    this.guidsService.getNew().subscribe(
      newGuid => {
        this.invitations.push(
        new Invitation(
          newGuid,
          project.id,
          this.selectedRoleId,
          this.formGroup.controls.emailFormControl.value,
          this.formGroup.controls.invitationMessageFormControl.value
        ));
        
        const hostProtocol = this.appConfigService.config.host.protocol;
        const hostName = this.appConfigService.config.host.name;
        this.invitationLinks.push(`${hostProtocol}://${hostName}/invitation/${newGuid}`);
      },
      error => console.log(error)
    );

    });

  }

  saveInvitation() {
    this.invitations.forEach(invitation => {
      invitation.id_role = this.selectedRoleId;
      this.invitationsService.addInvitation(invitation).subscribe();
    });
    this.hide();
  }

}
