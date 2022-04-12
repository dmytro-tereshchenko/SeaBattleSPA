import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamePrepComponent } from './game-prep.component';

describe('GamePrepComponent', () => {
  let component: GamePrepComponent;
  let fixture: ComponentFixture<GamePrepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GamePrepComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GamePrepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
