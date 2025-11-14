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

  getReportsGeoJsonByCategory(name: string): Promise<FeatureCollection> {
    return fetch(`http://localhost:5089/Report/geojson/${name}`)
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

  getReportsByCategory(name: string): Promise<Report[]> {
    return fetch(`http://localhost:5089/Report/category/${name}`)
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
      .then(async (resp) => {
        if (!resp.ok) {
          console.error(`Errore HTTP ${resp.status}`);
          return null;
        }
        const raw = await resp.json(); // quello che arriva dal backend
        //cosa arriva davvero
        console.log('UserInfo:', raw);
        // mappo le proprietà del backend nel mio modello Utente
        const utente: Utente = {
          id: raw.id,
          username: raw.username,
          dataNascita: raw.dob ?? raw.dataNascita ?? '',
          gender: (raw.gender ?? 'A') as 'M' | 'F' | 'A',
        };

        return utente;
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
  /** Elimina un report: DELETE /Report/delete/{id} */
  deleteReport(idReport: number) {
    return fetch(`http://localhost:5089/Report/delete/${idReport}`, {
      method: 'DELETE',
    })
      .then((resp) => {
        if (!resp.ok) {
          console.error(
            `Errore DELETE report ${idReport} - HTTP ${resp.status}`
          );
          throw new Error(`Errore eliminazione report (status ${resp.status})`);
        }
      })
      .catch((err) => {
        console.error('Errore deleteReport:', err);
        throw err;
      });
  }
}
