export interface IEventTimeAvailability{
    date:string
    slots:ITimeSlot[]
}
export interface ITimeSlot {
    startDateTime: string
    startTime: string
    isDaySecondHalf: boolean
}

