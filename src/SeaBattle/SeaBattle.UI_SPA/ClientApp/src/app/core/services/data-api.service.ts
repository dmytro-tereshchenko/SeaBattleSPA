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

  public GetData(path: string): Observable<any> {
    return this.authService.getUserObservable().pipe(
      mergeMap(user => { return this._CallResourceApi(user ? user.access_token : "", path); })
    );
  }

  private _CallResourceApi(token: string, path: string): Observable<any> {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.get<any>(`${environment.apiRoot}${path}`, { headers });
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
