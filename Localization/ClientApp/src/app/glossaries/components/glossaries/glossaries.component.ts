import { Component, OnInit, EventEmitter } from '@angular/core';

import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-glossaries',
  templateUrl: './glossaries.component.html',
  styleUrls: ['./glossaries.component.css']
})
export class GlossariesComponent implements OnInit {

  glossaries: Glossary[];

  constructor(private glossariesService: GlossariesService) { }

  ngOnInit() {
    this.glossariesService.getGlossaries()
      .subscribe(
        glossaries => this.glossaries = glossaries,
        error => console.error(error));
  }

}
