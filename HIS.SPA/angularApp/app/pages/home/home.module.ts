import { RecipeDashboardComponent } from './components/recipe-dashboard.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { HomeRoutes } from './home.routes';
import { HomeComponent } from './components/home.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        HomeRoutes
    ],

    declarations: [
        HomeComponent,
        RecipeDashboardComponent
    ],

    exports: [
        HomeComponent
    ]
})

export class HomeModule { }