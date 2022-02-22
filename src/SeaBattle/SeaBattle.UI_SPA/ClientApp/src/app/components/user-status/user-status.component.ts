import { Component, OnInit } from '@angular/core';
import { AuthService, User } from '../../core/services/auth.service';
import { DataApiService } from '../../core/services/data-api.service';

@Component({
  selector: 'app-user-status',
  templateUrl: './user-status.component.html',
  styleUrls: ['./user-status.component.css']
})
export class UserStatusComponent implements OnInit {

  constructor(public authService: AuthService, public apiService: DataApiService) {
    this.currentUser=null;
  }

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

  public onLogout() {
    this.authService.logout().catch(err => console.log(err));
  }
}
