import { Component, OnInit, Output, EventEmitter } from "@angular/core";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";

import { GlossariesForEditing } from "src/app/models/DTO/glossariesDTO.type";

@Component({
  selector: "app-add-glossary-form-modal",
  templateUrl: "./add-glossary-form-modal.component.html",
  styleUrls: ["./add-glossary-form-modal.component.css"]
})
export class AddGlossaryFormModalComponent extends ModalComponent
  implements OnInit {
  newGlossary: GlossariesForEditing;

  @Output()
  newGlossarySubmitted = new EventEmitter<GlossariesForEditing>();

  constructor() {
    super();
    this.resetNewGlossaryModel();
  }

  ngOnInit() {}

  show() {
    super.show();
    this.resetNewGlossaryModel();
  }

  submit() {
    this.hide();
    this.newGlossarySubmitted.emit(this.newGlossary);
  }

  resetNewGlossaryModel() {
    this.newGlossary = new GlossariesForEditing();
    this.newGlossary.locales_Ids = [];
    this.newGlossary.localization_Projects_Ids = [];
  }

  is_save() {
    console.log();

    console.log(this.newGlossary.name_text);
    console.log(this.newGlossary.name_text.length);
    console.log(this.newGlossary.locales_Ids.length);
    console.log(this.newGlossary.localization_Projects_Ids.length);

    return false;
  }
}
