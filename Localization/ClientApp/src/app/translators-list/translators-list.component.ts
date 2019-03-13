import { Component, OnInit } from '@angular/core';
import {FormControl} from '@angular/forms';
import { Translator } from 'src/app/models/Translators/translator.type';
import { TranslatorsService } from 'src/app/services/translators.service';
import { UserService } from 'src/app/services/user.service';
import { LanguageService } from 'src/app/services/languages.service'

@Component({
  selector: 'app-translators-list',
  templateUrl: './translators-list.component.html',
  styleUrls: ['./translators-list.component.css']
})
export class TranslatorsListComponent implements OnInit {
  translators: Translator[] = [];
  languages;
  selectedCurrentLanguage;
  selectedTranslateLanguage;
  nativeLanguage;
  selectedService;
  sortingCol;
  isDesc: boolean = false;
  topics = new FormControl();
  services = ['Перевод', 'Редактура'];
  topicsList = ['PR', 'Машиностроение', 'ИТ', 'Финансы', 'Маркетинг'];

  constructor(
    private translationService: TranslatorsService,
    private usersService: UserService,
    private languagesService: LanguageService
  ) {}

  ngOnInit() {
    this.loadPublicTranslators();
    this.loadLanguages();
  }

  sortBy(columnName) {

  }

  loadLanguages() {
    this.languagesService.getLanguageList()
    .subscribe({
      next: response => {
        // console.log(response)
        this.languages = response;
      },
      error: err => console.log(err),
    });
  }

  loadPublicTranslators() {
    this.translationService.getAllPublicTranslators().subscribe({
      next: response => {
        // console.log('next', response);
        this.translators = response;
      },
      error: err => console.log(err),
      complete: () => {
        this.translators.forEach((item) => {
          item.user_pic = this.loadPhoto(item.user_Id);
        });
      }
    });
  }

  loadPhoto(id) {
    this.usersService.getPhotoById(id)
      .subscribe(
        imageBlob => {
          let reader = new FileReader();
          reader.addEventListener('load', () => {
           return reader.result;
          }, false);

          if (imageBlob) {
            reader.readAsDataURL(imageBlob);
          }

        },
        error => {
          return '';
        });
  }
}
