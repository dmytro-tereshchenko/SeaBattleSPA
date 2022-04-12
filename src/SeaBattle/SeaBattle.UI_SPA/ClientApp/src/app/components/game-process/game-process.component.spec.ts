import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameProcessComponent } from './game-process.component';

describe('GameProcessComponent', () => {
  let component: GameProcessComponent;
  let fixture: ComponentFixture<GameProcessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GameProcessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GameProcessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
