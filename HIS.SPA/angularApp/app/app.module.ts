import { MyXHRBackend } from './core/extensions/MyXHRBackend';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, XHRBackend } from '@angular/http';

import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { HomeModule } from './pages/home/home.module';

import { Configuration } from './app.constants';
import { AppRoutes } from './app.routes';

import { AppComponent } from './app.component';
import { OAuthModule } from 'angular-oauth2-oidc/dist';

@NgModule({
    imports: [
        BrowserModule,
        AppRoutes,
        SharedModule,
        CoreModule.forRoot(),
        HomeModule,
        HttpModule,
        OAuthModule.forRoot()
    ],
    providers: [
        {provide: XHRBackend, useClass: MyXHRBackend}
    ],
    declarations: [
        AppComponent
    ],

    bootstrap: [AppComponent],
})

export class AppModule { }