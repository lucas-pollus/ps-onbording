import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class ApiService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  public get<T>(
    url: string,
    params: HttpParams = new HttpParams(),
    headers: HttpHeaders = new HttpHeaders()
  ): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}${url}`, { params, headers });
  }

  public put(url: string, body: Object = {}): Observable<any> {
    return this.http.put(`${this.baseUrl}${url}`, body);
  }

  public post(url: string, body: Object = {}): Observable<any> {
    return this.http.post(`${this.baseUrl}${url}`, body);
  }

  public path(url: string, body: Object = {}): Observable<any> {
    return this.http.patch(`${this.baseUrl}${url}`, body);
  }

  public delete(url: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}${url}`);
  }
}
