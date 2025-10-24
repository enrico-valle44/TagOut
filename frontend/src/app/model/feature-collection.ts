import { Feature } from "./feature";

export interface FeatureCollection {
  type: string;
  features: Feature[];
}
