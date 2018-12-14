import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from "rxjs/operators";

import { TreeNode } from "primeng/api";
import { File as FileData } from '../models/database-entities/file.type';

@Injectable({
  providedIn: 'root'
})

export class FileService {
  private _url = 'api/files';

  constructor(private http: HttpClient) { }

  getFiles(): Observable<TreeNode[]> {

    return this.http.get<TreeNode[]>(this._url)
      .pipe(
        // tap(response => console.log(response)),
        catchError(this.handleError('Get files', []))
      );
  }

  //Нужно для формирования отчетов
  getInitialProjectFolders(projectId: number): Observable<FileData[]> {
    const url = `${this._url}/ForProject:${projectId}`;
    return this.http.get<FileData[]>(url);
  }

  addFile(file: File, parentId?: number): Observable<TreeNode> {
    const url = `${this._url}/add/file`;

    const formData = new FormData();

    formData.append('file', file, file.name);
    formData.append('parentId', parentId.toString());

    return this.http.post<TreeNode>(url, formData);
  }

  addFolder(name: string, parentId?: number): Observable<TreeNode> {
    const url = `${this._url}/add/folder`;

    return this.http.post<TreeNode>(url, { name, parentId });
  }

  updateNode(data: FileData): Observable<any> {
    const url = `${this._url}/update/${data.id}`;

    return this.http.put(url, data)
      .pipe(catchError(this.handleError('Update node')));
  }

  deleteNode(data: FileData): Observable<any> {
    const url = `${this._url}/delete/${data.id}`;

    return this.http.delete(url)
      .pipe(catchError(this.handleError('Delete node')));
  }

  handleError<T>(operation = 'Operation', result?: T) {
    return (error: any): Observable<T> => {
      console.log(`${operation} failed: ${error.message}`);

      return of(result as T);
    }
  }
}
