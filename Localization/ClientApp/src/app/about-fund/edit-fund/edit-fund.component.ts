import { Component, OnInit } from '@angular/core';
import { FundService } from '../../services/fund.service';
import { fund } from "../../models/database-entities/fund.type";
import { Router } from '@angular/router';
@Component({
  selector: 'app-edit-fund',
  templateUrl: './edit-fund.component.html',
  styleUrls: ['./edit-fund.component.css']
})
export class EditFundComponent implements OnInit {
  /*fund: fund;
  loaded: boolean = false;

  constructor(private fundService: FundService) { }

  ngOnInit() {
    this.fund = new fund(null, null);
    this.fund.DateTime = null;
    //this.fund.ID_User=
    this.loaded = true;
  }

  submit(editedFund: fund) {
    this.fundService.create(editedFund).subscribe(
      fundId => {
        this.fundService.currentFundName = editedFund.Fund_text;
        this.fundService.currentFundDescription = editedFund.Fund_description;

      },
      error => console.error(error)
    );
  }*/



  fund: fund;

  loaded: boolean = false;

  constructor(private projectsService: FundService,
  //  private projectsLocalesService: ProjectsLocalesService,
    private router: Router) { }

  ngOnInit() {
    this.projectsService.getProject(this.projectsService.currentFundId).subscribe(
      project => {
       // this.project = project;
        this.loaded = true;
      },
      error => console.error(error)
    );
  }

  submit(editedProject: fund) {
    this.projectsService.update(editedProject).subscribe(
      result => { },
      error => console.error(error)
    );


  }

  delete(confirm: boolean) {
    if (confirm) {
      this.projectsService.delete(this.fund.id).subscribe(
        result => {
          this.router.navigate(["/Profile"]);
        },
        error => console.error(error)
      );
    }
  }



}
