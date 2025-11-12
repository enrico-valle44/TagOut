import { Injectable } from '@angular/core';
import { Utente } from './../../model/utente';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  legge<T>(chiave: string): T | null {
    try {
      const raw = localStorage.getItem(chiave);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }

  getId(): number {
    const raw = localStorage.getItem('tagout-utente');
    if (!raw) return 0;

    try {
      const utente = JSON.parse(raw);
      return utente?.id ? Number(utente.id) : 0;
    } catch (e) {
      console.error('Errore nel parsing del localStorage:', e);
      return 0;
    }
  }
  
  salva(chiave: string, valore: unknown): void {
    localStorage.setItem(chiave, JSON.stringify(valore));
  }

  rimuovi(chiave: string): void {
    localStorage.removeItem(chiave);
  }
}
