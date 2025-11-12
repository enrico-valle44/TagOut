import { Component, inject  } from '@angular/core';
import { ReportCard } from '../report-card/report-card';
import { Report } from '../../model/report';
import { Utente } from '../../model/utente';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { DataService } from './../../services/data-service/data-service';
import { LocalStorageService } from '../../services/local-storage-service/local-storage.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-profile',
  imports: [ReportCard, MatButtonModule, MatIconModule, RouterLink, CommonModule, MatCardModule, MatChipsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile {

  // servizi (per dopo)
  private dataServ = inject(DataService);
  private memoria = inject(LocalStorageService);

  // segnali o proprietà per dati
  public utente: Utente | null = null;
  public userReports: Report[] = [];

  constructor() {
    this.loadProfile();
  }

  async loadProfile() {
    const idUser = this.memoria.getId(); //uso servizio di localstorage
    if (!idUser) {
      console.log('Nessun id utente nel localStorage.');
      this.utente = null;
      this.userReports = [];
      return;
    }
    // se lo trova awaitamente prende i dati utente
    this.utente = await this.dataServ.getUserInfo(idUser);

    // report dell’utente
    this.userReports = await this.dataServ.getReportsByUserId(idUser);

    //controlli
    console.log('Utente:', this.utente);
    console.log('Report utente:', this.userReports);

  }

  //utile per il caricamento in lista dei report
  trackByReportId(index: number, report: Report) {
    return report.id;
  }
}
