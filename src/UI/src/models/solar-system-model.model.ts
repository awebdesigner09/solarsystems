
export interface SolarSystemModel {
  id: string;
  name: string;
  panelType: 'Monocrystalline' | 'Polycrystalline' | 'Thin-Film';
  capacityKW: number;
  basePrice: number;
  description: string;
  imageUrl: string;
}
