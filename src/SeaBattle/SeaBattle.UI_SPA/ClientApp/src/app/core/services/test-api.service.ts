import { Observable, from } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../../src/environments/environment';

import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

//import { Weather } from 'src/Weather';

import { AuthService, User } from './auth.service';
//import { resourceLimits } from 'worker_threads';

@Injectable({
  providedIn: 'root'
})
export class TestApiService {

  constructor(private httpClient: HttpClient, private authService: AuthService) {
  }

  public callApi(): Promise<any> {
    return this.authService.getUser().then((user: User|null) => {
      if (user && user.access_token) {
        return this._callApi(user.access_token);
      } else if (user) {
        return this.authService.renewToken().then((user: User) => {
          return this._callApi(user.access_token);
        });
      } else {
        throw new Error('user is not logged in');
      }
    });
  }

  // public CallResourceApi(user: User):Observable<Weather[]>{
  //   return this._CallResourceApi(user.access_token)
  // }

  public GetData(path: string):Observable<Observable<any>>{
    return this.authService.getUserObservable().pipe(
      map(user => {return this._CallResourceApi(user?user.access_token:"", path);})
      );
  }

  // public CallResourceApi3():Observable<Weather[]>{
  //   return this.authService.getUser2().subscribe(user => {return this._CallResourceApi(user?user.access_token:"");});
  // }

  private _CallResourceApi(token: string, path: string):Observable<any>{
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.get<any>(`${environment.apiRoot}${path}`, { headers });
  }

  _callApi(token: string):any {
    const headers = new HttpHeaders({
      Accept: 'application/json',
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.get(`${environment.apiRoot}weatherforecast`, { headers })
      .toPromise()
      .catch((result: HttpErrorResponse) => {
        if (result.status === 401) {
          return this.authService.renewToken().then(user => {
            return this._callApi(user.access_token);
          });
        }
        throw result;
      });
  }
}
