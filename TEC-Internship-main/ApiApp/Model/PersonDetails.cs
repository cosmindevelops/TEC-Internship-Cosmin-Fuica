namespace Internship.Model;

public class PersonDetails
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public DateTime BirthDay { get; set; }
    public string PersonCity { get; set; }
    public Person Person { get; set; }
}