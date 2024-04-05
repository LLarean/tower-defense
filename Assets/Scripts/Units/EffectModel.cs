using Builds;

public class EffectModel
{
    public ElementalType ElementalType;
    public float Duration;

    public EffectModel(ElementalType elementalType, float duration)
    {
        ElementalType = elementalType;
        Duration = duration;
    }
}