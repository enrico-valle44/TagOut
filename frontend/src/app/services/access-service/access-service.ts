import { Injectable } from '@angular/core';
import { Utente } from '../../model/utente';
import { LocalStorageService } from '../local-storage-service/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AccessService {
  private chiave = 'tagout-utente';

  constructor(private memoria: LocalStorageService) {}

  salvaUtente(utente: Utente): void {
    this.memoria.salva(this.chiave, utente);
  }

  leggiUtente(): Utente | null {
    return this.memoria.legge<Utente>(this.chiave);
  }

  haUtente(): boolean {
    return !!this.leggiUtente();
  }

  eliminaUtente(): void {
    this.memoria.rimuovi(this.chiave);
  }

  postUtente(utente: Utente) {
    return fetch('http://localhost:5089/UserInfo/add', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(utente)
    })
    .then(resp => resp.json())
    .then(data => {
      console.log('Risposta server:', data);
      return data;
    })
    .catch(err => {
      console.error('Errore POST:', err);
      throw err; // rilancia l'errore se vuoi gestirlo a chi chiama la funzione
    });
  }

}

