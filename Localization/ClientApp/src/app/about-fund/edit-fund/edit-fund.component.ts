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
  fund: fund;

  loaded: boolean = false;

  constructor(private fundsService: FundService,
  //  private projectsLocalesService: ProjectsLocalesService,
    private router: Router) { }

  ngOnInit() {
    this.fundsService.getFund(this.fundsService.currentFundId).subscribe(
      project => {
       // this.project = project;
        this.loaded = true;
      },
      error => console.error(error)
    );
  }

  submit(editedFund: fund) {
    this.fundsService.update(editedFund).subscribe(
      result => { },
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
