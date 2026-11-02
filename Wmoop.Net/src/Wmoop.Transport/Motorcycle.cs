namespace Wmoop.Transport;

public partial class Motorcycle
{
    public override string Classification => "motorcycle";

    public override string Describe() => base.Describe() + $" Engine: {EngineDisplacement}.";
}
