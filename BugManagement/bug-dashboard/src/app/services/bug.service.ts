import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bug } from '../models/bug';

@Injectable({
  providedIn: 'root'
})
export class BugService {

  private apiUrl = 'https://localhost:7195/api/bugs'; // your API URL

  constructor(private http: HttpClient) { }

  getAll(): Observable<Bug[]> {
    return this.http.get<Bug[]>(this.apiUrl);
  }

  getById(id: number): Observable<Bug> {
    return this.http.get<Bug>(`${this.apiUrl}/${id}`);
  }

  create(bug: Bug): Observable<any> {
    return this.http.post(this.apiUrl, bug);
  }

  update(id: number, bug: Bug): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, bug);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
