import { EventAvailabilityDetailItemDto, IEventTypeQuestion } from "./event-type-interfaces";

export interface ICreateEventTypeCommand {
    name: string;
    description?: string;
    location?: string;
    slug: string;
    eventColor: string;
    timeZoneName: string
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
    timeZone: string
    availabilityDetails: Array<EventAvailabilityDetailItemDto>
}



export interface IUpdateEventCommand {
    id: string,
    name: string,
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