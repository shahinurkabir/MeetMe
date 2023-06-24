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

  export interface ITimeIntervalsInMonth {
    day: number,
    weekDay: string,
    dateString: string,
    isOverride: boolean;
    intervals: ITimeInterval[],
    isPastDate: boolean
  }