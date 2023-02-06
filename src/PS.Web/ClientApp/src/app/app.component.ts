import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { PoMenuItem } from '@po-ui/ng-components';
import { filter, map, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit, OnDestroy {
  title!: string;

  menus: PoMenuItem[] = [];

  private _subscription!: Subscription;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this._setTitle();
    this._setMenus();
  }

  ngOnDestroy(): void {
    this._subscription.unsubscribe();
  }

  private _setMenus() {
    this.menus = [
      {
        label: 'Categorias Produtos',
        link: '/categorias-produtos',
        shortLabel: 'Categorias',
        icon: 'fa fa-boxes-stacked',
      },
    ];
  }

  private _setTitle() {
    const subRouteEvents = this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .pipe(map(() => this.activatedRoute))
      .pipe(
        map((route) => {
          while (route.firstChild) route = route.firstChild;
          return route;
        })
      )
      .pipe(switchMap((route) => route.title))
      .subscribe((title) => (this.title = title ?? ''));

    this._subscription?.add(subRouteEvents);
  }
}
