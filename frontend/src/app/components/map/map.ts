import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataService } from '../../services/data-service/data-service';
import * as L from 'leaflet';
import { GeoJsonObject } from 'geojson';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { RouterLink } from '@angular/router';
import { Properties } from '../../model/properties';
import { Router } from '@angular/router';
import {
  CATEGORY_STYLES,
  GEOJSON_MARKER_OPTIONS,
  CategoryStyle,
  ValidCategory,
} from '../../model/category-style';
import { MatChipsModule } from '@angular/material/chips';
import { LocationService } from './../../services/location-service/location-service';

@Component({
  selector: 'app-map',
  imports: [
    CommonModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    RouterLink,
    MatChipsModule,
  ],
  templateUrl: './map.html',
  styleUrl: './map.scss',
})
export class Map {
  private map: L.Map | undefined;
  public categoryNames: string[] = [];

  constructor(
    private dataServ: DataService,
    private router: Router,
    private locationServ: LocationService
  ) {}

  ngAfterViewInit() {
    this.setupMap();
  }

  async setupMap() {
    const pos = await this.locationServ.getPosition();
    this.map = L.map('map');
    this.map.setView([pos.coords.latitude, pos.coords.longitude], 13);

    const tileLayer = L.tileLayer(
      'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
      {
        maxZoom: 19,
        attribution:
          '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      }
    );
    tileLayer.addTo(this.map);

    const reports = await this.dataServ.getReportsGeoJson();
    const geojsonLayer = L.geoJSON(reports as GeoJsonObject, {
      pointToLayer: this.myPointToLayer.bind(this),
      onEachFeature: this.myOnEachFeature.bind(this),
    });
    geojsonLayer.addTo(this.map);
  }

  private getCategoryStyle(category: string | undefined): CategoryStyle {
    if (category && CATEGORY_STYLES.hasOwnProperty(category)) {
      return CATEGORY_STYLES[category as ValidCategory];
    }
    return CATEGORY_STYLES.default;
  }

  private getPrimaryCategory(categories: string[] | undefined): string {
    return categories?.[0] || 'default';
  }

  myPointToLayer(point: any, latLng: L.LatLng) {
    const primaryCategory = this.getPrimaryCategory(
      point?.properties?.categories
    );
    const categoryStyle = this.getCategoryStyle(primaryCategory);

    const geojsonMarkerOptions = {
      ...GEOJSON_MARKER_OPTIONS,
      fillColor: categoryStyle.marker,
    };

    return L.circleMarker(latLng, geojsonMarkerOptions);
  }

  myOnEachFeature(point: any, layer: L.Layer) {
    if (point.properties && point.properties.title) {
      const popupContent = this.createSimplePopupContent(point.properties);

      layer.bindPopup(popupContent, {
        maxWidth: 300,
        minWidth: 250,
        closeOnClick: false,
      });

      layer.on('click', (e: L.LeafletEvent) => {
        this.router.navigateByUrl(`/detail/${point.properties.id}`);
      });

      layer.on('popupopen', () => {
        this.attachPopupClickHandler(layer, point.properties.id);
      });

      layer.on('mouseover', () => {
        layer.openPopup();
      });
    }
  }

  async onChipClick(category: string) {
    if (this.map) {
      this.map.remove();
    }
    const pos = await this.locationServ.getPosition();
    this.map = L.map('map');
    this.map.setView([pos.coords.latitude, pos.coords.longitude], 13);

    const tileLayer = L.tileLayer(
      'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
      {
        maxZoom: 19,
        attribution:
          '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      }
    );
    tileLayer.addTo(this.map);

    const reports = await this.dataServ.getReportsGeoJsonByCategory(category);
    const geojsonLayer = L.geoJSON(reports as GeoJsonObject, {
      pointToLayer: this.myPointToLayer.bind(this),
      onEachFeature: this.myOnEachFeature.bind(this),
    });
    geojsonLayer.addTo(this.map);
  }

  private createSimplePopupContent(properties: Properties): string {
    const imageUrl =
      properties.images && properties.images.length > 0
        ? properties.images[0]
        : 'https://via.placeholder.com/280x200/4A90E2/FFFFFF?text=Graffiti+Image';

    return `
      <div class="simple-popup" style="
        cursor: pointer;
        border-radius: 12px;
        overflow: hidden;
        width: 280px;
        transition: all 0.3s ease;
      ">
        <img src="${imageUrl}" 
             alt="${properties.title}"
             style="
               width: 100%;
               height: 200px;
               object-fit: cover;
               display: block;
             "
             onerror="this.src='https://via.placeholder.com/280x200/4A90E2/FFFFFF?text=Image+Not+Found'">
      </div>
    `;
  }

  private attachPopupClickHandler(layer: L.Layer, propertyId: string) {
    setTimeout(() => {
      const popup = layer.getPopup();
      if (!popup) return;

      const popupElement = popup.getElement();
      if (!popupElement) return;

      const popupContentWrapper = popupElement.querySelector(
        '.leaflet-popup-content-wrapper'
      );
      if (popupContentWrapper) {
        const htmlElement = popupContentWrapper as HTMLElement;
        htmlElement.style.cursor = 'pointer';

        htmlElement.addEventListener('click', (event: Event) => {
          event.stopPropagation();
          this.router.navigateByUrl(`/detail/${propertyId}`);
        });
      }

      const image = popupElement.querySelector('img');
      if (image) {
        image.style.cursor = 'pointer';
      }
    }, 50);
  }
}
