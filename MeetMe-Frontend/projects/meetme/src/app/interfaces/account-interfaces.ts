export interface IAccountProfileInfo {
    id: string,
    userName: string,
    timeZone: string,
    baseURI: string,
    welcomeText?: string
}

export interface IUpdateAccountSettingsResponse {
    result: boolean,
    newToken:ITokenResponse
}

export interface ITokenResponse {
    token: string,
    expiredAt:number
}

