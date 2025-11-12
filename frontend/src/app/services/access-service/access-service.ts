import { Injectable } from '@angular/core';
import { Utente } from '../../model/utente';
import { LocalStorageService } from '../local-storage-service/local-storage.service';

@Injectable({
  providedIn: 'root'
})

export class AccessService {
  private chiave = 'tagout-utente';

  private utente: Utente = {
  username: '',
  dataNascita: '',
  gender: 'A'
  }; //conterrà l'id dopo la post

  constructor(private memoria: LocalStorageService) {}

  salvaInfoUtente(utente: Utente): void {
    this.memoria.salva(this.chiave, utente);
    this.utente.username = utente.username;
    this.utente.dataNascita = utente.dataNascita;
    this.utente.gender = utente.gender;
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
      this.utente.id = data; 
      // console.log('Utente:', this.utente.id);
      this.memoria.salva(this.chiave, this.utente)
      return data;
    })
    .catch(err => {
      console.error('Errore POST:', err);
      throw err; 
    });
  }

  // salvaId() {
  //   this.memoria.salva(this.chiave, this.utente)
  // }

}

