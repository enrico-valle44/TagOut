import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Report } from '../../model/report';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-report-card',
  imports: [MatCardModule, MatButtonModule,RouterLink],
  templateUrl: './report-card.html',
  styleUrl: './report-card.scss',
})
export class ReportCard {

  public report= input<Report>();
}
