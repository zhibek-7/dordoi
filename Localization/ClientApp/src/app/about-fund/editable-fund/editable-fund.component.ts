import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
//import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { fund } from "../../models/database-entities/fund.type";
import { FundService } from '../../services/fund.service';
//import { FundService } from '../../services/fund.service';
//import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-editable-funds',
  templateUrl: './editable-fund.component.html',
  styleUrls: ['./editable-fund.component.css']
})
export class EditableFundComponent implements OnInit {
  @Input()
  fund: fund;

  @Output()
  editedSubmittedf = new EventEmitter <fund>();


  loaded: boolean = false;

  form: FormGroup;

  constructor(private fundService: FundService) { }


  ngOnInit() {
    this.loaded = true;
    this.initForm();
  
  }

  initForm() {
    this.form = new FormGroup({
      nameMenuFormControl: new FormControl(this.fund.Fund_text, Validators.required),
      fundDescriptionFormControl: new FormControl(this.fund.Fund_description),
    });
   
  }

  async submit() {
    this.getProject();
    this.editedSubmittedf.emit(this.fund);
  }

  getProject() {
    this.fund.Fund_text = this.form.controls.nameMenuFormControl.value;
    this.fund.Fund_description = this.form.controls.fundDescriptionFormControl.value;
  
  }

 
 
}
