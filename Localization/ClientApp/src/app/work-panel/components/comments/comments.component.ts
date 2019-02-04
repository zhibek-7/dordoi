import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';

import { ShowImageModalComponent } from '../show-image-modal/show-image-modal';

import { CommentService } from '../../../services/comment.service';
import { SharePhraseService } from '../../localServices/share-phrase.service';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { ShareWordFromModalService } from '../../localServices/share-word-from-modal.service';

import { TermWithGlossary } from '../../localEntites/terms/termWithGlossary.type';
import { Comment } from '../../../models/database-entities/comment.type';
import { CommentWithUser } from '../../localEntites/comments/commentWithUser.type';

import { Term } from 'src/app/models/Glossaries/term.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { Image } from 'src/app/models/database-entities/image.type';

import * as $ from 'jquery';

@Component({
  selector: 'comments-component',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  //Переменные для блока с комментариями
  commentsList: CommentWithUser[];

  stringId: number;
  commentEdited = false;
  changedComment: CommentWithUser;

  filesToUpload: File[];

  addCommentText: string = "";

  // Переменные для блока с Глоссарием
  termsList: TermWithGlossary[];

  searchTermText: string = '';


  constructor(private commentService: CommentService,
    private sharePhraseService: SharePhraseService,
    private glossariesService: GlossariesService,
    private shareWordFromModalService: ShareWordFromModalService,
    private showImageDialog: MatDialog) {

    this.commentsList = [];
    this.termsList = [];
    this.filesToUpload = [];

    //Загрузка всех терминов при инициализации формы
    this.getTerms();

    // Событие, срабатываемое при выборе фразы для перевода
    this.sharePhraseService.onClick.subscribe(pickedPhrase => {
      this.stringId = pickedPhrase.id;
      this.getComments(this.stringId);

      // переключает TabBar на вкладку "Комментарии" при смене слова для перевода
      let activeTab = $(".directoryBlock .nav-tabs .active").attr('href');

      if (activeTab != "#nav-comment") {
        $("a[href='" + activeTab + "']").removeClass("active show").attr("aria-selected", false);
        $(activeTab).removeClass("active show")
      }
      $("a[href='#nav-comment']").addClass("active show").attr("aria-selected", true);
      $("#nav-comment").addClass("active show");
    });

    // Событие, срабатываемое при поиске слова в глоссарие из модального окна
    this.shareWordFromModalService.onClickFindInGlossary.subscribe(word => {
      this.searchTermText = word;

      // переключает TabBar на вкладку "Глоссарии"
      let activeTab = $(".directoryBlock .nav-tabs .active").attr('href');

      if (activeTab != "#nav-condition") {
        $("a[href='" + activeTab + "']").removeClass("active show").attr("aria-selected", false);
        $(activeTab).removeClass("active show")
      }
      $("a[href='#nav-condition']").addClass("active show").attr("aria-selected", true);
      $("#nav-condition").addClass("active show");
    });
  }

  ngOnInit(): void { }

  // Функция получения всех комментариев для данной фразы
  getComments(idString: number) {
    this.commentService.getAllCommentsInStringById(idString)
      .subscribe(comments => {
        this.commentsList = comments;
      });
  };

  // Добавление комментария
  public async addComment() {
    let comment: Comment = new Comment(301, this.stringId, this.addCommentText);        //TODO поменять на id реального пользователя, когда появится
    let insertedComment: CommentWithUser = await this.commentService.createComment(comment);
    this.commentsList.push(insertedComment);

    this.addCommentText = null;
  }

  // Событие, срабатываемое при нажатии клавиши Enter при добавлении нового комментария
  onEnterPressComment(event: any) {
    if (event.which == 13 || event.keyCode == 13) {
      this.addComment();
    }
  }

  // Изменение комментария
  async changeCommentClick(comment: CommentWithUser) {
    this.commentsList = await [];
    this.changedComment = comment;
    this.commentEdited = true;
    this.filesToUpload = [];
    this.commentsList.push(comment);
  }

  // Отмена изменения комментария
  cancelChangingComment(commentId: number) {
    this.endEditingMode();
  }

  // Сохранение измененного комментария
  async saveChangedComment(comment: CommentWithUser) {
    // comment.id_User = userId           // когда появится id реального пользователя, нужно будет использовать его (а пока костыль)
    let updatedComment: Comment = new Comment(301, this.stringId, this.changedComment.comment, new Date(Date.now()), comment.commentId);

    await this.commentService.updateComment(updatedComment);
    if (this.filesToUpload != null) {
      this.loadScrinshot();
    }
    this.endEditingMode();
  }

  // Функция завершения редактирования комментария
  endEditingMode() {
    this.commentEdited = false;
    this.changedComment.comment = "";
    this.filesToUpload = null;
    this.getComments(this.stringId);
  }

  // Функция загрузки скриншота
  loadScrinshot() {
    this.commentService.uploadImageToComment(this.filesToUpload, this.changedComment.commentId);
  }

  // Функция отображения скриншота в модальном окне в увеличенном размере
  showImage(image: Image) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      selectedImage: image
    };


    let dialogRef = this.showImageDialog.open(ShowImageModalComponent, dialogConfig);
  }

  // Функция, срабатываемая при загрузке скриншота
  handleFileInput(file: FileList) {
    this.filesToUpload.push(file.item(0));
    if (this.changedComment.images == undefined) {
      this.changedComment.images = [];
    }

    var reader = new FileReader();
    reader.onload = (event: any) => {

      var insertedImage = new Image();
      insertedImage.body = event.target.result;

      //обрезаем дополнительную информацию о изображении и оставляем только byte[]
      insertedImage.body = insertedImage.body.match(".*base64,(.*)")[1];

      this.changedComment.images.push(insertedImage)
    }
    reader.readAsDataURL(this.filesToUpload[this.filesToUpload.length - 1]);
  }

  // Удаление комментария
  deleteCommentClick(comment: CommentWithUser) {
    this.commentService.deleteComment(comment.commentId);
    for (var i = 0; i < this.commentsList.length; i++) {
      if (this.commentsList[i].commentId == comment.commentId) {
        this.commentsList.splice(i, 1);
        break;
      }
    }
  }

  // Поиск всех терминов из всех глоссариев присоедененных к проекту локализации
  getTerms() {
    this.glossariesService.getAllTermsFromAllGlossarisInProject(0)      //TODO поменять на id реального проекта, когда появится
      .subscribe(allTermsInProject => {
        this.termsList = allTermsInProject;
        this.termsList.forEach(element => {
          element.term = new Term(element.id, element.substringToTranslate, element.description, element.context, element.iD_FileOwner,
            element.translationMaxLength, element.value, element.positionInText, element.partOfSpeechId, true);
          element.glossary = new Glossary(element.glossaryId, element.glossaryName, element.glossaryDescription);
        });
      });
  }

  // Функция проверки кол-ва вариантов перевода по поиску по памяти
  checkNumberOfTermsInProject() {
    if (this.termsList.length == 0) {
      // if(this.termsList==null){
      return false;
    }
    else {
      return true;
    }
  }

  // Событие, срабатываемое при нажатии клавиши Enter при поиске в глоссарие
  onEnterPressGlossary(event: any) {
    if (event.which == 13 || event.keyCode == 13) {
      this.addComment();
    }
  }

  deleteTermClick(glossaryId: number, termId: number) {
    this.glossariesService.deleteTerm(glossaryId, termId)
      .subscribe();

    for (var i = 0; i < this.termsList.length; i++) {
      if (this.termsList[i].id == termId) {
        this.termsList.splice(i, 1);
        break;
      }
    }
  }

  // changeTermClick(termWithGlossary: TermWithGlossary){
  //     console.log(termWithGlossary);        
  // }

  // updateTermClick(){

  // }

}
