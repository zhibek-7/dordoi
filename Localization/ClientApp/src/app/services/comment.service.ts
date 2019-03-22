import { StringEdit } from "./../models/database-entities/stringEdit.type";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Comment } from "../models/database-entities/comment.type";
import { CommentWithUser } from "../work-panel/localEntites/comments/commentWithUser.type";

import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Guid } from "guid-typescript";

@Injectable()
export class CommentService {
  private url: string =
    document.getElementsByTagName("base")[0].href + "api/comment/";

  constructor(private http: HttpClient) {}

  async createComment(comment: Comment) {
    //TODO  рабочий вариант передачи данны
    // let comment2 = new Comment2(
    //   comment.ID_Translation_Substrings + "",
    //   comment.Comment_text
    // );
    let asyncResult = await this.http
      .post<CommentWithUser>(this.url + "AddComment", comment)
      .toPromise();
    return asyncResult;
  }

  uploadImageToComment(fileToUpload: File[], commentId: Guid) {
    const formData: FormData = new FormData();

    // fileToUpload.forEach(element => {
    formData.set("Image", fileToUpload[0]);
    formData.append("CommentId", commentId.toString());
    return this.http.post(this.url + "UploadImageToComment", formData);
    // });
  }

  getAllCommentsInStringById(idString: Guid): Observable<CommentWithUser[]> {
    return this.http
      .post<CommentWithUser[]>(this.url + "InString/" + idString, null)
      .pipe(catchError(this.handleError("Get comments in string", [])));
  }

  deleteComment(commentId: Guid) {
    return this.http
      .delete<boolean>(this.url + "DeleteComment/" + commentId)
      .toPromise();
  }

  updateComment(comment: Comment) {
    return this.http
      .put<boolean>(this.url + "UpdateComment/" + comment.id, comment).toPromise();
  }

  handleError<T>(operation = "Operation", result?: T) {
    return (error: any): Observable<T> => {
      console.log(`${operation} failed: ${error.message}`);

      return of(result as T);
    };
  }
}

//TODO  рабочий вариант передачи данны
//export class Comment2 {
//  public id: string;
//  public ID_User: string;
//  public ID_Translation_Substrings: string;
//  public Comment_text: string;
//  public DateTime: Date;

//  public constructor(ID_Translation_Substrings: string, Comment_text: string) {
//    this.ID_User = null; //Guid.createEmpty()
//    this.ID_Translation_Substrings = ID_Translation_Substrings + "";
//    this.Comment_text = Comment_text;
//    this.DateTime = null;
//  }
//}
