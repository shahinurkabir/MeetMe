export interface IEventTimeAvailability{
    date:string
    slots:ITimeSlot[]
}
export interface ITimeSlot {
    startAt: string
    startAtTimeOnly: string
}