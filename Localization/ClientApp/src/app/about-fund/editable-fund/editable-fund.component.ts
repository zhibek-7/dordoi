import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
//import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { fund } from "../../models/database-entities/fund.type";
import { FundService } from '../../services/fund.service';
//import { FundService } from '../../services/fund.service';
import { Guid } from "guid-typescript";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-editable-funds',
  templateUrl: './editable-fund.component.html',
  styleUrls: ['./editable-fund.component.css']
})
export class EditableFundComponent implements OnInit {
  @Input()
  fund: fund;
  fundid: string = null;
  @Output()
  editedSubmittedf = new EventEmitter <fund>();


  loaded: boolean = false;

  form: FormGroup;

  constructor(private route: ActivatedRoute,private fundService: FundService) { }


  ngOnInit() {

  
    //console.log(".CurrentFundSettingsComponent==" + this.route.snapshot.params["fundId"] + this.route.snapshot.params["fundName"]);
    //if (this.route.snapshot.params["fundId"] != null) {
    //  this.fundService.currentFundId = this.route.snapshot.params["fundId"];
    //  this.fund.id = this.fundService.currentFundId;
    //}

    //console.log("CurrentFundSettingsComponent ngOnInit() fundId: ", this.fundService.currentFundId = this.route.snapshot.params["fundId"]);


    //this.fund.id = Guid.parse(sessionStorage.getItem("FundID"));
    this.loaded = true;
    this.initForm();
  }

  initForm() {



    this.form = new FormGroup({
    //  idMenuFormControl : new FormControl(this.fund.id),
      nameMenuFormControl: new FormControl(this.fund.name_text, Validators.required),
      fundDescriptionFormControl: new FormControl(this.fund.description)
    });   
  }

  async submit() {
    this.getFund();
    this.editedSubmittedf.emit(this.fund);
  }


  getFund() {
   // this.fund.id = this.fundService.currentFundId;
    this.fund.name_text = this.form.controls.nameMenuFormControl.value;
    this.fund.description = this.form.controls.fundDescriptionFormControl.value;
  }

 
 
}
