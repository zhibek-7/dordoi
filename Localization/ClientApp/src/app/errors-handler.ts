import { ErrorHandler, Injectable, Injector } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { NgxSpinnerService } from "ngx-spinner";
import { NotifierService } from "angular-notifier";
//import {AuthorizationService} from '../services/authorization/authorization.service';

/**
 * Общий класс по обработке ошибок
 */
@Injectable()
export class ErrorsHandler implements ErrorHandler {
  constructor(private injector: Injector) {}

  // TODO localization
  handleError(error: Error | HttpErrorResponse): void {
    const spinner = this.injector.get(NgxSpinnerService);
    spinner.hide();

    console.error(error);

    if (error instanceof HttpErrorResponse) {
      // server error

      if (!navigator.onLine || error.status === 0) {
        this.notify("error", "Сервер недоступен");
      } else {
        if (error.status === 401) {
          // const authService = this.injector.get(AuthorizationService);
          //authService.logout();
          this.notify(
            "error",
            "Необходимо сначала войти в систему, либо учётные данные некорректны"
          );
        } else if (error.status === 403) {
          this.notify("error", "Недостаточно прав для доступа в эту область");
        } else if (typeof error.error === "string") {
          this.handleStringError(error.error);
        } else if (error.error instanceof Blob) {
          this.handleBlobErrorAsJson(error.error);
        } else {
          this.notify("error", "Неизвестная ошибка");
        }
      }
    } else {
      // client error
      // TODO log to server
    }
  }

  private handleStringError(error: string) {
    // TODO придумать более вменяемый способ передачи ошибки
    if (error.startsWith("Can't access file")) {
      this.notify("error", "Не удалось получить доступ к файлу");
      return;
    } else if (error.startsWith("Quota still in use")) {
      this.notify("error", "Не удалось удалить квоту - на неё еще есть ссылки");
      return;
    }

    // @ts-ignore
    switch (error) {
      case "Invalid login and/or password":
        this.notify("error", "Неверный логин или пароль");
        break;

      case "Can't download file":
        this.notify("error", "Не удаётся скачать файл!");
        break;

      default:
        this.notify("error", "Неизвестная ошибка");
    }
  }

  private handleBlobErrorAsJson(error: Blob) {
    const reader = new FileReader();
    reader.onload = (e: Event) => {
      this.handleStringError((<any>e.target).result);
    };
    reader.readAsText(error);
  }

  notify(type: string, message: string) {
    const notifier = this.injector.get(NotifierService);

    if (notifier) {
      notifier.notify(type, message);
    } else {
      alert(message);
    }
  }
}
