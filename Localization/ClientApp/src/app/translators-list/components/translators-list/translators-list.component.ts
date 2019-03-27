import { Component, OnInit, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';

import { checkAndUpdateBinding } from '@angular/core/src/view/util';

import {PageEvent} from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { TranslationTopicService } from '../../../services/translation-topic.service';
import { TypeOfServiceService } from '../../../services/type-of-service.service';
//import { TranslatorsService } from 'src/app/services/translators.service';
import { UserService } from 'src/app/services/user.service';
import { LanguageService } from 'src/app/services/languages.service';

import { ItemsSortBy } from '../../itemsSortBy.pipe';

import { Translator } from 'src/app/models/Translators/translator.type';
import { TranslationTopicForSelectDTO } from '../../../models/DTO/translationTopicForSelectDTO.type';
import { TypeOfServiceForSelectDTO } from '../../../models/DTO/typeOfServiceForSelectDTO.type';
import { Locale } from "src/app/models/database-entities/locale.type";
import { DialogInviteTranslatorComponent } from '../dialog-invite-translator/dialog-invite-translator.component';
import { Guid } from "guid-typescript";
import { ProjectsService } from 'src/app/services/projects.service';

//export interface DialogData {
//  animal: string;
//  name: string;
//}

@Component({
  selector: 'app-translators-list',
  templateUrl: './translators-list.component.html',
  styleUrls: ['./translators-list.component.css']
})
export class TranslatorsListComponent implements OnInit {

  translators: Translator[] = [];

  languages: Locale[] = [];
  services: TypeOfServiceForSelectDTO[] = []; // = ['Перевод', 'Редактура'];
  topicsList: TranslationTopicForSelectDTO[] = []; // = ['PR', 'Машиностроение', 'ИТ', 'Финансы', 'Маркетинг'];


  //#region sorts

  sortingList = [
    {
      value: [false, 'words_quantity'],
      name: 'По числу переведенных слов (убыв.)'
    },
    {
      value: [true, 'words_quantity'],
      name: 'По числу переведенных слов (возр.)'
    },
    {
      value: [true, 'cost'],
      name: 'По тарифу, сначала дешевле'
    },
    {
      value: [false, 'cost'],
      name: 'По тарифу, сначала дороже'
    }
  ];
  selectedSorting;//: [boolean, string];

  sortingColumn: string = null;
  isDesc: boolean = null;

  //#endregion

  //#region filters

  selectedCurrentLanguage: Guid;
  selectedTranslateLanguage: Guid;
  nativeLanguage: boolean;
  selectedService: Guid;
  min: number;
  max: number;

  //
  topics = new FormControl();
  //

  //#endregion


  //#region pagination

  listLength: number = 0;
  pageSize: number = 10;
  pageSizeOptions: number[] = [10, 25, 100];
  pageEvent: PageEvent;

  //#endregion


  constructor(
    //private translationService: TranslatorsService,
    private usersService: UserService,
    private languagesService: LanguageService,
    private translationTopicService: TranslationTopicService,
    private typeOfServiceService: TypeOfServiceService,
    private projectsService: ProjectsService,
    public dialog: MatDialog
  ) {}

  //#region Init
  ngOnInit() {
    this.loadPublicTranslators();
    this.loadLanguages();
    this.loadTranslationTopics();
    this.loadTypeOfServices();
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

  loadTranslationTopics() {
    this.translationTopicService.getAllForSelect().subscribe(
      translationTopics => this.topicsList = translationTopics,
      error => console.log(error)
    );
  }

  loadTypeOfServices() {
    this.typeOfServiceService.getAllForSelect().subscribe(
      typeOfServices => this.services = typeOfServices,
      error => console.log(error)
    );
  }

  //#endregion

  //#region sorting

  sortBy(parametrs) {
    this.sortingColumn = parametrs[1];
    this.isDesc = parametrs[0];
    this.loadPublicTranslators();
  }

  //#endregion

  //#region filtration

  filtredFn(searchingVal) {
    console.log(searchingVal);
    //let filtredTranslators = [];

    //this.translators.forEach(item => {
    //  console.log('item')
    //  if(this.check(item, searchingVal)) {
    //    filtredTranslators.push(item);
    //  }
    //});
    //this.translators = filtredTranslators;

    this.loadPublicTranslators();
  }

  clearSearch() {
    this.selectedCurrentLanguage = null;
    this.selectedTranslateLanguage = null;
    this.nativeLanguage = null;
    this.selectedService = null;
    this.topics.setValue(null);
    this.min = null;
    this.max = null;

    this.loadPublicTranslators();
  }

  //#endregion

  openDialog(translator): void {
    const dialogRef = this.dialog.open(DialogInviteTranslatorComponent, {
      width: '650px',
      data: { user: translator }//, name: translator.user_Name}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed', result);
    });
  }

  //checkValue(translator, filter, name) {
  //  if (!filter) {
  //    return true;
  //  } else {
  //    return translator[name] === filter;
  //  }
  //}

  //checkMulti(translator, filter, name) {
  //  if (!filter[name] || name.length === 0) {
  //    return true;
  //  } else {
  //    return translator.topics.some(r => filter[name].indexOf(r) >= 0);
  //  }
  //}

  //checkLang(translator, filter, name) {
  //  if (!filter) {
  //    return true;
  //  } else {
  //    return translator.languages.includes(filter);
  //  }
  //}

  //checkMin(translator, filter, name) {
  //  if (!filter.min) {
  //    return true;
  //  } else {
  //    return translator[name] >= filter.min;
  //  }
  //}

  //checkMax(translator, filter, name) {
  //  if (!filter.max) {
  //    return true;
  //  } else {
  //    return translator[name] <= filter.max;
  //  }
  //}

  //check(translator, filter) {
  //  return (this.checkMulti(translator, filter, 'topics') &&
  //          this.checkMin(translator, filter, 'cost') &&
  //          this.checkMax(translator, filter, 'cost')) &&
  //          this.checkLang(translator, filter.currentLanguage, 'languages') &&
  //          this.checkLang(translator, filter.translateLanguage, 'languages') &&
  //          this.checkValue(translator, filter.service, 'service') ;
  //}

  //loadPublicTranslators(pageIndex = 0) {
  //  this.translationService.getAllPublicTranslators().subscribe({
  //    next: response => {
  //      // console.log('next', response);
  //      this.translators = response.slice(pageIndex, this.pageSize + pageIndex);
  //      this.listLength = response.length;
  //    },
  //    error: err => console.log(err),
  //    complete: () => {
  //      this.translators.forEach(item => {
  //        item.user_pic = this.loadPhoto(item.user_Id);
  //      });
  //    }
  //  });
  //}

  //loadPhoto(id) {
  //  this.usersService.getPhotoById(id).subscribe(
  //    imageBlob => {
  //      const reader = new FileReader();
  //      reader.addEventListener(
  //        'load',
  //        () => {
  //          return reader.result;
  //        },
  //        false
  //      );

  //      if (imageBlob) {
  //        reader.readAsDataURL(imageBlob);
  //      }
  //    },
  //    error => {
  //      return '';
  //    }
  //  );
  //}

  onMinPriceChange(minPrice: number) {
    if (this.max != undefined && (this.min > this.max) || (this.min < 0)) {
      this.min = undefined;
    }
  }

  onMaxPriceChange(minPrice: number) {
    if ((this.min > this.max) || (this.max < 0)) {
      this.max = undefined;
    }
  }
  
  onPageChanged(args: PageEvent) {
    this.pageSize = args.pageSize;
    const currentOffset = args.pageSize * args.pageIndex;
    this.loadPublicTranslators(currentOffset);
  }

  loadPublicTranslators(pageIndex = 0) {

    let sortingColumns: string[] = null;
    if (this.sortingColumn) {
      sortingColumns = [this.sortingColumn];
    }

    let selectedTopics: Guid[] = null;
    if (this.topics.value != null && this.topics.value.length > 0) {
      selectedTopics = this.topics.value.map(t => t.id);
    }

    this.usersService.getAllTranslators(
      this.selectedCurrentLanguage,
      this.selectedTranslateLanguage,
      this.nativeLanguage,
      this.selectedService,
      selectedTopics,
      this.min,
      this.max,
      this.pageSize,
      pageIndex,
      sortingColumns,//[this.selectedSorting[1]],
      this.isDesc//this.selectedSorting[0]
    ).subscribe(
      response => {
        this.translators = response.body;

        //console.log("this.translators: ", this.translators);
        this.listLength = +response.headers.get("totalCount");
        //this.parent.currentOffset = offset;
      },
      error => {
        //this.isDataSourceLoaded = false;
        console.log(error);
      }
    );
  }



}

