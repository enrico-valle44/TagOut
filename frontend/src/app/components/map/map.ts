import { Component } from '@angular/core';
import { DataService } from '../../services/data-service/data-service';
import * as L from 'leaflet';
import { Feature } from '../../model/feature';
import { GeoJsonObject } from 'geojson';
import { MatButtonModule } from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatDividerModule} from '@angular/material/divider';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-map',
  imports: [MatButtonModule, MatDividerModule, MatIconModule,RouterLink],
  templateUrl: './map.html',
  styleUrl: './map.scss',
})
export class Map {
  private map: L.Map | undefined;

  // private dataServ = inject(DataService);
  constructor(private dataServ: DataService) {}

  ngAfterViewInit() {
    this.setupMap();

    // this.testClojure();
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
      pointToLayer: this.myPointToLayer,
      onEachFeature: this.myOnEachFeature,
    });

    geojsonLayer.addTo(this.map);
  }

  myPointToLayer(point: any, latLng: L.LatLng) {
    const geojsonMarkerOptions = {
      radius: 8,
      fillColor: '#ff7800',
      color: '#000',
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8,
    };
    return L.circleMarker(latLng, geojsonMarkerOptions);
  }

  myOnEachFeature(point: any, layer: L.Layer) {
    // const createPopupContent = (props: any) => {
    //   let result = '';
    //   for (const key in props) {
    //     const value = props[key];
    //     result += `<span><strong>${key}:</strong> ${value}</span><br/>`;
    //   }
    //   return result;
    // };

    if (point.properties && point.properties.title) {
      console.log('point properties:', point.properties);
      const content = createPopupContent(point.properties);
      layer.bindPopup(content);
    }
  }

  // createPopupContent(point: any): string {
  //   let result = '';

  //   for (const key in point) {
  //     const value = point[key];
  //     result += `<span><strong>${key}:</strong> ${value}</span><br/>`;
  //   }

  //   return result;
  // }

  // testClojure() {
  //   let functionVariable;
  //   {
  //     let counter = 0;
  //     functionVariable = () => {
  //       counter = counter + 1;
  //       console.log('Counter value:', counter);
  //     };
  //   }
  //   functionVariable();
  //   functionVariable();
  //   functionVariable();

  // }
}

function createPopupContent(properties: any): string {
  // let result = '';
  // for (const key in properties) {
  //   const value = properties[key];
  //   result += `<span><strong>${key}:</strong> ${value}</span><br/>`;
  // }
  // return result;

  // let div = '<div class="popup-content">';
  // for (const key in properties) {
  //   const value = properties[key];
  //   div += `<div class="popup-item"><strong>${key}:</strong> ${value}</div>`;
  // }
  // div += '</div>';
  // return div;

  // const container = document.createElement('div');
  // container.className = 'popup-content';
  // for (const key in properties) {
  //   const value = properties[key];
  //   const itemDiv = document.createElement('div');
  //   itemDiv.className = 'popup-item';

  //   const strong = document.createElement('strong');
  //   strong.textContent = `${key}: `;

  //   itemDiv.appendChild(strong);
  //   itemDiv.appendChild(document.createTextNode(value));

  //   container.appendChild(itemDiv);
  // }

  // return container.outerHTML;

  const container = document.createElement('div');
  container.style.display = 'flex';
  container.className = 'popup-content';

  if (properties.images && properties.images.length > 0) {
    const image = document.createElement('img');
    image.src = properties.images[0];
    image.width = 50;
    image.height = 50;
    image.style.objectFit = 'cover';
    container.appendChild(image);
  }

  const titleDiv = document.createElement('div');
  titleDiv.className = 'popup-title';
  titleDiv.textContent = properties.title;
  container.appendChild(titleDiv);

  return container.outerHTML;
}
