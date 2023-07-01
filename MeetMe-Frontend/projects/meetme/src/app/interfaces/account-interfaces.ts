export interface IUserDetail {
    id: string,
    userName: string,
    timeZone: string,
}

export interface IUpdateAccountSettingsResponse {
    result: boolean,
    newToken:ITokenResponse
}

export interface ITokenResponse {
    token: string,
    expiredAt:number
}