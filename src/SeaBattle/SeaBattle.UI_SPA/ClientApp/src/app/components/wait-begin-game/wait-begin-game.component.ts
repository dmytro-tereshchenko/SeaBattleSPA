import { Component, OnInit } from '@angular/core';
import { DataGameService } from '../../services/data-game.service';
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

}
