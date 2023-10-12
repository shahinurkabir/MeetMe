using MeetMe.Core.Persistence.Entities;

namespace DataProvider.InMemoryData;

public class InMemoryDatabase
{
    public InMemoryDatabase()
    {
        UserData.Add(new User
        {
            Id = Guid.NewGuid(),
            UserName = "Test User",
            UserId = "admin",
            Password = "123",
            BaseURI = "shahinur-kabir",
            TimeZone = "asia/dhaka",
            WelcomeText = "Please do book an appointment to talk about something."
        });
    }

    public List<Availability> AvailabilityData { get; } = new List<Availability>();
    public List<Appointment> AppointmentData { get; } = new List<Appointment>();
    public List<EventTypeQuestion> EventQuestionData { get; } = new List<EventTypeQuestion>();
    public List<EventType> EventTypeData { get; } = new List<EventType>();
    public List<User> UserData { get; } = new List<User>();

}



