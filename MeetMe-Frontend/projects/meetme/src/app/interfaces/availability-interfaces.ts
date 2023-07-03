
export interface IAvailability {
    id: string,
    name: string,
    ownerId: string,
    timeZone: string,
    isDefault:boolean,
    details: Array<IAvailabilityDetails>,
}

export interface IAvailabilityDetails {

    dayType: string, //w:weekday,d:date
    value: string,
    from: number,
    to: number,
    stepId:number
}