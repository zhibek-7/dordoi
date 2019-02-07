import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from '@angular/common/http';

import {
  MatMenuModule,
  MatIconModule,
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

import { AdministrationRoutingModule } from './administration-routing.module';

import { AdministrationComponent } from './components/administration/administration.component';
import { AuthComponent } from './components/auth/auth.component';
import { EventsComponent } from './components/events/events.component';
import { FoldersComponent } from './components/folders/folders.component';
import { ServersComponent } from './components/servers/servers.component';
import { SettingsComponent } from './components/settings/settings.component';
import { UsersListComponent } from './components/users-list/users-list.component';

@NgModule({
  declarations: [
    AdministrationComponent,
    AuthComponent,
    EventsComponent,
    FoldersComponent,
    ServersComponent,
    SettingsComponent,
    UsersListComponent,
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
    MatFormFieldModule,
    MatTableModule,
    MatPaginatorModule,
    CdkTreeModule,
    MatProgressBarModule,
  ],
  providers: [
  ]
})
export class AdministrationModule { }
