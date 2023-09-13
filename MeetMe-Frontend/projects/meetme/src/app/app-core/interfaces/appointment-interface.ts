export interface IAppointmentDto {

}


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

export interface IAppointmentDetailsDto {
    id: string;
    eventTypeId: string;
    inviteeName: string;
    inviteeEmail: string;
    inviteeTimeZone: string;
    guestEmails: string | null;
    startTime: string;
    endTime: string;
    appointmentDateTime: string;
    note: string | null;
    status: string;
    dateCreated: string;
    dateCancelled: string | null;
    cancellationReason: string | null;
}