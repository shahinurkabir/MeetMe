import { IAvailabilityDetails } from "./availability-interfaces";

export interface ICloneAvailabilityCommand {
    id:string;
}
export interface ICreateAvailabilityCommand {
    name:string;
    timeZone:string;
}
export interface IDeleteAvailabilityCommand {
    id:string;
}

export interface ISetDefaultAvailabilityCommand {
    id:string;
}

export interface IEditAvailabilityCommand {
    id: string ;
    name: string ;
    timeZone: string ;
    details: IAvailabilityDetails[] ;
}
export interface IEditAvailabilityNameCommand {
    id:string;
    name:string;
}
export interface ISetDefaultAvailabilityNameCommand {
    id: string;
}