import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { fund } from "../../models/database-entities/fund.type";
import { Router } from '@angular/router';
import { FundService } from '../../services/fund.service';
//import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-add-fund',
  templateUrl: './add-fund.component.html',
  styleUrls: ['./add-fund.component.css']
})
export class AddFundComponent implements OnInit {
  fund: fund;
  loaded: boolean = false;
  form: FormGroup;
  constructor(private fundService: FundService, private router: Router) { }

  ngOnInit() {
   this.fund = new fund(null, null);
    //this.fund.DateTime = null;
    //this.fund.ID_User=
   this.loaded = true;
  
  }

  initForm() {
    this.form = new FormGroup({
      nameMenuFormControl: new FormControl(this.fund.name_text, Validators.required),
      fundDescriptionFormControl: new FormControl(this.fund.description),

    });
  }



submit(editedFund: fund) {
    this.fundService.create(editedFund).subscribe(
         fundId => {
       //    this.fundService.currentFundId = fundId;
        this.fundService.currentFundName = editedFund.name_text;
        this.fundService.currentFundDescription = editedFund.description;
        //this.router.navigate(["/Fund"]);
      },
      error => console.error(error)
    );
  }
  



}
