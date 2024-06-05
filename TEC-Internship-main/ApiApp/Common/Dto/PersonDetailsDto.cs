namespace ApiApp.Common.Dto;

public class PersonDetailsDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public DateTime BirthDay { get; set; }
    public string PersonCity { get; set; }
    public PersonDto Person { get; set; }
}