using System;

public class Gene : ICloneable
{
    public MetaGene MetaGene;
    public float Value;

    public override string ToString()
    {
        return $"{this.MetaGene.Name}: {this.Value:0.#####}";
    }

    public object Clone()
    {
        return new Gene
        {
            MetaGene = this.MetaGene,
            Value = this.Value
        };
    }
}
