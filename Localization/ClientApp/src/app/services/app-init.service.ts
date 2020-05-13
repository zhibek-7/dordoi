import { Injectable } from '@angular/core';
import { AppConfigService } from 'src/services/app-config.service';

@Injectable({
  providedIn: 'root'
})
export class AppInitService {

  constructor(private appConfigService: AppConfigService) {
  }

  initApp(): Promise<any> {
    return this.appConfigService.loadAppConfig();
  }

}
