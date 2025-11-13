export interface Report {
  id?: number;
  description?: string;
  title: string;
  dateReport: string ;
  categories: string[];
  images: string[];
  lat: number;
  lng: number;
  distance?: number;
}
