import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IndividualService } from '../../services/individual.service';
import { individual } from "../../models/database-entities/individual.type";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-editable-individual',
  templateUrl: './editable-individual.component.html',
  styleUrls: ['./editable-individual.component.css']
})
export class EditableIndividualComponent implements OnInit {

  @Input()
  individ: individual;
  individid: string = null;
  @Output()
  editedSubmittedf = new EventEmitter<individual>();


  loaded: boolean = false;

  form: FormGroup;

  constructor(private route: ActivatedRoute, private individService: IndividualService) { }


  ngOnInit() {

    this.loaded = true;
    this.initForm();
  }

  initForm() {



    this.form = new FormGroup({

      nameMenuFormControl1: new FormControl(this.individ.name_text_first, Validators.required),
      DescriptionFormControl1: new FormControl(this.individ.description_first),


      nameMenuFormControl2: new FormControl(this.individ.name_text_second, Validators.required),
      DescriptionFormControl2: new FormControl(this.individ.description_second),


      nameMenuFormControl3: new FormControl(this.individ.name_text_third, Validators.required),
      DescriptionFormControl3: new FormControl(this.individ.description_third),


      nameMenuFormControl4: new FormControl(this.individ.description_fourth, Validators.required),
      DescriptionFormControl4: new FormControl(this.individ.description_fourth),


      nameMenuFormControl5: new FormControl(this.individ.name_text_fifth, Validators.required),
      DescriptionFormControl5: new FormControl(this.individ.description_fifth),
    });
  }

  async submit() {
    this.getFund();
    this.editedSubmittedf.emit(this.individ);
  }


  getFund() {
    
    this.individ.name_text_first = this.form.controls.nameMenuFormControl1.value;
    this.individ.description_first = this.form.controls.DescriptionFormControl1.value;

    this.individ.name_text_second = this.form.controls.nameMenuFormControl2.value;
    this.individ.description_second = this.form.controls.DescriptionFormControl2.value;

    this.individ.name_text_third = this.form.controls.nameMenuFormControl3.value;
    this.individ.description_third = this.form.controls.DescriptionFormControl3.value;

    this.individ.name_text_fourth = this.form.controls.nameMenuFormControl4.value;
    this.individ.description_fourth = this.form.controls.DescriptionFormControl4.value;

    this.individ.name_text_fifth = this.form.controls.nameMenuFormControl5.value;
    this.individ.description_fifth = this.form.controls.DescriptionFormControl5.value;
  }

}
