import { Component, EventEmitter, Output } from '@angular/core';
import * as L from 'leaflet';
import { LocationService } from './../../services/location-service/location-service';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-map-picker',
  imports: [MatButtonModule],
  templateUrl: './map-picker.html',
  styleUrl: './map-picker.scss',
})
export class MapPicker {
  private map!: L.Map;
  private marker!: L.Marker;
  public showNoClickMsg = false;
  hasClicked = false;
  selectedPos = {
    lat: 0,
    lng: 0,
  };

  @Output() positionSelected = new EventEmitter<{ lat: number; lng: number }>();

  constructor(private locationServ: LocationService) {}

  ngAfterViewInit() {
    this.setUpMap();
  }

  async setUpMap() {
    const pos = await this.locationServ.getPosition();
    this.map = L.map('map').setView(
      [pos.coords.latitude, pos.coords.longitude],
      13
    );
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
    }).addTo(this.map);

    this.map.on('click', (e: L.LeafletMouseEvent) => {
      this.hasClicked = true;
      this.selectedPos = {
        lat: e.latlng.lat,
        lng: e.latlng.lng,
      };
      if (this.marker) {
        this.marker.setLatLng([this.selectedPos.lat, this.selectedPos.lng]);
      } else {
        this.marker = L.marker([
          this.selectedPos.lat,
          this.selectedPos.lng,
        ]).addTo(this.map);
      }
      //this.positionSelected.emit({ lat, lng });
    });

    // this.map.on('click', (e: L.LeafletMouseEvent) => {
    //   this.selectedPos = { lat: e.latlng.lat, lng: e.latlng.lng };
    //   console.log(this.selectedPos);
    //   if (this.marker) {
    //     this.marker.setLatLng([e.latlng.lat, e.latlng.lng]);
    //   } else {
    //     this.marker = L.marker([e.latlng.lat, e.latlng.lng]).addTo(this.map);
    //   }
    // });
  }

  async confirm() {
    if (!this.hasClicked) {
      // Mostra messaggio temporaneo invece dell'alert
      this.showNoClickMsg = true;
      setTimeout(() => (this.showNoClickMsg = false), 3000);

      // Facoltativo: puoi anche prendere la posizione attuale come fallback
      // const pos = await this.locationServ.getPosition();
      // this.selectedPos = { lat: pos.coords.latitude, lng: pos.coords.longitude };

      return;
    }

    this.positionSelected.emit(this.selectedPos);
  }
  // confirm() {
  //   if (!this.selectedPos.lat || !this.selectedPos.lng) {
  //     alert('Clicca sulla mappa prima di confermare');
  //     return;
  //   }

  //   this.positionSelected.emit(this.selectedPos);
  // }
}
