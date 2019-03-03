import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";

import { TreeNode } from "primeng/api";
import { File as FileData } from "../models/database-entities/file.type";
import { Locale } from "../models/database-entities/locale.type";
import { FileTranslationInfo } from "../models/database-entities/fileTranslationInfo.type";
import { FolderModel } from "../models/DTO/folder.type";

@Injectable({
  providedIn: "root"
})
export class FileService {
  private _url = "api/files";

  constructor(private http: HttpClient) {}

  getFiles(): Observable<TreeNode[]> {
    return this.http
      .post<TreeNode[]>(this._url, null, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(
        // tap(response => console.log(response)),
        catchError(this.handleError("Get files", []))
      );
  }

  getFilesByProjectIdAsTree(
    projectId: number,
    fileNamesSearch?: string
  ): Observable<TreeNode[]> {
    let body: any = {};
    if (fileNamesSearch && fileNamesSearch != "") {
      body.fileNamesSearch = fileNamesSearch;
    }
    return this.http
      .post<TreeNode[]>(this._url + "/byProjectId/" + projectId, body, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("getFilesByProjectIdAsTree", [])));
  }

  //Нужно для формирования отчетов
  getInitialProjectFolders(projectId: number): Observable<FileData[]> {
    const url = `${this._url}/ForProject:${projectId}`;
    return this.http.post<FileData[]>(url, projectId, {
      headers: new HttpHeaders().set(
        "Authorization",
        "Bearer " + sessionStorage.getItem("userToken")
      )
    });
  }

  addFile(
    file: File,
    projectId: number,
    parentId?: number
  ): Observable<TreeNode> {
    const url = `${this._url}/add/fileByProjectId/${projectId}`;

    const formData = new FormData();

    formData.append("file", file, file.name);
    if (parentId) {
      formData.append("parentId", parentId.toString());
    } else {
      // null is not valid, eliding parameter is not valid
      formData.append("parentId", "");
    }

    return this.http
      .post<TreeNode>(url, formData, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  updateFileVersion(
    file: File,
    fileName: string,
    projectId: number,
    parentId?: number
  ): Observable<TreeNode> {
    const url = `${this._url}/updateFileVersion/byProjectId/${projectId}`;

    const formData = new FormData();
    formData.append("file", file, fileName);
    if (parentId) {
      formData.append("parentId", parentId.toString());
    } else {
      // null is not valid, eliding parameter is not valid
      formData.append("parentId", "");
    }

    return this.http
      .post<TreeNode>(url, formData, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("updateFileVersion")));
  }

  addFolder(
    name: string,
    projectId: number,
    parentId?: number
  ): Observable<TreeNode> {
    const url = `${this._url}/add/folderByProjectId/${projectId}`;
    const folderModel = new FolderModel(name, projectId, parentId);
    return this.http
      .post<TreeNode>(url, folderModel, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("addFolder")));
  }

  uploadFolder(
    files,
    projectId: number,
    signalrClientId: string,
    parentId?: number
  ): Observable<any> {
    const url = `${this._url}/upload/folderByProjectId/${projectId}`;

    const formData = new FormData();
    for (const file of files) {
      formData.append("files", file, file.webkitRelativePath);
    }
    if (parentId) {
      formData.append("parentId", parentId.toString());
    } else {
      // null is not valid, eliding parameter is not valid
      formData.append("parentId", "");
    }
    formData.append("signalrClientId", signalrClientId);

    return this.http
      .post<TreeNode>(url, formData, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("Upload folder")));
  }

  updateNode(data: FileData): Observable<any> {
    const url = `${this._url}/update/${data.id}`;

    return this.http
      .put(url, data, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("Update node")));
  }

  deleteNode(data: FileData): Observable<any> {
    const url = `${this._url}/delete`;

    return this.http
      .post(url, data, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("Delete node")));
  }

  changeParentFolder(
    fileData: FileData,
    newParentId?: number
  ): Observable<Object> {
    const url = `${this._url}/${fileData.id}/changeParentFolder/${newParentId}`;

    return this.http
      .post<TreeNode>(url,  fileData, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("changeParentFolder")));
  }

  getTranslationLocalesForFileAsync(fileData: FileData): Observable<Locale[]> {
    const url = `${this._url}/${fileData.id}/locales/list`;

    return this.http
      .post<Locale[]>(url,fileData, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(
        catchError(this.handleError("getTranslationLocalesForFileAsync", []))
      );
  }

  updateTranslationLocalesForFileAsync(
    fileData: FileData,
    localesIds: number[]
  ): Observable<Locale[]> {
    const url = `${this._url}/${fileData.id}/locales`;

    return this.http
      .put<Locale[]>(url, localesIds, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(
        catchError(this.handleError("updateTranslationLocalesForFileAsync", []))
      );
  }

  downloadFile(fileData: FileData, localeId?: number): Observable<Blob> {
    const url = `${this._url}/${fileData.id}/download`;
    if (!localeId) {
      localeId = -1;
    }

    return this.http.post(url, localeId, {
      headers: new HttpHeaders().set(
        "Authorization",
        "Bearer " + sessionStorage.getItem("userToken")
      ),
      responseType: "blob"
    });
  }

  getFileTranslationInfos(
    fileData: FileData
  ): Observable<FileTranslationInfo[]> {
    const url = `${this._url}/${fileData.id}/GetTranslationInfo`;
    return this.http
      .post<FileTranslationInfo[]>(url, null, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      })
      .pipe(catchError(this.handleError("getFileTranslationInfo", [])));
  }

  handleError<T>(operation = "Operation", result?: T) {
    return (error: any): Observable<T> => {
      console.log(`${operation} failed: ${error.message}`);

      return of(result as T);
    };
  }
}
