import { Geometry } from './geometry';
import { Properties } from './properties';

export interface Feature {
  type: string;
  properties: Properties;
  geometry: Geometry;
  id: number;
}
