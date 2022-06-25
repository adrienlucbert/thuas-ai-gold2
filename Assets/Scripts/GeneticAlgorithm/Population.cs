using System;

public class Population : ICloneable
{
    public MetaPopulation MetaPopulation;
    public Genome[] Genomes;
    public uint GenerationId = 0;

    public override string ToString()
    {
        return String.Join(", ", Array.ConvertAll(this.Genomes, genome => genome.ToString()));
    }

    public object Clone()
    {
        return new Population
        {
            MetaPopulation = this.MetaPopulation,
            Genomes = Array.ConvertAll(this.Genomes, genome => (Genome)genome.Clone()),
            GenerationId = this.GenerationId
        };
    }
}
