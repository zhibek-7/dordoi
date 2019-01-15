import { Component, OnInit } from '@angular/core';

import { CommentService } from '../../../services/comment.service';
import { SharePhraseService } from '../../localServices/share-phrase.service';

import { Comment } from '../../../models/database-entities/comment.type';
import { CommentWithUser } from '../../localEntites/comments/commentWithUser.type';
 
@Component({
    selector: 'comments-component',
    templateUrl: './comments.component.html',
    styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

    commentsList: Array<CommentWithUser>;

    stringId: number;
    commentEdited = false;
    changedComment = "";

    fileToUpload: File = null;
    imageUrl: string = undefined;

    constructor(private commentService: CommentService,  private sharePhraseService: SharePhraseService ) { 

        // Событие, срабатываемое при выборе фразы для перевода
        this.sharePhraseService.onClick.subscribe(pickedPhrase => {
            this.stringId = pickedPhrase.id;
            this.getComments(this.stringId);            
        });
    }

    ngOnInit(): void {}

    // Функция получения всех комментариев для данной фразы
    getComments(idString: number){
        this.commentService.getAllCommentsInStringById(idString)
            .subscribe( comments => {
                this.commentsList = comments;
        });        
    };

    // Добавление комментарие
    public async addComment(textFromInput: string){
        let comment: Comment = new Comment(301, this.stringId, textFromInput);        // поменять на id реального пользователя, когда появится
        let insertedComment: CommentWithUser = await this.commentService.createComment(comment);
        this.commentsList.push(insertedComment);
    }

    // Событие, срабатываемое при нажатии клавиши Enter при добавлении нового комментария
    onEnterPress(event: any){
        if(event.which == 13 || event.keyCode == 13){
            this.addComment(event.target.value);
            event.target.value = null;
        }
    }

    // Изменение комментария
    async changeCommentClick(comment: CommentWithUser){
        this.commentsList = await [];
        this.changedComment = comment.comment;
        this.commentEdited = true
        this.commentsList.push(comment);        
    }

    // Отмена изменения комментария
    cancelChangingComment(commentId: number){
        this.endEditingMode();
    }        

    // Сохранение измененного комментария
    async saveChangedComment(comment: CommentWithUser) {
      // comment.id_User = userId           // когда появится id реального пользователя, нужно будет использовать его (а пока костыль)
      let updatedComment: Comment = new Comment(301, this.stringId, this.changedComment, new Date(Date.now()), comment.commentId);

      await this.commentService.updateComment(updatedComment);
      if (this.fileToUpload != null) {
        this.loadScrinshot();
      }
      this.endEditingMode();
    }

    // Функция завершения редактирования комментария
    endEditingMode() {
      this.commentEdited = false;
      this.changedComment = "";
      this.fileToUpload = null;
      this.imageUrl = undefined;
      this.getComments(this.stringId);
    }

    // Функция загрузки скриншота
    loadScrinshot() {
      this.commentService.uploadImage(this.fileToUpload);
    }

    // Функция, срабатываемая при загрузке скриншота
    handleFileInput(file: FileList) {
      this.fileToUpload = file.item(0);

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageUrl = event.target.result;
      }
      reader.readAsDataURL(this.fileToUpload);
    }

    // Удаление комментария
    deleteCommentClick(commentId: number){
        this.commentService.deleteComment(commentId);
        for(var i = 0; i < this.commentsList.length; i++) {
            if(this.commentsList[i].commentId == commentId) {
                this.commentsList.splice(i, 1);
                break;
            }
        }
    }

}
