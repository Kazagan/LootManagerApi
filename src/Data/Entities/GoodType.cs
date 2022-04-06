namespace Data.Entities;

public class GoodType : Entity
{
    public GoodType()
    {
        Name = "";
    }
    public string Name { get; set; }
    public override bool IsInvalid()
    {
        throw new NotImplementedException();
    }
}