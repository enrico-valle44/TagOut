import { Router, Routes } from '@angular/router';
import { Map } from './components/map/map';
import { Feed } from './components/feed/feed';
import { Detail } from './components/detail/detail';
import { Profile } from './components/profile/profile';
import { NotFound } from './components/not-found/not-found';
import { NewReport } from './components/new-report/new-report';
import { inject } from '@angular/core';
import { AccessService } from './services/access-service/access-service';
import { Register } from './components/register/register';

export const routes: Routes = [
    {
    path: '',
    pathMatch: 'full',
    canActivate: [() => {
      const accessoServ = inject(AccessService);
      const router = inject(Router);

      if (accessoServ.haUtente()) {
        return router.createUrlTree(['/map']);
      } else {
        return router.createUrlTree(['/register']);
      }
    }],
    component: Register 
  },
  { path: 'register', component: Register },
  { path: 'map', component: Map },
  { path: 'feed', component: Feed },
  { path: 'detail/:id', component: Detail },
  { path: 'profile', component: Profile },
  { path: 'new-report', component: NewReport },
  { path: '**', component: NotFound },
];
