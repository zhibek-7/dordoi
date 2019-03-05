import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

import { Invitation } from "src/app/models/database-entities/invitation.type";

@Injectable({
  providedIn: "root"
})
export class InvitationsService {
  private _url = "api/invitations";

  private readonly invitationIdItemName = "invitationId";

  constructor(private http: HttpClient) { }

  get currentInvitationId(): string {
    return sessionStorage.getItem(this.invitationIdItemName);
  }

  set currentInvitationId(newInvitationId: string) {
    sessionStorage.setItem(this.invitationIdItemName, newInvitationId);
  }

  addInvitation(invitation: Invitation): Observable<Object> {
    return this.http
      .post(`${this._url}/add`, invitation, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      });
  }

  getInvitationById(invitationId: string): Observable<Invitation> {
    return this.http
      .post<Invitation>(`${this._url}/${invitationId}`, null, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      });
  }

}
