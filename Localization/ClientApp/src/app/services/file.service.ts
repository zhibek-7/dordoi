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
  private apiUrl = 'https://localhost:5001/api';
  private filesEndpoint = 'files';

  constructor(private http: HttpClient) { }

  getFiles(): Observable<TreeNode[]> {
    const url = `${this.apiUrl}/${this.filesEndpoint}`;

    return this.http.get<TreeNode[]>(url)
      .pipe(
        // tap(response => console.log(response)),
        catchError(this.handleError('Get files', []))
      );
  }

  addFile(file: File, parentId?: number): Observable<TreeNode> {
    const url = `${this.apiUrl}/${this.filesEndpoint}/add/file`;

    const formData = new FormData();

    formData.append('file', file, file.name);
    formData.append('parentId', parentId.toString());

    return this.http.post<TreeNode>(url, formData);
  }

  addFolder(name: string, parentId?: number): Observable<TreeNode> {
    const url = `${this.apiUrl}/${this.filesEndpoint}/add/folder`;

    return this.http.post<TreeNode>(url, { name, parentId });
  }

  updateNode(data: FileData): Observable<any> {
    const url = `${this.apiUrl}/${this.filesEndpoint}/update/${data.id}`;

    return this.http.put(url, data)
      .pipe(catchError(this.handleError('Update node')));
  }

  deleteNode(data: FileData): Observable<any> {
    const url = `${this.apiUrl}/${this.filesEndpoint}/delete/${data.id}`;

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
