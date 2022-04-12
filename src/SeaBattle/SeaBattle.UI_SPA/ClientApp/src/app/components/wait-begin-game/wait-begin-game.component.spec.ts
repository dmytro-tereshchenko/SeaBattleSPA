import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaitBeginGameComponent } from './wait-begin-game.component';

describe('WaitBeginGameComponent', () => {
  let component: WaitBeginGameComponent;
  let fixture: ComponentFixture<WaitBeginGameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaitBeginGameComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaitBeginGameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
