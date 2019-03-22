import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from "@angular/router";
import {Observable} from "rxjs";
import { Injectable } from '@angular/core';

import { AuthenticationService } from "./authentication.service";
 
@Injectable()
export class AuthenticationGuard implements CanActivate{

    userAuthorized: boolean = false;
 
    constructor(private authService: AuthenticationService, private router: Router) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
      ): Observable<boolean>|Promise<boolean>|boolean {

        this.authService.checkUserAuthorisationAsync()
              .subscribe(response => {
              },
              error => {
                console.log("Ошибка: " + error);
//                alert("Необходимо авторизироваться");
                this.router.navigate(['account']);
              })        
        return true;
      }      
}