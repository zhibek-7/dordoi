import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import {
  MatButtonModule,
  MatMenuModule,
  MatIconModule,
  MatInputModule,
} from '@angular/material';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { CdkTreeModule } from '@angular/cdk/tree';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';

import { DataImportService } from '../services/dataImport.service';
import { DataImportSignalRService } from '../services/dataImportSignalR.service';

import { AdministrationRoutingModule } from './administration-routing.module';

import { AdministrationComponent } from './components/administration/administration.component';
import { AuthComponent } from './components/auth/auth.component';
import { EventsComponent } from './components/events/events.component';
import { FoldersComponent } from './components/folders/folders.component';
import { ServersComponent } from './components/servers/servers.component';
import { SettingsComponent } from './components/settings/settings.component';
import { UsersListComponent } from './components/users-list/users-list.component';
import { DataImportComponent } from './components/data-import/data-import.component';
import { RequestInterceptorService } from '../services/requestInterceptor.service';

@NgModule({
  declarations: [
    AdministrationComponent,
    AuthComponent,
    EventsComponent,
    FoldersComponent,
    ServersComponent,
    SettingsComponent,
    UsersListComponent,
    DataImportComponent,
  ],
  imports: [
    AdministrationRoutingModule,
    FormsModule,
    CommonModule,
    HttpClientModule,
    MatMenuModule,
    MatIconModule,
    MatToolbarModule,
    MatSidenavModule,
    MatTabsModule,
    MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule,
    MatTableModule,
    MatPaginatorModule,
    CdkTreeModule,
    MatProgressBarModule,
    MatButtonModule,
    MatCheckboxModule,
    MatSelectModule,
  ],
  providers: [
    DataImportService,
    DataImportSignalRService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
  ]
})
export class AdministrationModule { }
