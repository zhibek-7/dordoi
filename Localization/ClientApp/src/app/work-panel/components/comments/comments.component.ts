import { Component, OnInit } from "@angular/core";
import { MatDialog, MatDialogConfig } from "@angular/material";

import { ShowImageModalComponent } from "../show-image-modal/show-image-modal";

import { CommentService } from "../../../services/comment.service";
import { SharePhraseService } from "../../localServices/share-phrase.service";
import { GlossariesService } from "src/app/services/glossaries.service";
import { ShareWordFromModalService } from "../../localServices/share-word-from-modal.service";
import { ProjectsService } from "../../../services/projects.service";

import { TermWithGlossary } from "../../localEntites/terms/termWithGlossary.type";
import { Comment } from "../../../models/database-entities/comment.type";
import { CommentWithUser } from "../../localEntites/comments/commentWithUser.type";

import { Term } from "src/app/models/Glossaries/term.type";
import { Glossary } from "src/app/models/database-entities/glossary.type";
import { Image } from "src/app/models/database-entities/image.type";

import * as $ from "jquery";
import { Guid } from "guid-typescript";

@Component({
  selector: "comments-component",
  templateUrl: "./comments.component.html",
  styleUrls: ["./comments.component.css"]
})
export class CommentsComponent implements OnInit {
  //Переменные для блока с комментариями
  commentsList: CommentWithUser[];

  stringId: Guid;
  commentEdited = false;
  changedComment: CommentWithUser;

  filesToUpload: File[];

  addCommentText: string = "";

  // Переменные для блока с Глоссарием
  termsList: TermWithGlossary[];

  searchTermText: string = "";

  constructor(
    private commentService: CommentService,
    private sharePhraseService: SharePhraseService,
    private glossariesService: GlossariesService,
    private shareWordFromModalService: ShareWordFromModalService,
    private showImageDialog: MatDialog,
    private projectsService: ProjectsService
  ) {
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
      let activeTab = $(".directoryBlock .nav-tabs .active").attr("href");

      if (activeTab != "#nav-comment") {
        $("a[href='" + activeTab + "']")
          .removeClass("active show")
          .attr("aria-selected", false);
        $(activeTab).removeClass("active show");
      }
      $("a[href='#nav-comment']")
        .addClass("active show")
        .attr("aria-selected", true);
      $("#nav-comment").addClass("active show");
    });

    // Событие, срабатываемое при поиске слова в глоссарие из модального окна
    this.shareWordFromModalService.onClickFindInGlossary.subscribe(word => {
      this.searchTermText = word;

      // переключает TabBar на вкладку "Глоссарии"
      let activeTab = $(".directoryBlock .nav-tabs .active").attr("href");

      if (activeTab != "#nav-condition") {
        $("a[href='" + activeTab + "']")
          .removeClass("active show")
          .attr("aria-selected", false);
        $(activeTab).removeClass("active show");
      }
      $("a[href='#nav-condition']")
        .addClass("active show")
        .attr("aria-selected", true);
      $("#nav-condition").addClass("active show");
    });
  }

  ngOnInit(): void {}

  // Функция получения всех комментариев для данной фразы
  getComments(idString: Guid) {
    this.commentService
      .getAllCommentsInStringById(idString)
      .subscribe(comments => {
        this.commentsList = comments;
      });
  }

  // Добавление комментария
  public async addComment() {
    let comment_loc: Comment = new Comment(
      this.stringId + "",
      this.addCommentText
    );

    if (comment_loc.Comment_text != "") {
      let insertedComment: CommentWithUser = await this.commentService.createComment(
        comment_loc
      );
      this.commentsList.push(insertedComment);

      this.addCommentText = "";
    }
  }

  // Изменение комментария
  async changeCommentClick(comment: CommentWithUser) {
    // console.log(" comment.id=" + comment.comment_id);
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
    // console.log("comment=" + comment);
    console.log("comment.comment_Id=" + comment.comment_id);
    console.log("comment_text =" + comment.comment_text);
    console.log("changedComment_text=" + this.changedComment.comment_text);
    console.log("changedComment id=" + this.changedComment.comment_id);

    let updatedComment: Comment = new Comment(
      this.stringId + "",
      this.changedComment.comment_text
    );

    updatedComment.DateTime = new Date(Date.now());
    updatedComment.id = this.changedComment.comment_id;

    await this.commentService.updateComment(updatedComment);
    if (this.filesToUpload != null) {
      this.loadScrinshot();
    } else {
      this.endEditingMode();
    }
  }

  // Функция завершения редактирования комментария
  endEditingMode() {
    this.commentEdited = false;
    this.changedComment.comment_text = "";
    this.filesToUpload = null;
    this.getComments(this.stringId);
  }

  // Функция загрузки скриншота
  loadScrinshot() {
    this.commentService
      .uploadImageToComment(this.filesToUpload, this.changedComment.comment_id)
      .subscribe(success => {
        this.endEditingMode();
      });
  }

  // Функция отображения скриншота в модальном окне в увеличенном размере
  showImage(image: Image) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      selectedImage: image
    };

    //TODO не используется let dialogRef =
    this.showImageDialog.open(ShowImageModalComponent, dialogConfig);
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

      this.changedComment.images.push(insertedImage);
    };
    reader.readAsDataURL(this.filesToUpload[this.filesToUpload.length - 1]);
  }

  // Удаление комментария
  deleteCommentClick(comment: CommentWithUser) {
    this.commentService.deleteComment(comment.comment_id);
    for (var i = 0; i < this.commentsList.length; i++) {
      if (this.commentsList[i].comment_id == comment.comment_id) {
        this.commentsList.splice(i, 1);
        break;
      }
    }
  }

  // Поиск всех терминов из всех глоссариев присоедененных к проекту локализации
  getTerms() {
    const id = this.projectsService.currentProjectId;
    this.glossariesService
      .getAllTermsFromAllGlossarisInProject(id) //TODO поменять на id реального проекта, когда появится
      .subscribe(allTermsInProject => {
        this.termsList = allTermsInProject;
        this.termsList.forEach(element => {
          element.term = new Term(
            element.id,
            element.substring_to_translate,
            element.description,
            element.context,
            element.id_file_owner,
            element.translation_max_length,
            element.value,
            element.position_in_text,
            element.context_file,
            element.part_of_speech_id,
            true
          );
          element.glossary = new Glossary(
            element.glossary_id,
            element.glossary_name,
            element.glossary_description
          );
        });
      });
  }

  // Функция проверки кол-ва вариантов перевода по поиску по памяти
  checkNumberOfTermsInProject() {
    //if (this.termsList.length == 0) {
    if (this.termsList == null) {
      return false;
    } else {
      return true;
    }
  }

  // Событие, срабатываемое при нажатии клавиши Enter при поиске в глоссарие
  onEnterPressComment(event: any) {
    if (event.which == 13 || event.keyCode == 13) {
      this.addComment();
    }
  }

  deleteTermClick(glossaryId: Guid, termId: Guid) {
    this.glossariesService.deleteTerm(glossaryId, termId).subscribe();

    for (var i = 0; i < this.termsList.length; i++) {
      if (this.termsList[i].id == termId) {
        this.termsList.splice(i, 1);
        break;
      }
    }
  }
}
