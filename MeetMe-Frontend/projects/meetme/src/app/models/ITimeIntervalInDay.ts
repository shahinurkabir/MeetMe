import { ITimeInterval } from "./ITimeInterval"

export interface ITimeIntervalInDay {
    day: string,
    isAvailable: boolean
    intervals: ITimeInterval[]
  }