import { Component, OnInit } from '@angular/core';
import { DataGameService } from '../../services/data-game.service';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { DataShipService } from '../../services/data-ship.service';
import { DataStartFieldService } from '../../services/data-start-field.service';
import { Player } from '../../data/player';
import { interval, Subscription } from 'rxjs';
import { Router } from "@angular/router";

@Component({
  selector: 'app-wait-begin-game',
  templateUrl: './wait-begin-game.component.html',
  styleUrls: ['./wait-begin-game.component.scss']
})
export class WaitBeginGameComponent implements OnInit {

  constructor(private gameService: DataGameService,
    private gameFieldService: DataGameFieldService,
    private shipService: DataShipService,
    private startFieldService: DataStartFieldService,
    private router: Router) { }

  players: Player[];

  private updateTimeOut: number = 1000;
  private subscription: Subscription;

  ngOnInit(): void {
    const source = interval(this.updateTimeOut);
    this.update();
    this.subscription = source.subscribe(val => this.update());
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  update() {
    this.gameService.getGameFromServer().subscribe(game => {
      this.players = game.players
      if (game.gameState === 4) {
        this.router.navigate(['/game']);
      }
    });
  }

  quit() {
    this.gameService.getGame().subscribe(game => {
      if (game) {
        this.gameService.quitGame().subscribe(state => {
          if (state === 10) {
            this.gameService.clear();
            this.gameFieldService.clear();
            this.startFieldService.clear();
            this.shipService.clear();
          }
        })
      }
      this.router.navigate(['/']);
    })
  }

}
