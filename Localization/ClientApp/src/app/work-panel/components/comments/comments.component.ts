import { Component, OnInit } from '@angular/core';
 
@Component({
    selector: 'comments-component',
    templateUrl: './comments.component.html',
    styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

    commentsList: Array<Comment>;

    constructor() { }

    ngOnInit(): void {
        this.getComments();
     }

    async getComments(){
        // this.commentsList = await this.jsonParserService.getCommentsFromJsonFile('../../../assets/testData/comments.json'); 
    }

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

}
