import { Component, OnInit } from "@angular/core";
import { ProjectsService } from "../services/projects.service";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import * as moment from "moment";
import { UserService } from "src/app/services/user.service";
import { FundService } from '../services/fund.service';
import { fund } from "../models/database-entities/fund.type";
moment.locale("ru");

@Component({
  selector: "app-user-account",
  templateUrl: "./user-account.component.html",
  styleUrls: ["./user-account.component.css"],
  providers: [ProjectsService, UserService, FundService]
})
export class UserAccountComponent implements OnInit {
  projectsArr: LocalizationProject[];
  fundsArr: fund[];
  columnsToDisplay = ["fundName", "changed", "settings", "fundMenu"];
  currentUserName = "";


  funds: fund[];
  fund: fund ; 

  constructor(
    private projectsService: ProjectsService,
    private userService: UserService,
    private fundService:  FundService
  ) {}

  getPickedProject(pickedProject) {
    this.projectsService.currentProjectId = pickedProject.id;
    this.projectsService.currentProjectName = pickedProject.name_text;
  }

  getPickedFund(pickedFund) {
    this.fundService.currentFundId = pickedFund.id;
    this.fundService.currentFundName = pickedFund.name_text;
  }


  ngOnInit() {    
    this.currentUserName = this.userService.currentUserName;

    this.fundService.getFunds().subscribe(
      fund => {
        this.fundsArr = fund;
      },
      error => console.error(error)
    );    


 
    //TODO пока не нужно
    //this.projectsArr.forEach((element)=>{element.lastActivity = new Date();});
    // this.projectsArr = this.projectsService.getProjects() - метод возвращающий массив проетов. Отбор по юзеру не реализован
  }

  populateForm(pd: fund) {
    this.fundService.formData = Object.assign({}, pd);
  }
  editFund(f: fund) {
    this.fund = f;
  }


  editedFund(fund1: fund) {
    this.fund = new fund(fund1.name_text, fund1.description);
  }
}
