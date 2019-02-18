import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    public resultHttpClient: any;

    constructor(
        private http: HttpClient,
    ) { }

    ngOnInit(): void {
        this.http.get('http://localhost:5000/api/todo').subscribe((result) => {
            this.resultHttpClient = result;
        });
    }
}

