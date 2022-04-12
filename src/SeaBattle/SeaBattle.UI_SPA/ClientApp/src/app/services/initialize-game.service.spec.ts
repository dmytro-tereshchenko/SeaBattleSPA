import { TestBed } from '@angular/core/testing';

import { InitializeGameService } from './initialize-game.service';

describe('InitializeGame.ServiceService', () => {
  let service: InitializeGameService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InitializeGameService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
