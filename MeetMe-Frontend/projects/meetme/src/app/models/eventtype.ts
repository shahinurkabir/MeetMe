export interface EventType {
    id: string,
    name: string,
    description: string,
    location: string,
    eventColor: string,
    slug: string,
    activeYN: boolean,
    ownerId: string
}

export interface CreateEventTypeCommand {
    name: string;
    description?: string;
    location?: string;
    slug: string;
    eventColor: string;
    timeZoneName:string
    activeYN: boolean;
}

export interface EventTypeAvailability {
    id: string,
    dateForwardKind: string,
    forwardDuration?: number,
    dateFrom?: Date,
    dateTo?: Date,
    duration: number,
    bufferTimeBefore: number,
    bufferTimeAfter: number,
    baseScheduleId?: string,
    timeZoneId: number
    availabilityDetails: Array<EventAvailabilityDetailItem>
}

export interface EventAvailabilityDetailItem {

    /// <summary>
    /// D:Date
    /// W:Weekday
    /// </summary>
    type: string,
    day?: string,
    date?: string,
    stepId: number,
    from: number,
    to: number
}

export interface UpdateEventCommand {
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
  