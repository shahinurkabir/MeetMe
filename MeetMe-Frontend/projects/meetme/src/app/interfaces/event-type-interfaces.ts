export interface IEventType {
    id: string,
    name: string,
    description: string,
    location: string,
    eventColor: string,
    slug: string,
    activeYN: boolean,
    ownerId: string
    availabilityId: string,
    forwardDuration?: number,
    dateForwardKind: string,
    dateFrom?: Date,
    dateTo?: Date,
    duration: number,
    bufferTimeBefore: number,
    bufferTimeAfter: number,
    timeZone: string
    
}

export interface IEventTypeQuestionList {
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
    name: string;
    currentTime: string;
}

export interface IEventAvailabilityDetailItemDto {

    /// <summary>
    /// D:Date
    /// W:Weekday
    /// </summary>
    dayType: string,
    value: string,
    stepId: number,
    from: number,
    to: number
}
