import { Component, OnInit } from '@angular/core';
import { AuthService, User } from '../../core/services/auth.service';
import { DataApiService } from '../../core/services/data-api.service';

import { Weather } from '../../data/weather';

@Component({
  selector: 'app-test-user',
  templateUrl: './test-user.component.html',
  styleUrls: ['./test-user.component.scss']
})
export class TestUserComponent implements OnInit {

  constructor(public authService: AuthService, public apiService: DataApiService) {
    this.currentUser=null;}

    ngOnInit(): void {
      this.authService.getUser().then(user => {
        this.currentUser = user;
      }).catch(err => console.log(err));
    }

    get currentUserJson(): string {
      return JSON.stringify(this.currentUser, null, 2);
    }
    currentUser: User|null;

    public onLogin() {
      this.authService.login().catch(err => {
        console.log(err);
      });
    }

    public onCallAPI3() {
      let Weather:Weather[];
      this.apiService.GetData<Weather[]>("weatherforecast/get").subscribe(result=> {Weather=result;console.log(Weather);});
    }

    public onRenewToken() {
      this.authService.renewToken()
        .then(user => {
          this.currentUser = user;
          console.log('Silent Renew Success');
        })
        .catch(err => console.log(err));
    }

    public onLogout() {
      this.authService.logout().catch(err => console.log(err));
    }

    //Previous version of call api. Need for testing end reusing in future feature.

    // public onCallAPI() {
    //   this.apiService.callApi().then(result => {
    //     console.log(this.currentUser);
    //     console.log('API Result: ' + JSON.stringify(result));
    //   }, err => console.log(err));
    // }

    // public onCallAPI2() {
    //   let Weather:Weather[];
    //   this.apiService.CallResourceApi(this.currentUser as User).subscribe(result => {Weather=result;console.log(Weather);});
    // }
}
