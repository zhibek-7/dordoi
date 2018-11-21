import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule} from '@angular/router';


import { CoreRoutingModule } from './core-routing.model';

import { HeaderComponent } from './header/header.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoginComponent } from './login/login.component';


@NgModule({
    imports: [ 
        CommonModule,
        CoreRoutingModule
    ],
    declarations: [
        NotFoundComponent,
        HeaderComponent,
        LoginComponent
    ],
    exports: [
        RouterModule,
        HeaderComponent
    ],
    providers: [],
})
export class CoreModule {}