namespace MeetMe.Core.Dtos
{
    public class AppointmentQuestionaireItemDto
    {
        public Guid questionId { get; set; }
        public string? questionName { get; set; }
        public string answer { get; set; } = null!;
        public bool isMultipleChoice { get; set; }

    }
}
