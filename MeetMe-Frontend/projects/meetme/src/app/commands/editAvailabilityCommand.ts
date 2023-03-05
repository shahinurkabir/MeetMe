import { IAvailabilityDetails } from "../models/IAvailabilityDetails";

export class EditAvailabilityCommand {
    id: string = "";
    name: string = "";
    timeZoneId: number = 0;
    details: IAvailabilityDetails[] = []
}