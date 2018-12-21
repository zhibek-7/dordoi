import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
import { LanguageService } from 'src/app/services/languages.service';

@Component({
  selector: 'app-strings-main',
  templateUrl: './strings-main.component.html',
  styleUrls: ['./strings-main.component.css']
})
export class StringsMainComponent implements OnInit {

  constructor(
    private translationSubstringService: TranslationSubstringService,
    private languageService: LanguageService,
  ) { }

  ngOnInit() {
  }

}
