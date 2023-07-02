export interface IUpdateUserLinkCommand {
    baseURI: string
};

export interface IUpdateProfileCommand {
 userName: string,
 timeZone: string,
 welcomeText?: string
}