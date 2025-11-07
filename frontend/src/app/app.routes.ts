import { Router, Routes } from '@angular/router';
import { Map } from './components/map/map';
import { Feed } from './components/feed/feed';
import { Detail } from './components/detail/detail';
import { User } from './components/user/user';
import { NotFound } from './components/not-found/not-found';
import { NewReport } from './components/new-report/new-report';
import { inject } from '@angular/core';
import { Accesso as AccessoService } from './services/access-service/accesso-service';
export const routes: Routes = [
   { path: '', redirectTo: '/benvenuto', pathMatch: 'full' }, 

  {
    path: 'map',
    component: Map,
    canActivate: [() => {
      const accessoServ = inject(AccessoService);
      const router = inject(Router);
      return accessoServ.haUtente() || router.createUrlTree(['/benvenuto']);
    }],
  },

  { path: 'benvenuto', loadComponent: () => import('./components/accesso/accesso').then(c => c.Accesso) },
  { path: 'map', component: Map },
  { path: 'feed', component: Feed },
  { path: 'detail/:id', component: Detail },
  { path: 'user', component: User },
  { path: 'new-report', component: NewReport },
  { path: '**', component: NotFound },
];
