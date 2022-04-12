import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { GameSizeLimit } from '../../data/game-size-limit';
import { InitializeGameService } from '../../services/initialize-game.service';
import { DataGameService } from '../../services/data-game.service';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { AuthService } from '../../core/services/auth.service';
import { Game } from '../../data/game';
import { mergeMap } from 'rxjs/operators';
import { Router } from "@angular/router"

@Component({
  selector: 'app-create-game',
  templateUrl: './create-game.component.html',
  styleUrls: ['./create-game.component.scss']
})
export class CreateGameComponent implements OnInit {

  constructor(private location: Location,
    private apiService: InitializeGameService,
    private gameService: DataGameService,
    private gameFieldService: DataGameFieldService,
    private userService: AuthService,
    private router: Router) {
    this.players = 2;
    this.gameSize = {
      maxPlayerSize: 4,
      fieldMaxSizeX: 100,
      fieldMaxSizeY: 100,
      fieldMinSizeX: 10,
      fieldMinSizeY: 10
    } as GameSizeLimit;
    this.sizeX = this.gameSize.fieldMinSizeX;
    this.sizeY = this.gameSize.fieldMinSizeY;
  }

  players: number;
  sizeX: number;
  sizeY: number;
  gameSize: GameSizeLimit;

  ngOnInit(): void {
    this.gameService.getGame()
      .subscribe(g => {
        switch (g.gameState) {
          case 2: {
            this.router.navigate(['/join-game']);
            break;
          }
          case 3: {
            this.userService.getUser().then(u => {
              if (g.players.find(p => p.name === u?.profile.name ?? "")?.playerState === 3) {
                this.router.navigate(['/wait-begin-game']);
              }
            })
            this.router.navigate(['/game-prep']);
            break;
          }
          case 4: {
            this.router.navigate(['/game']);
            break;
          }
        }
      });
    this.apiService.GetGameSize().subscribe(result => {
      this.gameSize = result;
      this.sizeX = result.fieldMinSizeX;
      this.sizeY = result.fieldMinSizeY;
    })
  }

  goBack(): void {
    this.location.back();
  }

  createGame(): void {
    this.gameService.createGame(this.players)
      .pipe(mergeMap(game => this.gameFieldService.createGameField(this.sizeX, this.sizeY)))
      .subscribe(g => this.router.navigate(['/join-game']));
  }

}
