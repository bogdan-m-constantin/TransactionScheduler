namespace TransactionScheduling.Project.Domain.Objects
{
    public class BaseEntity(int id)
    {
        public int Id { get; set; } = id;
    }
    public class Client(int id, string firstName, string lastName, string personalCode, string idNumber, DateTime dateOfBirth, double ammountOfPoints) : BaseEntity(id)
    {
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string PersonalCode { get; set; } = personalCode;
        public string IdNumber { get; set; } = idNumber;
        public DateTime DateOfBirth { get; set; } = dateOfBirth;
        public double AmmountOfPoints { get; set; } = ammountOfPoints;

    }
    
}
