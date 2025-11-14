import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LocalStorageService } from '../../services/local-storage-service/local-storage.service';
import { Utente } from '../../model/utente';

@Component({
  selector: 'app-bentornato',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
  ],
  templateUrl: './bentornato.html',
  styleUrl: './bentornato.scss',
})
export class Bentornato {

  // servizi
  private memoria = inject(LocalStorageService);
  private router = inject(Router);
  private snack = inject(MatSnackBar);

  // dati
  public utente: Utente | null = null;

  constructor() {
     console.log('Bentornato constructor'); //per controllare in console
    // recupero utente
    this.utente = this.memoria.legge('tagout-utente');
    console.log('Utente in bentornato:', this.utente);
    
    this.snack.open('Bentornato!', '', {
      duration: 1500,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }

  entra() {
    this.router.navigate(['/map']);
  }
}
