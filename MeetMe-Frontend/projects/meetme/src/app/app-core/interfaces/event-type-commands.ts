import { IEventAvailabilityDetailItemDto, IEventTypeQuestion } from "./event-type-interfaces";

export interface ICreateEventTypeCommand {
    name: string;
    description?: string;
    duration: number,
    location?: string;
    slug: string;
    eventColor: string;
    timeZoneName: string
    activeYN: boolean;
}

export interface IUpdateEventAvailabilityCommand {
    id: string,
    dateForwardKind: string,
    forwardDurationInDays?: number,
    dateFrom?: Date,
    dateTo?: Date,
    bufferTimeBefore: number,
    bufferTimeAfter: number,
    availabilityId?: string,
    timeZone: string
    availabilityDetails: Array<IEventAvailabilityDetailItemDto>
}



export interface IUpdateEventCommand {
    id: string,
    name: string,
    duration: number,
    description?: string,
    location?: string,
    slug: string,
    eventColor: string
}


export interface IUpdateEventQuestionCommand {
    eventTypeId: string,
    questions: IEventTypeQuestion[]
}


export interface IToggleEventTypeStatusCommand {
    id: string,
}