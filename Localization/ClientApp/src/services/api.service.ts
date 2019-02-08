/*
  Сервис взаимодействия с АПИ FileCloud server
 */

import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Resource} from '../models/Resource';
import {SearchData} from '../models/SearchData';
import {saveAs} from 'file-saver';
import {AppConfigService} from './app-config.service';
import {HistoryElem} from '../models/HistoryElem';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private sectionFiles = 'files/';
  private sectionPublic = 'public/';
  private sectionUsers = 'users/';

  private methodCreateFolder = 'createfolder';
  private methodUploadFiles = 'uploadfiles';
  private methodDownloadFiles = 'downloadfiles';
  private methodDropFiles = 'dropfiles';
  private methodRename = 'rename';
  private methodMove = 'movefiles';
  private methodCopy = 'copyfiles';

  private methodShareLink = 'share';
  private methodShareLinkOff = 'shareoff';
  private methodGetFile = 'sharefile/';
  private methodDownloadLink = 'downloadlink/';

  private methodSearch = 'find/';
  private methodAdvSearch = 'advfind';

  private methodRecovery = 'recovery';
  private methodLabel = 'labelfiles';
  private methodGetFavoriteList = 'favorites';
  private methodAuth = 'login';
  private methodSignUp = 'register';
  private methodGetSharedWithMe = 'shared';
  private methodSharing = 'sharing';
  private methodGetRootFolder = 'getrootfolder';
  private methodGetFolder = 'folder';
  private methodGetTrash = 'trash';
  private methodGetRecent = 'recent';

  private methodGetUserQuota = 'userquota';
  private methodGetUserOccupiedSize = 'occupied';

  private methodGetHierarchy = 'hierarchy';

  private methodGetFileHistory = 'history/';

  private appConfig: any;

  constructor(private httpClient: HttpClient,
              private environment: AppConfigService) {
    this.appConfig = this.environment.config;
  }

  getCASUser() {
    window.location.href = this.appConfig.casServer;
  }

  // Возвращает корневую директорию пользователя
  getRootFolder(token: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionUsers + this.methodGetRootFolder;
    return this.httpClient.get<Resource>(getUrl, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Возвращает стек каталогов для указанного файла
   * @param fileId ID файла
   * @param token токен авторизированного пользователя
   */
  getHierarchy(fileId, token: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetHierarchy;
    return this.httpClient.get<Resource[]>(getUrl, {
      params: {
        fileId: fileId
      },
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Возвращает содержимое корзины
   * @param token токен авторизированного пользователя
   * @param sort сортировка
   */
  getTrash(token: string, sort: string) {
    const gerUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetTrash;
    return this.httpClient.get<Resource[]>(gerUrl, {
      params: {
        sort: sort
      },
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // TODO удалить
  /**
   * Получает список файлов из каталога
   * @param fileId ID каталога
   * @param token токен авторизованного пользователя
   * @param sort сортировка
   * @deprecated используй getFolder()
   */
  getResource(fileId, token: string, sort: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetFolder;
    return this.httpClient.get<Resource[]>(getUrl, {
      params: {
        folderId: fileId,
        sort: sort
      },
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Возвращает содержимое каталога
   * @param fileId ID каталога
   * @param token токен авторизированного пользователя
   * @param sort сортировка
   */
  getFolder(fileId, token: string, sort: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetFolder;
    return this.httpClient.get<Resource[]>(getUrl, {
      params: {
        folderId: fileId,
        sort: sort
      },
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Возвращает помеченные файлы
   * @param token токен авторизированного пользователя
   * @param sort сортировка
   */
  getFavoriteList(token: string, sort: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetFavoriteList;
    return this.httpClient.get<Resource[]>(getUrl, {
      params: {
        sort: sort
      },
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Возвращает доступные общие файлы
   * @param token токен авторизованного пользователя
   * @param sort сортировка
   */
  getSharedWithMe(token: string, sort: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetSharedWithMe;
    return this.httpClient.get<Resource[]>(getUrl, {
      params: {
        sort: sort
      },
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Возвращает список недавних файлов
   * @param token токен авторизованного пользователя
   */
  getRecentFiles(token: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetRecent;
    return this.httpClient.get<Resource[]>(getUrl, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Поиск файлов/папок
   *
   * @param name фрагмент имени или содержимого
   * @param token токен авторизованного пользователя
   */
  searchResource(name: string, token: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodSearch + name;
    return this.httpClient.get<Resource[]>(getUrl,
      {
        headers: {
          'Authorization': 'Bearer ' + token
        }
      }
    );
  }

  /**
   * Получить историю по файлу
   *
   * @param fileId ID файла
   * @param token токен авторизованного пользователя
   */
  getFileHistory(fileId: number, token: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetFileHistory + fileId;
    return this.httpClient.get<HistoryElem[]>(getUrl,
      {
        headers: {
          'Authorization': 'Bearer ' + token
        }
      }
    );
  }

  // Расширенный поиск
  advancedSearch(searchData: SearchData, token: string, sort: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodAdvSearch;
    let params = new HttpParams().set('sort', sort);
    if (searchData.types.length !== 0) {
      params = params.set('types', searchData.types.toString());
    }
    if (searchData.inTrash) {
      params = params.set('inTrash', searchData.inTrash);
    }
    if (searchData.inLabel) {
      params = params.set('inLabel', searchData.inLabel);
    }
    if (searchData.startDate != null) {
      params = params.set('startDate', searchData.startDate.toISOString());
    }
    if (searchData.endDate != null) {
      params = params.set('endDate', searchData.endDate.toISOString());
    }
    if (searchData.name.length !== 0) {
      params = params.set('name', searchData.name);
    }

    const options = {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      }),
      params: params
    };
    return this.httpClient.get<Resource[]>(getUrl, options);
  }

  // Создает новую папку
  createDir(name: string, parentId, token: string) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodCreateFolder;
    return this.httpClient.post(putUrl, null, {
      headers: {
        'Authorization': 'Bearer ' + token
      },
      params: {
        name: name,
        parentId: parentId
      }
    });
  }

  // Загрузка файла
  uploadFiles(files, parentId, token) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodUploadFiles;
    const formData = new FormData();
    console.log(files);
    for (const file of files) {
      formData.append('files', file, file.name);
    }
    formData.append('parentId', parentId);
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Загрузка папки
  uploadFolder(files, parentId, token) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodUploadFiles;
    const formData = new FormData();
    console.log(files);
    for (const file of files) {
      formData.append('files', file, file.webkitRelativePath);
    }
    formData.append('parentId', parentId);
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Совместный доступ к файлам
  shareWithMe(fileIds, emails, token) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodSharing;
    const formData = new FormData();
    formData.append('fileIds', fileIds);
    for (const email of emails) {
      formData.append('emails', email);
    }
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  downloadFiles(files, token: string) {
    const postUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodDownloadFiles;
    const formData = new FormData();

    for (const fileId of files) {
      formData.append('fileIds', fileId);
    }
    return this.httpClient.post(postUrl, formData, {
      responseType: 'blob',
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Переименование файла\папки
  renameItem(fileId, token: string, newName: string) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodRename;

    return this.httpClient.post(putUrl, null, {
      headers: {
        'Authorization': 'Bearer ' + token
      },
      params: {
        fileId: fileId,
        newName: newName
      }
    });
  }

  // Удаление папок\файлов
  dropFiles(fileIds, token, isTrash: boolean) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodDropFiles;
    const formData = new FormData();
    formData.append('isTrash', isTrash ? 'true' : 'false');
    formData.append('fileIds', fileIds);
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Восстановление папок\файлов
  recoveryFiles(fileIds, token) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodRecovery;
    const formData = new FormData();
    formData.append('fileIds', fileIds);
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Пометить папки\файлы
  labelFiles(fileIds, token) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodLabel;
    const formData = new FormData();
    formData.append('fileIds', fileIds);
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Перемещение файлов\папок
  moveFiles(folderId, fileIds, token) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodMove;
    const formData = new FormData();
    formData.append('folderId', folderId);
    formData.append('fileIds', fileIds);
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Копирование папок\файлов
  copyFiles(folderId, fileIds, token) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodCopy;
    const formData = new FormData();
    formData.append('folderId', folderId);
    formData.append('fileIds', fileIds);
    return this.httpClient.post(putUrl, formData, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  // Получить ссылку на файл
  getShareLink(fileId, token: string) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodShareLink;

    return this.httpClient.get<any>(putUrl, {
      headers: {
        'Authorization': 'Bearer ' + token
      },
      params: {
        fileId: fileId
      }
    });
  }

  // Отключить ссылку на файл
  dropShareLink(fileId, token: string) {
    const putUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodShareLinkOff;
    return this.httpClient.post(putUrl, null, {
      headers: {
        'Authorization': 'Bearer ' + token
      },
      params: {
        fileId: fileId
      }
    });
  }

  // Получить файл по ссылке
  getFileByLink(link: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionPublic + this.methodGetFile + link;
    return this.httpClient.get<Resource>(getUrl);
  }

  // Скачать файл по ссылке
  downloadFileByLink(link: string, filename: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionPublic + this.methodDownloadLink + link;
    this.httpClient.get(getUrl, {
      responseType: 'blob'
    }).subscribe(blob => {
      saveAs(blob, filename);
    });
  }

  // Залогиниться в системе
  login(email: string, password: string) {
    const postUrl = this.appConfig.apiCloud + this.sectionPublic + this.methodAuth;
    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);
    return this.httpClient.post(postUrl, formData);

  }

  // Залогиниться в системе CAS
  CASLogin(ticket: string) {
    const postUrl = this.appConfig.apiCloud + this.methodAuth;
    return this.httpClient.post(postUrl, null,
      {
        params: {
          ticket: ticket
        }
      });
  }

  // Регистрация пользователя
  signUp(userName, lastName, userUid, userEmail, password) {
    const postUrl = this.appConfig.apiCloud + this.sectionPublic + this.methodSignUp;
    const formData = new FormData();
    formData.append('username', userName);
    formData.append('lastname', lastName);
    formData.append('userUid', userUid);
    formData.append('email', userEmail);
    formData.append('password', password);
    return this.httpClient.post(postUrl, formData);
  }

  /**
   * Возвращает квоту пользователя
   *
   * @param token токен авторизованного пользователя
   */
  getUserQuota(token: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionUsers + this.methodGetUserQuota;
    return this.httpClient.get(getUrl, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  /**
   * Возвращает объем занятого пространства в байтах
   *
   * @param token токен авторизованного пользователя
   */
  getUserStorageFilling(token: string) {
    const getUrl = this.appConfig.apiCloud + this.sectionFiles + this.methodGetUserOccupiedSize;
    return this.httpClient.get(getUrl, {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    });
  }

  getAllUsers() {
    return [
      {
        id: '123',
        name: 'Peter',
        role: 'Администратор',
        creationDate: '01.08.2018',
        foldersWeight: '8Гб',
        maxFoldersWeight: '10Гб',
        condition: 'Работает'
      },
      {
        id: '456',
        name: 'Henry',
        role: 'Пользователь',
        creationDate: '02.08.2018',
        foldersWeight: '8Гб',
        maxFoldersWeight: '10Гб',
        condition: 'Заблокирован'
      },
      {
        id: '789',
        name: 'Olha',
        role: 'Администратор',
        creationDate: '10.08.2018',
        foldersWeight: '8Гб',
        maxFoldersWeight: '10Гб',
        condition: 'Работает'
      }
    ];
  }

}

const users = [
  {
    id: '123',
    name: 'Peter',
    role: 'Администратор',
    creationDate: '01.08.2018',
    foldersWeight: '8Гб',
    maxFoldersWeight: '10Гб',
    condition: 'Работает'
  },
  {
    id: '456',
    name: 'Henry',
    role: 'Пользователь',
    creationDate: '02.08.2018',
    foldersWeight: '8Гб',
    maxFoldersWeight: '10Гб',
    condition: 'Заблокирован'
  },
  {
    id: '789',
    name: 'Olha',
    role: 'Администратор',
    creationDate: '10.08.2018',
    foldersWeight: '8Гб',
    maxFoldersWeight: '10Гб',
    condition: 'Работает'
  }
];
