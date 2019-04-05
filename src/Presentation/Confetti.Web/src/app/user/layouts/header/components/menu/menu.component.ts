import { Component, OnInit, Input } from '@angular/core';
import { TopMenuModel } from '@app/user/models/catalog/top_menu_model';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  @Input()
  public topMenuModel: TopMenuModel;

  constructor() { }

  ngOnInit() {
  }

}
