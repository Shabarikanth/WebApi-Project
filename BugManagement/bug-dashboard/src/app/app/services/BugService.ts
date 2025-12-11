import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bug } from '../models/bug';


@Injectable({
    providedIn: 'root'
})
export class BugService {
    // base URL for API. If you use a proxy config, this can be '/api/bugs'
    private base = '/api/bugs';

    constructor(private http: HttpClient) { }

    getAll(): Observable<Bug[]> {
        return this.http.get<Bug[]>(this.base);
    }

    get(id: number): Observable<Bug> {
        return this.http.get<Bug>(`${this.base}/${id}`);
    }

    create(bug: Partial<Bug>): Observable<Bug> {
        return this.http.post<Bug>(this.base, bug);
    }

    update(id: number, bug: Partial<Bug>): Observable<void> {
        return this.http.put<void>(`${this.base}/${id}`, bug);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.base}/${id}`);
    }
}
