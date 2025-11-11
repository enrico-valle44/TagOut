import { Injectable } from '@angular/core';
import { Utente } from '../../model/utente';
import { LocalStorageService } from '../local-storage-service/local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class Accesso {
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
}
