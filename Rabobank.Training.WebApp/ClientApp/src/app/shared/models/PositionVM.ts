import { MandateVM } from "./MandateVM";

export interface PositionVM {
  code: string;
  name: string;
  value: number;
  mandates: MandateVM[];
}
