export  const date = {
    isDayPast: function (date: Date) {
        let currentDateTime = new Date();
        let currentDate = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate())
        return currentDate.getTime() > date.getTime();
    },
    isDayCurrent: function (date: Date) {
        let currentDateTime = new Date();
        let currentDate = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate())
        return currentDate.getTime() == date.getTime();
    },
    isMonthCurrent: function (date: Date) {
        let currentDateTime = new Date();
        return currentDateTime.getFullYear() == date.getFullYear()
            && currentDateTime.getMonth() == date.getMonth()
    }
}