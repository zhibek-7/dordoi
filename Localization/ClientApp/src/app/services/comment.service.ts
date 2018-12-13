import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';

import { Comment } from '../models/database-entities/comment.type';
import { CommentWithUser } from '../work-panel/localEntites/comments/commentWithUser.type';

import { Observable, of  } from 'rxjs';
import { catchError } from "rxjs/operators";

@Injectable()
export class CommentService {

    private url: string = document.getElementsByTagName('base')[0].href + "api/comment/";

    constructor(private http: HttpClient) {

    }

    // async createTranslate(translate: Translation){
    //     let asyncResult = await this.http.post(this.url, translate).toPromise();
    //     return asyncResult;
    // }

    getAllCommentsInStringById(idString: number): Observable<CommentWithUser[]>{        
        return this.http.get<CommentWithUser[]>(this.url + '/InString/' + idString)
                .pipe(
                    catchError(this.handleError('Get comments in string', []))
                );
    }

    // async rejectTranslate(idTranslation: number){
    //     await this.http.delete(this.url + '/RejectTranslation/' + idTranslation).toPromise();
    // }

    deleteComment(commentId: number){
        return this.http.delete(this.url + "DeleteComment/" + commentId).subscribe(
            // error => console.log(error)
        );
    }

    updateTextOfComment(commentId: number, commentNewText: string){
        return this.http.put(this.url + "UpdateComment/" + commentId, commentNewText).subscribe();
    }

    handleError<T>(operation = 'Operation', result?: T) {
        return (error: any): Observable<T> => {
          console.log(`${operation} failed: ${error.message}`);
    
          return of(result as T);
        }
    }

}