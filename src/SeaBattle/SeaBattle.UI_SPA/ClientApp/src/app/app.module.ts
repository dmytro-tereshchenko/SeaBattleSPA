import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MatSliderModule } from '@angular/material/slider';
import { MatButtonModule } from '@angular/material/button';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { UserStatusComponent } from './components/user-status/user-status.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestUserComponent } from './components/test-user/test-user.component';
import { StartMenuComponent } from './components/start-menu/start-menu.component';
import { CreateGameComponent } from './components/create-game/create-game.component';
import { JoinGameComponent } from './components/join-game/join-game.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    UserStatusComponent,
    TestUserComponent,
    StartMenuComponent,
    CreateGameComponent,
    JoinGameComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: StartMenuComponent },
      { path: 'user-status', component: TestUserComponent },
      { path: 'create-game', component: CreateGameComponent },
      { path: 'join-game', component: JoinGameComponent },
    ]),
    BrowserAnimationsModule,
    MatSliderModule,
    MatButtonModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
