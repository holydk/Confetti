import { Component, OnInit, Input } from '@angular/core';
import { TopMenuModel } from '@app/user/models/catalog/top_menu_model';

@Component({
  selector: 'app-top-menu',
  templateUrl: './top-menu.component.html',
  styleUrls: ['./top-menu.component.scss']
})
export class TopMenuComponent implements OnInit {
  @Input()
  public topMenuModel: TopMenuModel;

  constructor() { }

  ngOnInit() {
  }
}
