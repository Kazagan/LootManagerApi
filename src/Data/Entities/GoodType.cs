namespace Data.Entities;

public class GoodType : Entity
{
    public GoodType()
    {
        Name = "";
    }
    public string Name { get; set; }
    public virtual bool IsInvalid()
    {
        throw new NotImplementedException();
    }
}