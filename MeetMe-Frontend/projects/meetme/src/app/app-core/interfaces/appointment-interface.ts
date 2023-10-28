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
    note?: string,
    questionnaireContent?: string
}

export interface ICancelAppointmentCommand {
    id: string
    cancellationReason:string|null
}

export interface IAppointmentDetailsDto {
        id: string;
        eventTypeId: string;
        eventTypeTitle: string | null;
        eventTypeDescription: string | null;
        eventTypeLocation: string | null;
        eventTypeDuration: number;
        eventTypeColor: string;
        eventTypeTimeZone: string;
        eventOwnerId: string;
        eventOwnerName: string;
        inviteeName: string;
        inviteeEmail: string;
        inviteeTimeZone: string;
        guestEmails: string | null;
        startTimeUTC: string;
        endTimeUTC: string;
        appointmentDateTime: string;
        note: string | null;
        status: string;
        dateCreated: string;
        dateCancelled: string | null;
        cancellationReason: string | null;
        isExpanded: boolean;
}
export interface IAppointmentQuestionaireItemDto {
    questionId: string;
    questionName: string;
    answer: string;
    isMultipleChoice: boolean;
}

