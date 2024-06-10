import { Observable } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { AuthService, User } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class DataApiService {

  constructor(private httpClient: HttpClient,
    private authService: AuthService) {
  }

  public GetData<T>(path: string): Observable<T> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => this._GetResourceApi<T>(user ? user.access_token : "", path))
    );
  }

  private _GetResourceApi<T>(token: string, path: string): Observable<T> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.get<T>(`${environment.apiRoot}/${path}`, { headers });
  }

  public PostData<T>(path: string, data: any): Observable<any> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => this._PostResourceApi(user ? user.access_token : "", path, data))
    );
  }

  private _PostResourceApi<T>(token: string, path: string, data: any): Observable<T> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.post<T>(`${environment.apiRoot}/${path}`, data, { headers });
  }

  public PutData<T>(path: string, data: any): Observable<any> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => this._PutResourceApi(user ? user.access_token : "", path, data))
    );
  }

  private _PutResourceApi<T>(token: string, path: string, data: any): Observable<T> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });
    return this.httpClient.put<T>(`${environment.apiRoot}/${path}`, data, { headers });
  }

  public DeleteData<T>(path: string, data: any): Observable<any> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => this._DeleteResourceApi(user ? user.access_token : "", path, data))
    );
  }

  private _DeleteResourceApi<T>(token: string, path: string, body: any): Observable<T> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.delete<T>(`${environment.apiRoot}/${path}`, { headers, body });
  }
}
