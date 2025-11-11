import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Toolbar } from './components/toolbar/toolbar';
import { TopToolbar } from "./components/top-toolbar/top-toolbar";
import { LocalStorageService } from './services/local-storage-service/local-storage.service';
import { Utente } from './model/utente';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Toolbar, TopToolbar ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {

  protected readonly title = signal('frontend');

}
