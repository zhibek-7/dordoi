import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  private appConfig: any;

  constructor(private httpClient: HttpClient) { }

  loadAppConfig(): Promise<any> {
    return this.httpClient.get('/assets/app-config.json')
      .toPromise()
      .then(data => {
        this.appConfig = data;
      });
  }

  get config() {
    return this.appConfig;
  }
}
