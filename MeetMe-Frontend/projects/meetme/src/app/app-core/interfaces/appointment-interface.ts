export interface ICreateAppointmentCommand {
    eventTypeId: string
    inviteeName: string
    inviteeEmail: string
    inviteeTimeZone: string
    startTime: string
    meetingDuration: number
    guestEmails?:string
    note?: string
}

export interface ICancelAppointmentCommand {
    id: string
    cancellationReason:string
}