import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LocalStorageService {
  legge<T>(chiave: string): T | null {
    try {
      const raw = localStorage.getItem(chiave);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }

  salva(chiave: string, valore: unknown): void {
    localStorage.setItem(chiave, JSON.stringify(valore));
  }

  rimuovi(chiave: string): void {
    localStorage.removeItem(chiave);
  }
}