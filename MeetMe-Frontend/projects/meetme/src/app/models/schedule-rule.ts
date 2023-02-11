export interface ScheduleRule {
    id:string
    name:string
    ownerId:string,
    timeZoneId:number
    ruleAttributes:ScheduleRuleAttribute[]
}

export interface ScheduleRuleAttribute {
    id:string
    ruleId:string
    type:string
    day:string
    date?:Date
    stepId:number
    from:number
    to:number
}
