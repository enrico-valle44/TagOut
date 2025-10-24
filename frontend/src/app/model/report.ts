export interface Report {
  id?: number;
  description?: string;
  title: string;
  date: string;
  categories: string[];
  images: string[];
  lat: number;
  lng: number;
  distance?: number;
}
