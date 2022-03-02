import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MatSliderModule } from '@angular/material/slider';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { UserStatusComponent } from './components/user-status/user-status.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestUserComponent } from './components/test-user/test-user.component';
import { StartMenuComponent } from './components/start-menu/start-menu.component';
import { CreateGameComponent } from './components/create-game/create-game.component';
import { SearchGameTableComponent } from './components/search-game-table/search-game-table.component';
import { GamePrepComponent } from './components/game-prep/game-prep.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    UserStatusComponent,
    TestUserComponent,
    StartMenuComponent,
    CreateGameComponent,
    SearchGameTableComponent,
    GamePrepComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: StartMenuComponent },
      { path: 'user-status', component: TestUserComponent },
      { path: 'create-game', component: CreateGameComponent },
      { path: 'join-game', component: SearchGameTableComponent },
      { path: 'game-prep', component: GamePrepComponent },
    ]),
    BrowserAnimationsModule,
    MatSliderModule,
    MatButtonModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
