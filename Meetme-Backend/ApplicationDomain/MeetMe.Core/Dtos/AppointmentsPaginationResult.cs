using System.Security.Cryptography.X509Certificates;

namespace MeetMe.Core.Dtos
{
    public class AppointmentsPaginationResult {

        public AppointmentsPaginationResult()
        {
            Result = new List<AppointmentsByDate>();
        }
        public PaginationInfo PaginationInfo { get; set; } = null!;
        public List<AppointmentsByDate> Result { get; set; } = null!;
    }

    public class AppointmentsByDate {
        public string Date { get; set; } = null!;
        public List<AppointmentDetailsDto> Appointments { get; set; } = null!;
    }

}
