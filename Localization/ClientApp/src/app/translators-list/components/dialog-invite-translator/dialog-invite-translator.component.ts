import { Component, OnInit, Inject, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Guid } from 'guid-typescript';

import { Translator } from 'src/app/models/Translators/translator.type';
import { LocalizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';
import { Invitation } from "src/app/models/database-entities/invitation.type";
import { Role } from "src/app/models/database-entities/role.type";

import { ProjectsService } from 'src/app/services/projects.service';
import { RolesService } from "src/app/services/roles.service";
import { InvitationsService } from "src/app/services/invitations.service";
import { AppConfigService } from "src/services/app-config.service";
import { GuidsService } from "src/app/services/guids.service";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";



@Component({
  selector: 'app-dialog-invite-translator',
  templateUrl: './dialog-invite-translator.component.html',
  styleUrls: ['./dialog-invite-translator.component.css']
})
export class DialogInviteTranslatorComponent extends ModalComponent implements OnInit {

  @Input()
  user: Translator;

  projects: LocalizationProjectForSelectDTO[];
  selectedProjects: LocalizationProjectForSelectDTO[] = [];

  roles: Role[];
  selectedRoleId: Guid;
  
  invitationWithLinks: [Invitation, string][] = [];

  formGroup: FormGroup;
  text = "";
  

  constructor(private projectsService: ProjectsService,
    private invitationsService: InvitationsService,
    private rolesService: RolesService,
    private guidsService: GuidsService,
    private appConfigService: AppConfigService) {
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
      invitationMessageFormControl: new FormControl(this.text)
    });
  }
  
  loadProjects() {
    this.projectsService.getLocalizationProjectForSelectDTOByUser().subscribe(
      localizationProject => this.projects = localizationProject,
      error => console.error(error));
  }

  loadRoles() {
    this.rolesService.getAllRoles().subscribe(
      roles => {
        this.roles = roles;

        if (this.roles.length > 0) 
          this.selectedRoleId = this.roles[0].id;
      },
      error => console.log(error)
    );
  }

  reset() {
    this.invitationWithLinks = [];
    this.text =
      "Добрый день.\r\n" +
      "Мы хотели бы пригласить Вас помочь нам в процессе перевода нашего проекта.\r\n" +
      "\r\n" +
      "Для подключения к проекту необходимо авторизоваться в системе или зарегистрироваться.\r\n" +
      "Регистрация простая и бесплатная.";
    this.formGroup.controls.invitationMessageFormControl.setValue(this.text);
  }

  show() {
    this.reset();
    this.loadProjects();
    this.loadRoles();

    super.show();
  }

  changeSelectedProjects(selected: LocalizationProjectForSelectDTO, event: any) {
    if (event.checked) {
      this.selectedProjects.push(selected);
      this.generateNewInvitationLink(selected);
    }
    else {
      this.selectedProjects = this.selectedProjects.filter(t => t != selected);
      this.invitationWithLinks = this.invitationWithLinks.filter(t => t[0].id_project != selected.id);
    }
  }

  generateNewInvitationLink(project: LocalizationProjectForSelectDTO) {
    this.guidsService.getNew().subscribe(
      newGuid => {
        let invit =
          new Invitation(
            newGuid,
            project.id,
            this.selectedRoleId,
            this.user.user_email,
            this.formGroup.controls.invitationMessageFormControl.value
          );

        const hostProtocol = this.appConfigService.config.host.protocol;
        const hostName = this.appConfigService.config.host.name;
        let link = `${hostProtocol}://${hostName}/invitation/${newGuid}`;

        this.invitationWithLinks.push([invit, link]);
      },
      error => console.log(error)
    );
  }

  saveInvitation() {
    this.invitationWithLinks.forEach(invitationLink => {
      invitationLink[0].id_role = this.selectedRoleId;
      invitationLink[0].message = this.formGroup.controls.invitationMessageFormControl.value;
      this.invitationsService.addInvitation(invitationLink[0]).subscribe();
    });
  }

  submit() {
    this.hide();
    this.saveInvitation();
  }
  
}
