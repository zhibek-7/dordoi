import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthenticationService {

  private url: string = 'api/User/';

  constructor(private http: HttpClient) { }

  checkUserAuthorisation(): Observable<boolean>{
    return this.http.post<boolean>(this.url + "checkUserAuthorisation", null);
  }

  saveToken(token: string){
    sessionStorage.setItem('userToken', token);  
  }
  
  deleteToken(){
    sessionStorage.clear();
  }

}