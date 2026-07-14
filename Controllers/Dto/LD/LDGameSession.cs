
public class LDGameSession
{
    public int Id { get; set; }

    private DateTime _createdAt;

    public DateTime Created_At
    {
        get => _createdAt;
        set => _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    public int Nbr_of_laps { get; set; }
    public int Map_Id { get; set; }

}

