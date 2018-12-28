import { Component, OnInit } from '@angular/core';
//import { ActivatedRoute/*, Params*/ } from "@angular/router";

import { GlossaryService } from 'src/app/services/glossary.service';
import { Glossaries, GlossariesDTO } from 'src/app/models/DTO/glossaries.type';
//import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-list-glossaries',
  templateUrl: './list-glossaries.component.html',
  styleUrls: ['./list-glossaries.component.css'],
  providers: [GlossaryService]
})
export class ListGlossariesComponent implements OnInit {

  glossaries: GlossariesDTO[];

  constructor(
    //private route: ActivatedRoute,
    private glossariesService: GlossaryService)
  { }

  ngOnInit() {
    this.glossariesService.getGlossariesDTO()
      .subscribe(glossaries => this.glossaries = glossaries,
                               error => console.error(error));
  }

}
