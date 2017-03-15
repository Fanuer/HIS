import { Injectable } from '@angular/core';

@Injectable()
export class Configuration {
    public Server = 'http://localhost:5003/';
    public AuthServer = 'https://his-auth.azurewebsites.net';
}