namespace Wmoop.Geography;

public partial class Country
{
    public override void Accept(IAreaVisitor visitor) => visitor.VisitCountry(this);
}
