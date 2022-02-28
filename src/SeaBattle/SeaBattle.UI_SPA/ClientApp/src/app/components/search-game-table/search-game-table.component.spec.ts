import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchGameTableComponent } from './search-game-table.component';

describe('SearchGameTableComponent', () => {
  let component: SearchGameTableComponent;
  let fixture: ComponentFixture<SearchGameTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchGameTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchGameTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
