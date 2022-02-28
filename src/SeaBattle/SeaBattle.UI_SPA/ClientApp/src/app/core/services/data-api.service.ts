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

  constructor(private httpClient: HttpClient, private authService: AuthService) {
  }

  public GetData<T>(path: string): Observable<T> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => { return this._GetResourceApi<T>(user ? user.access_token : "", path); })
    );
  }

  private _GetResourceApi<T>(token: string, path: string): Observable<T> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.get<T>(`${environment.apiRoot}${path}`, { headers });
  }

  public PostData<T>(path: string, data: any): Observable<any> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => { return this._PostResourceApi(user ? user.access_token : "", path, data); })
    );
  }

  private _PostResourceApi<T>(token: string, path: string, data: any): Observable<T> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.post<T>(`${environment.apiRoot}${path}`, data, { headers });
  }

  public PutData<T>(path: string, data: any): Observable<any> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => { return this._PutResourceApi(user ? user.access_token : "", path, data); })
    );
  }

  private _PutResourceApi<T>(token: string, path: string, data: any): Observable<T> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.put<T>(`${environment.apiRoot}${path}`, data, { headers });
  }

  //Previous version of call api. Need for testing end reusing in future feature.

  // public callApi(): Promise<any> {
  //   return this.authService.getUser().then((user: User | null) => {
  //     if (user && user.access_token) {
  //       return this._callApi(user.access_token);
  //     } else if (user) {
  //       return this.authService.renewToken().then((user: User) => {
  //         return this._callApi(user.access_token);
  //       });
  //     } else {
  //       throw new Error('user is not logged in');
  //     }
  //   });
  // }

  // _callApi(token: string): any {
  //   const headers = new HttpHeaders({
  //     Accept: 'application/json',
  //     Authorization: 'Bearer ' + token,
  //   });

  //   return this.httpClient.get(`${environment.apiRoot}weatherforecast`, { headers })
  //     .toPromise()
  //     .catch((result: HttpErrorResponse) => {
  //       if (result.status === 401) {
  //         return this.authService.renewToken().then(user => {
  //           return this._callApi(user.access_token);
  //         });
  //       }
  //       throw result;
  //     });
  // }
}
