import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { signal } from '@angular/core';
import { DetailService } from '../../services/detail-service/detail-service';
import { Report } from '../../model/report';
import { MatCard, MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { NgIf } from '@angular/common';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-detail',
  imports: [
    MatCardModule,
    MatButtonModule,
    CommonModule,
    MatIconModule,
    MatChipsModule,
    NgIf,
    NgFor,
    
    
  ],
  templateUrl: './detail.html',
  styleUrl: './detail.scss',
})
export class Detail {
  report = signal<Report | null>(null);
  currentImg = signal(0);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private detailServ: DetailService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) this.router.navigate(['not-found']);

    this.detailServ
      .get(id)
      .then((rep) => this.report.set(rep))
      .catch(() => this.router.navigate(['not-found']));
  }

  nextImg() {
    const imgs = this.report()?.images;
    if (!imgs) return;
    this.currentImg.update((value) => (value + 1) % imgs.length);
  }

  prevImg() {
    const imgs = this.report()?.images;
    if (!imgs) return;
    this.currentImg.update((value) => (value - 1 + imgs.length) % imgs.length);
  }

  focusonMap() {
    const lat = this.report()!.lat;
    const lng = this.report()!.lng;
    window.open(
      '/map', '_blank'
    );
  }
}
