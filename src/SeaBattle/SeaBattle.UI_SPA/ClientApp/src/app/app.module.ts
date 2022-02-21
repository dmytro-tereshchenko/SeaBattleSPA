import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { UserStatusComponent } from './user-status/user-status.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestUserComponent } from './test-user/test-user.component';
import { StartMenuComponent } from './start-menu/start-menu.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    UserStatusComponent,
    TestUserComponent,
    StartMenuComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: StartMenuComponent, pathMatch: 'full' },
      { path: 'user-status', component: TestUserComponent },
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
