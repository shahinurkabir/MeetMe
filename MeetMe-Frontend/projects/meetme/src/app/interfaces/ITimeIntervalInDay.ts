export interface ITimeIntervalInDay {
    day: string,
    isAvailable:boolean,
    intervals: ITimeInterval[]
  }
  export interface ITimeInterval {
    startTime: string,
    endTime: string
    startTimeInMinute: number,
    endTimeInMinute: number,
    errorMessage?: string
  }