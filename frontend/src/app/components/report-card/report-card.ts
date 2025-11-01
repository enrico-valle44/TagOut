import { Component, input } from '@angular/core';
import { MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Report } from '../../model/report';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-report-card',
  imports: [MatCardModule, MatButtonModule, RouterLink, MatIcon],
  templateUrl: './report-card.html',
  styleUrl: './report-card.scss',
})
export class ReportCard {
  public report = input<Report>();
}
