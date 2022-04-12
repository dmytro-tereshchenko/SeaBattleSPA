import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyShipComponent } from './buy-ship.component';

describe('BuyShipComponent', () => {
  let component: BuyShipComponent;
  let fixture: ComponentFixture<BuyShipComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BuyShipComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BuyShipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
