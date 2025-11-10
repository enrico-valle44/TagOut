// category-style.ts
export interface CategoryStyle {
  bg: string;
  text: string;
  icon: string;
  marker: string;
}

export type ValidCategory = 'selective' | 'Avvistamenti' | 'Sport' | 'Cultura' | 'Politica';

export interface CategoryStyles {
  selective: CategoryStyle;
  Avvistamenti: CategoryStyle;
  Sport: CategoryStyle;
  Cultura: CategoryStyle;
  Politica: CategoryStyle;
  default: CategoryStyle;
}

export const CATEGORY_STYLES: CategoryStyles = {
  selective: { bg: '#ffebee', text: '#d32f2f', icon: 'delete', marker: 'red' },
  Avvistamenti: { bg: '#fff8e1', text: '#ff8f00', icon: 'visibility', marker: 'orange' },
  Sport: { bg: '#e8f5e9', text: '#2e7d32', icon: 'sports', marker: '#2e7d32' },
  Cultura: { bg: '#fff3e0', text: '#e65100', icon: 'museum', marker: '#e65100' },
  Politica: { bg: '#fce4ec', text: '#c2185b', icon: 'gavel', marker: '#c2185b' },
  default: { bg: '#e3f2fd', text: '#1976d2', icon: 'location_on', marker: 'blue' }
} as const;

export const GEOJSON_MARKER_OPTIONS = {
  radius: 8,
  color: '#000',
  weight: 1,
  opacity: 1,
  fillOpacity: 0.8,
} as const;