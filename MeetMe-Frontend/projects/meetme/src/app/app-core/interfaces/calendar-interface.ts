export interface IEventTimeAvailability{
    date:string
    slots:ITimeSlot[]
}
export interface ITimeSlot {
    startDateTime: string
    startTime: string
    isDaySecondHalf: boolean
}

export interface ICreateAppointmentCommand {
    eventTypeId: string
    inviteeName: string
    inviteeEmail: string
    startTime: string
    meetingDuration: number
    guestEmails?:string
    note?: string
}