public class MetaGene
{
    public string Name;
    public (float, float) Bounds;

    public Gene MakeGene(float value)
    {
        return new Gene
        {
            MetaGene = this,
            Value = value
        };
    }

    public Gene MakeRandomGene()
    {
        return this.MakeGene(RNG.Range(this.Bounds.Item1, this.Bounds.Item2));
    }
}
