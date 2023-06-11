import { IAvailability } from "./IAvailability";
import { IAvailabilityDetails } from "./IAvailabilityDetails";

export interface EventType {
    id: string,
    name: string,
    description: string,
    location: string,
    eventColor: string,
    slug: string,
    activeYN: boolean,
    ownerId: string
    availabilityId:string,
    forwardDuration?:number,
    dateForwardKind: string,
    dateFrom?: Date,
    dateTo?: Date,
    duration: number,
    bufferTimeBefore: number,
    bufferTimeAfter: number,
    timeZoneId: number
    
}

export interface ICreateEventTypeCommand {
    name: string;
    description?: string;
    location?: string;
    slug: string;
    eventColor: string;
    timeZoneName:string
    activeYN: boolean;
}

export interface IUpdateEventAvailabilityCommand {
    id: string,
    dateForwardKind: string,
    forwardDuration?: number,
    dateFrom?: Date,
    dateTo?: Date,
    duration: number,
    bufferTimeBefore: number,
    bufferTimeAfter: number,
    availabilityId?: string,
    timeZoneId: number
    availabilityDetails: Array<EventAvailabilityDetailItemDto>
}

export interface EventAvailabilityDetailItemDto {

    /// <summary>
    /// D:Date
    /// W:Weekday
    /// </summary>
    dayType: string,
    value?: string,
    stepId: number,
    from: number,
    to: number
}

export interface IUpdateEventCommand {
    id: string,
    name: string,
    description?: string,
    location?: string,
    slug: string,
    eventColor: string
}

export interface IEventTypeQuestionList {
    eventTypeId: string,
    questions: IEventTypeQuestion[]
}

export interface IUpdateEventQuestionCommand {
    eventTypeId: string,
    questions: IEventTypeQuestion[]
}
export interface IEventTypeQuestion {
    id?: string,
    name: string,
    eventTypeId?: string,
    questionType: string,
    options: string,
    otherOptionYN: boolean,
    activeYN: boolean,
    requiredYN: boolean,
    displayOrder: number
}

export interface TimeZoneData {
    id: number;
    name: string;
    countryName: string;
    countryCode: string;
    cffset: string;
    offsetMinutes: number;
  }
  