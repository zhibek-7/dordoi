import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Role } from 'src/app/models/database-entities/role.type';

@Injectable()
export class RolesService {

  static connectionUrl: string = 'api/role/';

  constructor(private httpClient: HttpClient) { }

  getAllRoles(): Observable<Role[]> {
    return this.httpClient.post<Role[]>(RolesService.connectionUrl + 'list', null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }

}
