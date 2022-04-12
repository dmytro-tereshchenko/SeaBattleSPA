import { Component, OnInit } from '@angular/core';
import { DataGameService } from '../../services/data-game.service';
import { AuthService } from '../../core/services/auth.service';
import { Game } from '../../data/game';
import { Router } from "@angular/router"

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private gameService: DataGameService,
    private userService: AuthService,
    private router: Router) { }

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
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
