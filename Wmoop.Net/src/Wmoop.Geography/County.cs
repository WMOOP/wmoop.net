namespace Wmoop.Geography;

public partial class County
{
    public override void Accept(IAreaVisitor visitor) => visitor.VisitCounty(this);
}
