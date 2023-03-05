import { IAvailabilityDetails } from "./IAvailabilityDetails"

export interface IAvailability {
    id: string,
    name: string,
    ownerId: string,
    timeZoneId: number
    details: Array<IAvailabilityDetails>
}
