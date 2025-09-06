import {
  trigger,
  style,
  animate,
  transition,
  query,
} from '@angular/animations';
import { Component } from '@angular/core';

import { ChildrenOutletContexts, RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-user',
  imports: [RouterOutlet],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
  animations: [
    trigger('routerFadeIn', [
      transition('* <=> *', [
        query(
          ':enter',
          [
            style({ opacity: 0 }),
            animate('1s ease-in-out', style({ opacity: 1 })),
          ],
          { optional: true }
        ),
      ]),
    ]),
  ],
})
export class UserComponent {
  //childrenoutletcontext contains the routeUrl etc contextual information about childs
  constructor(private context: ChildrenOutletContexts) {}
  getRouteUrl() {
    return this.context.getContext('primary')?.route?.url; //primary refering to direct children
  }
}
