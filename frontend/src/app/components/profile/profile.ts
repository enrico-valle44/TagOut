import { Component, inject  } from '@angular/core';
import { ReportCard } from '../report-card/report-card';
import { Report } from '../../model/report';
import { Utente } from '../../model/utente';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';
import { DataService } from './../../services/data-service/data-service';
import { LocalStorageService } from '../../services/local-storage-service/local-storage.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-profile',
  imports: [ MatButtonModule, MatIconModule, RouterLink, CommonModule, MatCardModule, MatChipsModule, MatProgressSpinnerModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile {

  // servizi (per dopo)
  private dataServ = inject(DataService);
  private memoria = inject(LocalStorageService);
  private router = inject(Router);
  // segnali o proprietà per dati
  public utente: Utente | null = null;
  public userReports: Report[] = [];

  public confirmDeleteId: number | null = null; // id del report per cui sto chiedendo conferma di eliminazione 
  public isDeletingId: number | null = null; /// id del report che è in fase di eliminazione (per mostrare "in corso")
  public deleteError: string | null = null; // messaggio di errore legato alla delete 

  public isLoadingReports: boolean = true;

  constructor() {
    this.loadProfile();
  }

  async loadProfile() {
    const idUser = this.memoria.getId(); //uso servizio di localstorage
    if (!idUser) {
      console.log('Nessun id utente nel localStorage.');
      this.utente = null;
      this.userReports = [];
      this.isLoadingReports = false; // ha finito comunque
      return;
    }
    try {
      // utente
      this.utente = await this.dataServ.getUserInfo(idUser);

      // report dell’utente
      this.isLoadingReports = true; //inizio caricamento report
      this.userReports = await this.dataServ.getReportsByUserId(idUser) || [];

      console.log('Utente:', this.utente);
      console.log('Report utente:', this.userReports);
    } catch (err) {
      console.error('Errore caricando profilo/report', err);
      this.userReports = [];
    } finally {
      this.isLoadingReports = false; //finito il caricamento (anche se con errore)
    }
  }

  //utile per il caricamento in lista dei report
  trackByReportId(index: number, report: Report) {
    return report.id;
  }
  logout() {
    // pulisco i dati utente nel localStorage
    this.memoria.rimuovi('tagout-utente');  
    // 2) navigo verso la pagina di registrazione/login
    this.router.navigate(['/register']);
  }


  /** Primo click su "Elimina": attivo la richiesta di conferma per quel report */
  startDelete(idReport: number | undefined) {
    if (!idReport) return;
    this.deleteError = null;

    // se clicco di nuovo sullo stesso, annullo la conferma
    if (this.confirmDeleteId === idReport) {
      this.confirmDeleteId = null;
    } else {
      this.confirmDeleteId = idReport;
    }
  }

  /** Click su "Annulla" nella conferma */
  cancelDelete() {
    this.confirmDeleteId = null;
    this.isDeletingId = null;
    this.deleteError = null;
  }

  /** Click su "Conferma": chiamo il backend e aggiorno la lista */
  async confirmDelete(idReport: number | undefined) {
    if (!idReport) return;

    this.isDeletingId = idReport;
    this.deleteError = null;

    try {
      await this.dataServ.deleteReport(idReport);

      // rimuovo dalla lista in memoria
      this.userReports = this.userReports.filter(r => r.id !== idReport);

      // reset stato conferma
      if (this.confirmDeleteId === idReport) {
        this.confirmDeleteId = null;
      }
    } catch (err) {
      console.error(err);
      this.deleteError = 'Impossibile eliminare il report. Riprova più tardi.';
    } finally {
      this.isDeletingId = null;
    }
  }

}
