import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';

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
        this.http.get(`${environment.apiUrl}/todo`).subscribe((result) => {
            this.resultHttpClient = result;
        });
    }
}

