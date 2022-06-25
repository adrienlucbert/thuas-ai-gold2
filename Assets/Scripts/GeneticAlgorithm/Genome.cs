using System;
using System.Linq;
using System.Collections;
using System.Diagnostics;

public class Genome : ICloneable, IComparable<Genome>
{
    public MetaGenome MetaGenome;
    public Gene[] Genes;
    public float? Fitness = null;
    public object Context;

    public Gene this[uint index]
    {
        get => this.Genes[index];
        set
        {
            this.Genes[index] = value;
            this.Fitness = null;
        }
    }

    public void Init()
    {
        this.Context = this.MetaGenome.Init(this);
    }

    public void Update()
    {
        this.MetaGenome.Update(this, this.Context);
    }

    public void Evaluate()
    {
        this.Fitness = this.MetaGenome.FitnessFn(this, this.Context);
    }

    public override string ToString()
    {
        return $"({String.Join(", ", Array.ConvertAll(this.Genes, gene => gene.ToString()))}), fitness: {this.Fitness:0.#####}";
    }

    public object Clone()
    {
        Genome newGenome = new Genome
        {
            MetaGenome = this.MetaGenome,
            Genes = Array.ConvertAll(this.Genes, gene => (Gene)gene.Clone()),
            Fitness = this.Fitness
        };
        newGenome.Update();
        return newGenome;
    }

    public int CompareTo(Genome rhs)
    {
        Debug.Assert(this.Fitness.HasValue && rhs.Fitness.HasValue, "Genomes cannot be compared unless their fitness was evaluated");
        return rhs.Fitness.Value.CompareTo(this.Fitness.Value);
    }
}
