import { Injectable } from '@angular/core';
import { DataService } from '../data-service/data-service';
import { Report } from '../../model/report';

@Injectable({
  providedIn: 'root',
})
export class DetailService {
  constructor(private dataServ: DataService) {}

  get(id: number): Promise<Report> {
    return this.dataServ.getReports().then((list) => {
      const report = list.find((rep) => rep.id === id);
      if (!report) throw new Error('Report non trovato');
      return report;
    });
  }
}
