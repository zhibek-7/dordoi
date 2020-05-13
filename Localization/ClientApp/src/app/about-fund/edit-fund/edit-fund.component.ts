import { Component, OnInit } from '@angular/core';
import { FundService } from '../../services/fund.service';
import { fund } from "../../models/database-entities/fund.type";
import { Router, ActivatedRoute } from '@angular/router';
import { Guid } from "guid-typescript";
@Component({
  selector: 'app-edit-fund',
  templateUrl: './edit-fund.component.html',
  styleUrls: ['./edit-fund.component.css']
})
export class EditFundComponent implements OnInit {
  fund: fund;
  fundId: Guid;
  loaded: boolean = false;

  constructor(private fundsService: FundService,
  //  private projectsLocalesService: ProjectsLocalesService,
    private router: Router, private route: ActivatedRoute) { }

 

  ngOnInit() {
  

    if (this.route.snapshot.params["fundId"] != null) {
      this.fundsService.currentFundId = this.route.snapshot.params["fundId"];
      this.fund.id = this.fundsService.currentFundId;
    }



    this.fundsService.getFund(this.fundsService.currentFundId).subscribe(
      project => {
        this.fund = project;
        this.loaded = true;
      },
      error => console.error(error)
    );





    console.log(".CurrentFundSettingsComponent==" + this.route.snapshot.params["fundId"] + this.route.snapshot.params["fundName"]);
    if (this.route.snapshot.params["fundId"] != null) {
      this.fundsService.currentFundId = this.route.snapshot.params["fundId"];
    }

    console.log("CurrentFundSettingsComponent ngOnInit() fundId: ", this.fundId);
    //console.log("CurrentFundtSettingsComponent ngOnInit() fundName: ", this.fundName);


  }

  submit(editedFund: fund) {
    this.fundsService.update(editedFund).subscribe(
      result => {
        this.fundsService.currentFundId = editedFund.id;
        this.fundsService.currentFundName = editedFund.name_text;
        this.fundsService.currentFundDescription = editedFund.description;
        this.router.navigate(["/Profile"]);
      },
      error => console.error(error)
    );


  }



  delete(confirm: boolean) {
    if (confirm) {
      this.fundsService.delete(this.fund.id).subscribe(
        result => {
          this.router.navigate(["/Profile"]);
        },
        error => console.error(error)
      );
    }
  }



}
