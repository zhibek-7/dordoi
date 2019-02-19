import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ParticipantsService } from 'src/app/services/participants.service';
import { UserService } from 'src/app/services/user.service';

import { Role } from 'src/app/models/database-entities/role.type';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { User } from 'src/app/models/database-entities/user.type';

@Component({
  selector: 'app-invite-user',
  templateUrl: './invite-user.component.html',
  styleUrls: ['./invite-user.component.css']
})
export class InviteUserComponent extends ModalComponent implements OnInit {

  @Output()
  participantAdded = new EventEmitter();

  @Input()
  projectId: number;

  @Input()
  roles: Role[];

  selectedRoleId: number;

  users: User[];

  selectedUserId: number;

  constructor(
    private participantsService: ParticipantsService,
    private usersService: UserService,
  ) { super(); }

  ngOnInit() {
  }

  show() {
    if (this.roles.length > 0) {
      this.selectedRoleId = this.roles[0].id;
    }
    this.loadUsers();
    super.show();
  }

  loadUsers() {
    this.usersService.getUserList()
      .subscribe(
        users => {
          this.users = users;
          if (users.length > 0) {
            this.selectedUserId = users[0].id;
          }
        },
        error => console.log(error));
  }

  addParticipant() {
    this.participantsService.addParticipant(this.projectId, this.selectedUserId, this.selectedRoleId)
      .subscribe(
        () => {
          this.hide();
          this.participantAdded.emit();
        },
        error => console.log(error));
  }

}
