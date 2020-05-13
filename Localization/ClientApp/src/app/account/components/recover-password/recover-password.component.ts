import { Component, OnInit } from "@angular/core";
import { FormControl, Validators, FormGroup } from "@angular/forms";
import { UserService } from "src/app/services/user.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-recover-password',
  templateUrl: './recover-password.component.html',
  styleUrls: ['./recover-password.component.css']
})
export class RecoverPasswordComponent implements OnInit {

  constructor(private router: Router, private userService: UserService) { }

  formGroup: FormGroup;

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.formGroup = new FormGroup({
      nameFormControl: new FormControl("", Validators.required)
    });
  }

  submit() {
    if (this.formGroup.invalid) {
      return;
    }

    if (this.formGroup.valid) {
      let name = this.formGroup.controls.nameFormControl.value;

      this.userService
        .recoverPassword(name)
        .subscribe(response => {
          response ? this.router.navigate(["/account/.."]) : this.formGroup.setErrors({ notFoundUser: true });
          },
          error => {
            this.formGroup.setErrors({ notFoundUser: true });
          }
        );
    }
  }

}
