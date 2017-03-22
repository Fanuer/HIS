
import { Component, OnInit } from '@angular/core';
import './home.component.scss';

@Component({
    selector: 'home-component',
    templateUrl: 'home.component.html'
})

export class HomeComponent implements OnInit {

    public message: string;

    constructor() {
    }

    ngOnInit() {
    }
}
