import { Component, ViewChild, TemplateRef } from '@angular/core';
import { DataService } from '../../services/data-service/data-service';
import * as L from 'leaflet';
import { GeoJsonObject } from 'geojson';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { RouterLink } from '@angular/router';
import { Feature } from '../../model/feature'; 
import { Properties } from '../../model/properties';
import { CommonModule } from '@angular/common'; 
import { Router } from '@angular/router';
import { NgIf, NgFor } from '@angular/common';

import {
  CATEGORY_STYLES, 
  GEOJSON_MARKER_OPTIONS, 
  CategoryStyle,
  ValidCategory 
} from '../../model/category-style';

@Component({
  selector: 'app-map',
  imports: [MatButtonModule, MatDividerModule, MatIconModule, RouterLink, NgIf, NgFor, CommonModule],
  templateUrl: './map.html',
  styleUrl: './map.scss',
})
export class Map {
  private map: L.Map | undefined;
  @ViewChild('popupTemplate') popupTemplate!: TemplateRef<any>;

  constructor(
    private dataServ: DataService,
    private router: Router
  ) {}

  ngAfterViewInit() {
    this.setupMap();
  }

  async setupMap() {
    this.map = L.map('map');
    this.map.setView([44.40614435613236, 8.949400422559357], 13);

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
    const primaryCategory = this.getPrimaryCategory(point?.properties?.categories);
    const categoryStyle = this.getCategoryStyle(primaryCategory);

    const geojsonMarkerOptions = {
      ...GEOJSON_MARKER_OPTIONS,
      fillColor: categoryStyle.marker,
    };

    return L.circleMarker(latLng, geojsonMarkerOptions);
  }

  myOnEachFeature(point: any, layer: L.Layer) {
    if (point.properties && point.properties.title) {
      const primaryCategory = this.getPrimaryCategory(point.properties.categories);
      const colors = this.getCategoryStyle(primaryCategory);
      
      const popupContent = this.createPopupContent(point.properties, colors);
      
      layer.bindPopup(popupContent, {
        maxWidth: 300,
        minWidth: 250,
        closeOnClick: false
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

  private createPopupContent(properties: Properties, colors: CategoryStyle): string {
    const imageUrl = properties.images && properties.images.length > 0 
      ? properties.images[0] 
      : 'https://via.placeholder.com/280x140/4A90E2/FFFFFF?text=Graffiti+Image';

    return `
      <div class="custom-popup" style="background: ${colors.bg}; border-color: ${colors.text}">
        <div class="popup-image-section">
          <img src="${imageUrl}" alt="${properties.title}" class="popup-image">
        </div>
        <div class="popup-content-section">
          <h3 class="popup-title">${properties.title}</h3>
          ${properties.description ? `
            <p class="popup-description">
              ${properties.description.length > 100 ? properties.description.substring(0, 100) + '...' : properties.description}
            </p>
          ` : ''}
          ${properties.categories && properties.categories.length > 0 ? `
            <div class="popup-categories">
              ${properties.categories.slice(0, 3).map(category => `
                <span class="category-chip" style="background: ${colors.text}22; color: ${colors.text}; border-color: ${colors.text}44">
                  ${category}
                </span>
              `).join('')}
            </div>
          ` : ''}
          ${properties.date ? `
            <div class="popup-date">
               ${new Date(properties.date).toLocaleDateString('it-IT')}
            </div>
          ` : ''}
          <div class="popup-hint">
             Clicca per scoprire di più su "${properties.title}" →
          </div>
        </div>
      </div>
    `;
  }

  private attachPopupClickHandler(layer: L.Layer, propertyId: string) {
    setTimeout(() => {
      const popup = layer.getPopup();
      if (!popup) return;

      const popupElement = popup.getElement();
      if (!popupElement) return;

      const popupContentWrapper = popupElement.querySelector('.leaflet-popup-content-wrapper');
      if (popupContentWrapper) {
        const htmlElement = popupContentWrapper as HTMLElement;
        htmlElement.style.cursor = 'pointer';
        
        htmlElement.addEventListener('click', (event: Event) => {
          event.stopPropagation();
          this.router.navigateByUrl(`/detail/${propertyId}`);
        });
      }
    }, 50);
  }
}