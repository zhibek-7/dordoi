import { Component, OnInit } from '@angular/core';

import { GlossariesService } from '../services/glossaries.service';
import { Glossary } from '../models/database-entities/glossary.type';

@Component({
  selector: 'app-glossaries',
  templateUrl: './glossaries.component.html',
  styleUrls: ['./glossaries.component.css']
})
export class GlossariesComponent implements OnInit {

  public glossaries: Glossary[];

  selectedGlossary: Glossary

  public selectGlossary(glossary: Glossary): void {
    this.selectedGlossary = glossary;
  }

  constructor(private glossariesService: GlossariesService) { }

  ngOnInit() {
    this.glossariesService.getGlossaries()
      .subscribe(
        glossaries => this.glossaries = glossaries,
        error => console.error(error));
  }

}
