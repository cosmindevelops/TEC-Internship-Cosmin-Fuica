namespace ApiApp.Common.Dto;

public class CreateUpdatePersonDetailsDto
{
    public int PersonId { get; set; }
    public DateTime BirthDay { get; set; }
    public string PersonCity { get; set; }
}