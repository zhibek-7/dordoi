import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { InvitationsService } from 'src/app/services/invitations.service';

@Component({
  selector: "app-invitation",
  templateUrl: "./invitation.component.html",
  styleUrls: ["./invitation.component.css"]
})
export class InvitationComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private invitationsService: InvitationsService,
  ) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(
      paramMap => {
        const invitationIdParamName = "invitationId";
        if (paramMap.has(invitationIdParamName)) {
          const invitationId = paramMap.get(invitationIdParamName);
          this.invitationsService.currentInvitationId = invitationId;
        }
        this.router.navigate(['account']);
      });
  }

}

