export interface IEventTimeAvailability{
    date:string
    slots:ITimeSlot[]
}
export interface ITimeSlot {
    startAt: string
    startAtTimeOnly: string
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