import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ProjectsService } from "../services/projects.service";
import { AuthenticationService } from "../services/authentication.service";
import { Guid } from "guid-typescript";
import { FundService } from '../services/fund.service';
import { fund } from "../models/database-entities/fund.type";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import { UserService } from "src/app/services/user.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  providers: [ProjectsService, UserService, FundService]
})
export class AppComponent implements OnInit {
  funds: fund[];
  currentFund: fund;
  fundid: string = null;
  fundVisibility = false;




  //projects: LocalizationProject[];
  //currentProject: LocalizationProject;
  name: string = null;
  //projectid: string = null;
  //projectVisibility = false;




  userAuthorized: boolean = false;
  currentUserName: string;
  constructor(
    private fundService: FundService,


    private projectService: ProjectsService,
    private authenticationService: AuthenticationService,
    private router: Router,
    public usersService: UserService
  ) {
    this.currentUserName = this.usersService.currentUserName;
  }

  ngOnInit() {
    this.getFund();
  }

  createNewFund() {
    console.log("app createFund ---!!!!!!!!!");
  }

  getFunds() {
    this.checkAuthorization();
    this.fundService.getFunds().subscribe(
      funds => {
        this.funds = funds;
      },
      error => console.error(error)
    );
  }


  getCurrentFund(currentFund: fund) {
    this.currentFund = currentFund;

    this.fundService.currentFundId = currentFund.id;
    this.fundService.currentFundName = currentFund.name_text;
    this.getFund();
  }
  fundcleaning() {
    this.fundService.currentFundId = Guid.createEmpty();
    this.fundService.currentFundName = "";
  }

  getFund() {
    this.name = sessionStorage.getItem("Fund_text");
    this.fundid = sessionStorage.getItem("FundID");
  }



  getFundName() {
    return sessionStorage.getItem("Fund_text");
  }

  getFundId() {
    return sessionStorage.getItem("FundID");
  }


  




  logOut() {
    this.authenticationService.logOut();
    this.name = "";
  }

  checkAuthorization() {
    this.authenticationService.checkUserAuthorisationAsync().subscribe(
      response => {
        this.userAuthorized = response;
      },
      error => {
        console.log("Ошибка: " + error);
        alert("Необходимо авторизироваться");
        this.userAuthorized = false;
        this.router.navigate(["account"]);
      }
    );
  }



  editFund(fund1: fund) {
    this.currentFund = new fund(fund1.name_text, fund1.description);
  }


}
