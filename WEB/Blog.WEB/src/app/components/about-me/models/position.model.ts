import { Period } from "./period.model";

export class Position {
    period: Period;
    companyName: string;
    position: string;
    city: string;
    country?: string;
    description: string;
}