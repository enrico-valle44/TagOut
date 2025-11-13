import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DataService } from './../../services/data-service/data-service';
import { LocationService } from '../../services/location-service/location-service';
import { ReportCard } from '../report-card/report-card';
import { Report } from '../../model/report';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-feed',
  imports: [ReportCard, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed {
  private dataServ = inject(DataService);
  private locationServ = inject(LocationService);
  public reports: Report[] = [];

  constructor() {
    // this.dataServ.getReports().then((data) => {
    //   if ('geolocation' in navigator) {
    //     this.locationServ.sortReportsByDistance(data)
    //     .then((sortedReports) => this.reports = sortedReports)
    //   } else {
    //     this.reports = data;
    //   }
    // });

    // this.dataServ.getReports().then((data) => {
    //   if ('geolocation' in navigator) {
    //     return this.locationServ.sortReportsByDistance(data)
    //   } else {
    //     return data;
    //   }
    // }).then(newReports => this.reports = newReports);
    this.loadReports();
  }

  async loadReports() {
    const unorderedReports = await this.dataServ.getReports();

    this.reports = this.sortReportsByDateTime(this.reports);

    if ('geolocation' in navigator) {
      const orderedByDistance = await this.locationServ.sortReportsByDistance(
        unorderedReports
      );
      this.reports = this.sortReportsByDateTime(orderedByDistance);
    } else {
      this.reports = this.sortReportsByDateTime(unorderedReports);
    }
  }

  private sortReportsByDateTime(reports: Report[]): Report[] {
    return reports.sort((a, b) => {
      const dateA = new Date(a.date);
      const dateB = new Date(b.date);

      return dateB.getTime() - dateA.getTime();
    });
  }
}
