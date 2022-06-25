using System;
using System.Linq;
using System.Diagnostics;

public class Genome : ICloneable, IComparable<Genome>
{
    public MetaGenome MetaGenome;
    public Gene[] Genes;
    public float? Fitness = null;

    public Gene this[uint index]
    {
        get => this.Genes[index];
        set
        {
            this.Genes[index] = value;
            this.Fitness = null;
        }
    }

    public void Evaluate()
    {
        this.Fitness = this.MetaGenome.FitnessFn(Array.ConvertAll(this.Genes, gene => gene.Value));
    }

    public override string ToString()
    {
        return $"({String.Join(", ", Array.ConvertAll(this.Genes, gene => gene.ToString()))}), fitness: {this.Fitness:0.#####}";
    }

    public object Clone()
    {
        return new Genome
        {
            MetaGenome = this.MetaGenome,
            Genes = Array.ConvertAll(this.Genes, gene => (Gene)gene.Clone()),
            Fitness = this.Fitness
        };
    }

    public int CompareTo(Genome rhs)
    {
        Debug.Assert(this.Fitness.HasValue && rhs.Fitness.HasValue, "Genomes cannot be compared unless their fitness was evaluted");
        return rhs.Fitness.Value.CompareTo(this.Fitness.Value);
    }
}
