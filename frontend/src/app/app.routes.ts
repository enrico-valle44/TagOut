import { Routes } from '@angular/router';
import { Map } from './components/map/map';
import { Feed } from './components/feed/feed';
import { Detail } from './components/detail/detail';
import { User } from './components/user/user';
import { NotFound } from './components/not-found/not-found';

export const routes: Routes = [
  { path: '', redirectTo: '/map', pathMatch: 'full' },
  { path: 'map', component: Map },
  { path: 'feed', component: Feed },
  { path: 'detail/:id', component: Detail },
  { path: 'user', component: User },
  { path: '**', component: NotFound },
];
