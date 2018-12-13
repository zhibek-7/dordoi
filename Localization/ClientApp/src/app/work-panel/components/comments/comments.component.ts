import { Component, OnInit } from '@angular/core';

import { CommentService } from '../../../services/comment.service';
import { SharePhraseService } from '../../localServices/share-phrase.service';

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

    constructor(private commentService: CommentService,  private sharePhraseService: SharePhraseService ) { 

        this.sharePhraseService.onClick.subscribe(pickedPhrase => {
            this.stringId = pickedPhrase.id;
            this.getComments(this.stringId);            
        });
    }

    ngOnInit(): void {}

    getComments(idString: number){
        this.commentService.getAllCommentsInStringById(idString)
            .subscribe( comments => {
                this.commentsList = comments;
        });        
    };

    public addComment(textFromInput: string){
        // let lastElement = this.commentsList[this.commentsList.length - 1];
        // let idOfTheNewElement = Number(lastElement.id) + 1;
        // this.commentsList.push(new Comment(idOfTheNewElement, textFromInput));
    }

    onEnterPress(event: any){
        if(event.which == 13 || event.keyCode == 13){
            this.addComment(event.target.value);
            event.target.value = null;
        }
    }

    async changeCommentClick(comment: CommentWithUser){
        this.commentsList = await [];
        this.changedComment = comment.comment;
        this.commentEdited = true
        this.commentsList.push(comment);        
    }

    cancelChangingComment(commentId: number){
        this.commentEdited = false;
        this.changedComment = "";
        this.getComments(this.stringId);
    }
    
    loadScrinshot(){

    }

    saveChangedComment(){

    }

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
