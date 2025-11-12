import { Injectable } from '@angular/core';
import { FeatureCollection } from 'geojson';
import { Report } from '../../model/report';
import { Category } from '../../model/category';
import { Utente } from '../../model/utente';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor() {}

  getReportsGeoJson(): Promise<FeatureCollection> {
    return fetch('http://localhost:5089/Report/geojson')
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  getReports(): Promise<Report[]> {
    return fetch('http://localhost:5089/Report/all')
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  getReport(id: number): Promise<Report> {
    return fetch(`http://localhost:5089/Report/${id}`)
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  getCategories(): Promise<string[]> {
    return fetch('http://localhost:5089/Category/all')
      .then((resp) => {
        if (!resp.ok) {
          throw new Error(`Errore HTTP ${resp.status}`);
        }
        return resp.json() as Promise<Category[]>;
      })
      .then((categories) => categories.map((c) => c.name))
      .catch((err) => {
        console.error('Errore fetching categories:', err);
        return [];
      });
  }

  postReport(report: Report, idUser: number) {
    return fetch(`http://localhost:5089/Report/add/${idUser}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(report),
    })
      .then((resp) => {
        if (!resp.ok) {
          throw new Error(`Errore HTTP ${resp.status}`);
        }
        return resp.json();
      })
      .then((data) => {
        console.log('Risposta server:', data);
        return data;
      })
      .catch((err) => {
        console.error('Errore POST:', err);
        throw err;
      });
  }

  //user
  
  /** Prende l'id dal localStorage e chiama GET /UserInfo/{id} */
  getUserInfo(idUser: number): Promise<Utente | null> {
    return fetch(`http://localhost:5089/UserInfo/${idUser}`)
      .then((resp) => {
        if (!resp.ok) {
          console.error(`Errore HTTP ${resp.status}`);
          return null;
        }
        return resp.json() as Promise<Utente>;
      })
      .catch((err) => {
        console.error('Errore getUserInfo:', err);
        return null;
      });
  }


  /** Ritorna i report dell’utente: GET /Report/byUser/{id} (adatta il path al tuo backend) */
  getReportsByUserId(idUser: number): Promise<Report[]> {
    return fetch(`http://localhost:5089/Report/user/${idUser}`)
      .then((resp) => {
        if (!resp.ok) {
          console.error(`getReportsByUserId - HTTP ${resp.status}`);
          return [];
        }
        return resp.json() as Promise<Report[]>;
      })
      .catch((err) => {
        console.error('Errore getReportsByUserId:', err);
        return [];
      });
}


}
