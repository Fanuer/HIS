import { Injectable } from '@angular/core';

@Injectable()
export class Configuration {
    public Server = 'https://his-apigateway.azurewebsites.net';
    public AuthServer = 'https://his-auth.azurewebsites.net';
}