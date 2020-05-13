import { Component, OnInit } from '@angular/core';
import { individual } from '../../models/database-entities/individual.type';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IndividualService } from '../../services/individual.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-individual',
  templateUrl: './add-individual.component.html',
  styleUrls: ['./add-individual.component.css']
})
export class AddIndividualComponent implements OnInit {


  individ: individual;
  loaded: boolean = false;
  form: FormGroup;
  constructor(private individService: IndividualService, private router: Router) { }

  ngOnInit() {
    this.individ = new individual(null, null, null, null, null, null, null, null, null, null);
    //this.fund.DateTime = null;
    //this.fund.ID_User=
    this.loaded = true;

  }

  initForm() {
    this.form = new FormGroup({
      nameMenuFormControl1: new FormControl(this.individ.name_text_first, Validators.required),
      DescriptionFormControl1: new FormControl(this.individ.description_first),

      nameMenuFormControl2: new FormControl(this.individ.name_text_second, Validators.required),
      DescriptionFormControl2: new FormControl(this.individ.description_second),

      nameMenuFormControl3: new FormControl(this.individ.name_text_third, Validators.required),
      DescriptionFormControl3: new FormControl(this.individ.description_third),

      nameMenuFormControl4: new FormControl(this.individ.name_text_fourth, Validators.required),
      DescriptionFormControl4: new FormControl(this.individ.description_fourth),

      nameMenuFormControl5: new FormControl(this.individ.name_text_fifth, Validators.required),
      DescriptionFormControl5: new FormControl(this.individ.description_fifth),

    });
  }



  submit(editedIndivid: individual) {
    this.individService.create(editedIndivid).subscribe(
      fundId => {
       
        this.individService.currentIndivName_first = editedIndivid.name_text_first;
        this.individService.currentIndivDescription_first = editedIndivid.description_first;

        this.individService.currentIndivName_second = editedIndivid.name_text_second;
        this.individService.currentIndivDescription_second = editedIndivid.description_second;


        this.individService.currentIndivName_third = editedIndivid.name_text_third;
        this.individService.currentIndivDescription_third = editedIndivid.description_third;


        this.individService.currentIndivName_fourth = editedIndivid.name_text_fourth;
        this.individService.currentIndivDescription_fourth = editedIndivid.description_fourth;


        this.individService.currentIndivName_fifth = editedIndivid.name_text_fifth;
        this.individService.currentIndivDescription_fifth = editedIndivid.description_fifth;



        this.router.navigate(["/Profile"]);
      },
      error => console.error(error)
    );
  }



}
