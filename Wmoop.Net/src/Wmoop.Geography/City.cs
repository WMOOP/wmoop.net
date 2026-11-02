namespace Wmoop.Geography;

public partial class City
{
    public override void Accept(IAreaVisitor visitor) => visitor.VisitCity(this);
}
