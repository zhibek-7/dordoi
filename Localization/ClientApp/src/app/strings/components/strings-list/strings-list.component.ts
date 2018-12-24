import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
import { LanguageService } from 'src/app/services/languages.service';

@Component({
  selector: 'app-strings-list',
  templateUrl: './strings-list.component.html',
  styleUrls: ['./strings-list.component.css']
})
export class StringsListComponent implements OnInit {

  constructor(
    private translationSubstringService: TranslationSubstringService,
    private languageService: LanguageService,
  ) { }

  ngOnInit() {
  }

}
