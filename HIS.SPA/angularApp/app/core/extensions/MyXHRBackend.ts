import { Injectable } from '@angular/core';
import { XHRBackend, Request, XHRConnection, Response, BrowserXhr, ResponseOptions, XSRFStrategy } from '@angular/http';
import {Observable} from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Router } from '@angular/router';

@Injectable()
export class MyXHRBackend extends XHRBackend {

    constructor(private router: Router, _browserXHR: BrowserXhr, _baseResponseOptions: ResponseOptions, _xsrfStrategy: XSRFStrategy) {
        super(_browserXHR, _baseResponseOptions, _xsrfStrategy);
    }

  createConnection(request: Request): XHRConnection {
    let connection: XHRConnection = super.createConnection(request);
    // Before returning the connection we try to catch all possible errors(4XX,5XX and so on)
    connection.response = connection.response.catch(this.processResponse);
    return connection;
  };

  processResponse(response: Response) {
    switch (response.status) {
      case 401:
        // You could redirect to login page here
        this.router.navigate(['/login']);
        break;
        //return Observable.throw('your custom error here');
      /*case 403:
        // You could redirect to forbidden page here
        return Observable.throw('your custom error here');
      case 404:
        // You could redirect to 404 page here
        return Observable.throw('your custom error here');*/
      default:
        return Observable.throw(response);
    }
  }
}