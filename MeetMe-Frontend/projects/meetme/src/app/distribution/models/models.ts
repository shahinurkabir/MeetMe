export class ContactModel {
    nameModel?:NameModel=undefined
    addressModel?:AddressModel=undefined
}
export class NameModel {
    firstName:string=""
    lastName:string=""
}
export class AddressModel {
    city:string=""
    state:string=""
    zip:string=""
}

