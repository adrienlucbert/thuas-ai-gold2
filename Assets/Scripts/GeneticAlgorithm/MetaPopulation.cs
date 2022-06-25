using System.Collections.Generic;

public class MetaPopulation
{
    public MetaGenome MetaGenome;
    public uint Size;

    public Population MakePopulation(uint generation = 0)
    {
        List<Genome> genomes = new List<Genome>();
        for (int i = 0; i < this.Size; ++i)
            genomes.Add(this.MetaGenome.MakeRandomGenome());
        return new Population
        {
            MetaPopulation = this,
            Genomes = genomes.ToArray(),
            GenerationId = generation
        };
    }
}
