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
    public bool IsInit = false;

    public Gene this[uint index]
    {
        get => this.Genes[index];
        set
        {
            this.Genes[index] = value;
            this.Fitness = null;
        }
    }

    public Gene this[string name]
    {
        get => Array.Find(this.Genes, gene => gene.MetaGene.Name == name);
    }

    public void Init()
    {
        this.Context = this.MetaGenome.Init(this);
        this.IsInit = true;
    }

    public void Update()
    {
        this.MetaGenome.Update(this);
    }

    public void Evaluate()
    {
        this.Fitness = this.MetaGenome.FitnessFn(this);
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
        return newGenome;
    }

    public int CompareTo(Genome rhs)
    {
        Debug.Assert(this.Fitness.HasValue && rhs.Fitness.HasValue, "Genomes cannot be compared unless their fitness was evaluated");
        return rhs.Fitness.Value.CompareTo(this.Fitness.Value);
    }
}
