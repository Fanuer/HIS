import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnauthorizedComponent } from './components/unauthorized.component';
import { UnauthorizedRoutes } from './unauthorized.routes';

@NgModule({
    imports: [
        CommonModule,
        UnauthorizedRoutes
    ],

    declarations: [
        UnauthorizedComponent
    ],
})

export class UnauthorizedModule { }