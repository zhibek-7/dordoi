import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';

import { Comment } from '../models/database-entities/comment.type';
import { CommentWithUser } from '../work-panel/localEntites/comments/commentWithUser.type';

import { Observable, of  } from 'rxjs';
import { catchError } from "rxjs/operators";

@Injectable()
export class CommentService {

    private url: string = document.getElementsByTagName('base')[0].href + "api/comment/";

    constructor(private http: HttpClient) {

    }

    async createComment(comment: Comment){
        let asyncResult = await this.http.post<CommentWithUser>(this.url + "AddComment", comment).toPromise();
        return asyncResult;
    }

    uploadImageToComment(fileToUpload: File[], commentId: number) {
        const formData: FormData = new FormData();

        fileToUpload.forEach(element => {
            formData.set('Image', element); 
            formData.append('CommentId', commentId.toString());
            return this.http.post(this.url + "UploadImageToComment", formData).toPromise();
        });        
    }

    getAllCommentsInStringById(idString: number): Observable<CommentWithUser[]>{    
        return this.http.get<CommentWithUser[]>(this.url + 'InString/' + idString)
                .pipe(
                    catchError(this.handleError('Get comments in string', []))
                );
    }    

    deleteComment(commentId: number){
        return this.http.delete<boolean>(this.url + "DeleteComment/" + commentId).toPromise();
    }

    updateComment(comment: Comment){
        return this.http.put<boolean>(this.url + "UpdateComment/" + comment.id, comment).toPromise();
    }

    handleError<T>(operation = 'Operation', result?: T) {
        return (error: any): Observable<T> => {
          console.log(`${operation} failed: ${error.message}`);
    
          return of(result as T);
        }
    }

}
