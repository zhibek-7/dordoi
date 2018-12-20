import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ParticipantsService } from 'src/app/services/participants.service';
import { LanguageService } from 'src/app/services/languages.service';
import { RolesService } from 'src/app/services/roles.service';
import { UserService } from 'src/app/services/user.service';

import { Participant } from 'src/app/models/Participants/participant.type';
import { Role } from 'src/app/models/database-entities/role.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Selectable } from 'src/app/shared/models/selectable.model';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

@Component({
  selector: 'app-participant-details',
  templateUrl: './participant-details.component.html',
  styleUrls: ['./participant-details.component.css']
})
export class ParticipantDetailsComponent extends ModalComponent implements OnInit {

  @Output()
  participantDeleted: EventEmitter = new EventEmitter();

  participant: Participant;

  participantPhoto: any = null;

  isPhotoLoading: boolean = true;

  locales: Locale[] = [];

  constructor(
    private participantsService: ParticipantsService,
    private localesService: LanguageService,
    private usersService: UserService,
  ) { super(); }

  ngOnInit() {
  }

  showParticipant(participant: Participant) {
    this.participant = participant;
    this.loadPhoto();
    this.loadLocales();
    this.show();
  }

  loadPhoto() {
    this.usersService.getPhotoById(this.participant.userId)
      .subscribe(
        imageBlob => {
          let reader = new FileReader();
          reader.addEventListener("load", () => {
            this.participantPhoto = reader.result;
          }, false);

          if (imageBlob) {
            reader.readAsDataURL(imageBlob);
          }

          this.isPhotoLoading = false;
        },
        error => {
          this.isPhotoLoading = false;
          console.log(error);
        });
  }

  loadLocales() {
    this.localesService.getByUserId(this.participant.userId)
      .subscribe(
        locales => this.locales = locales,
        error => console.log(error));
  }

  hide() {
    this.resetData();
    super.hide();
  }

  resetData() {
    this.locales = [];
    this.isPhotoLoading = true;
    this.participantPhoto = null;
  }

  deleteParticpant() {
    this.participantsService.deleteParticipant(this.participant.localizationProjectId, this.participant.userId)
      .subscribe(() => {
          this.participantDeleted.emit();
          this.hide();
        },
        error => console.log(error));
  }

}
