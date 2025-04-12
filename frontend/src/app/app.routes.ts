import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./home/home/home.component')
      .then((r) => r.HomeComponent)
  }
];
