import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

import { Translator } from 'src/app/models/Translators/translator.type';

import { TranslatorsService } from 'src/app/services/translators.service';
import { UserService } from 'src/app/services/user.service';
import { LanguageService } from 'src/app/services/languages.service';

import { ItemsSortBy } from './itemsSortBy.pipe';
import { checkAndUpdateBinding } from '@angular/core/src/view/util';
import {PageEvent} from '@angular/material';

@Component({
  selector: 'app-translators-list',
  templateUrl: './translators-list.component.html',
  styleUrls: ['./translators-list.component.css']
})
export class TranslatorsListComponent implements OnInit {
  translators: Translator[] = [];
  filtredTranslators;
  languages;
  selectedCurrentLanguage;
  selectedTranslateLanguage;
  nativeLanguage;
  selectedService;
  listLength: number = 0;
  pageSize: number = 10;
  pageSizeOptions: number[] = [10, 25, 100];
  pageEvent: PageEvent;
  min;
  max;
  sortingColumn: string;
  isDesc = false;
  selectedSorting;
  topics = new FormControl();
  services = ['Перевод', 'Редактура'];
  topicsList = ['PR', 'Машиностроение', 'ИТ', 'Финансы', 'Маркетинг'];
  sortingList = [
    {
      value: [false, 'wordsQuantity'],
      name: 'По числу переведенных слов (убыв.)'
    },
    {
      value: [true, 'wordsQuantity'],
      name: 'По числу переведенных слов (возр.)'
    },
    { value: [true, 'cost'], name: 'По тарифу, сначала дешевле' },
    { value: [false, 'cost'], name: 'По тарифу, сначала дороже' }
  ];

  constructor(
    private translationService: TranslatorsService,
    private usersService: UserService,
    private languagesService: LanguageService
  ) {}

  ngOnInit() {
    this.loadPublicTranslators();
    this.loadLanguages();
    this.filtredTranslators = this.translators;
  }

  sortBy(parametrs) {
    this.sortingColumn = parametrs[1];
    this.isDesc = parametrs[0];
  }

  checkValue(translator, filter, name) {
    if (!filter) {
      return true;
    } else {
      return translator[name] === filter;
    }
  }

  checkMulti(translator, filter, name) {
    if (!filter[name] || name.length === 0) {
      return true;
    } else {
      return translator.topics.some(r => filter[name].indexOf(r) >= 0);
    }
  }

  checkLang(translator, filter, name) {
    if (!filter) {
      return true;
    } else {
      return translator.languages.includes(filter);
    }
  }

  checkMin(translator, filter, name) {
    if (!filter.min) {
      return true;
    } else {
      return translator[name] >= filter.min;
    }
  }

  checkMax(translator, filter, name) {
    if (!filter.max) {
      return true;
    } else {
      return translator[name] <= filter.max;
    }
  }

  check(translator, filter) {
    return (this.checkMulti(translator, filter, 'topics') &&
            this.checkMin(translator, filter, 'cost') &&
            this.checkMax(translator, filter, 'cost')) &&
            this.checkLang(translator, filter.currentLanguage, 'languages') &&
            this.checkLang(translator, filter.translateLanguage, 'languages') &&
            this.checkValue(translator, filter.service, 'service') ;
  }

  filtredFn(searchingVal) {
    console.log(searchingVal);
    this.filtredTranslators = [];

    this.translators.forEach(item => {
      console.log('item')
      if(this.check(item, searchingVal)) {
        this.filtredTranslators.push(item);
      }
    });
  }

  clearSearch() {
    this.loadPublicTranslators();
    this.filtredTranslators = this.translators;
    this.selectedCurrentLanguage = '';
    this.selectedTranslateLanguage = '';
    this.nativeLanguage = '';
    this.selectedService = '';
    this.topics.setValue('');
    this.min = '';
    this.max = '';
  }

  loadLanguages() {
    this.languagesService.getLanguageList().subscribe({
      next: response => {
        // console.log(response)
        this.languages = response;
      },
      error: err => console.log(err)
    });
  }

  onPageChanged(args: PageEvent) {
    this.pageSize = args.pageSize;
    const currentOffset = args.pageSize * args.pageIndex;
    this.loadPublicTranslators(currentOffset);
    this.filtredTranslators = this.translators;
  }

  loadPublicTranslators(pageIndex = 0) {
    this.translationService.getAllPublicTranslators().subscribe({
      next: response => {
        // console.log('next', response);
        this.translators = response.slice(pageIndex, this.pageSize + pageIndex);
        this.listLength = response.length;
      },
      error: err => console.log(err),
      complete: () => {
        this.translators.forEach(item => {
          item.user_pic = this.loadPhoto(item.user_Id);
        });
      }
    });
  }

  loadPhoto(id) {
    this.usersService.getPhotoById(id).subscribe(
      imageBlob => {
        const reader = new FileReader();
        reader.addEventListener(
          'load',
          () => {
            return reader.result;
          },
          false
        );

        if (imageBlob) {
          reader.readAsDataURL(imageBlob);
        }
      },
      error => {
        return '';
      }
    );
  }

  onMinPriceChange(minPrice: number){
    if(this.max != undefined && (this.min > this.max) || (this.min < 0)) {      
      this.min = undefined;
    }
  }

  onMaxPriceChange(minPrice: number){
    if((this.min > this.max) || (this.max < 0)) {      
      this.max = undefined;
    }
  }

}
