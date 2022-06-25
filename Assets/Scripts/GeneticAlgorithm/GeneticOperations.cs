using System;
using System.Collections.Generic;

public static class GeneticOperations
{
    /// <summary>
    /// Creates a fully random new genome
    /// </summary>
    public static Genome FullMutation(MetaGenome metaGenome, Func<int, List<Genome>> PickNParents)
    {
        return metaGenome.MakeRandomGenome();
    }

    /// <summary>
    /// Copies a genome from the population and changes randomly one of its genes
    /// </summary>
    public static Genome PartialMutation(MetaGenome metaGenome, Func<int, List<Genome>> PickNParents)
    {
        Genome genome = (Genome)PickNParents(1)[0].Clone();
        uint mutationPos = (uint)RNG.Range(0, metaGenome.MetaGenes.Length);
        genome[mutationPos] = metaGenome.MetaGenes[mutationPos].MakeRandomGene();
        return genome;
    }

    /// <summary>
    /// Performs a uniform crossover between 2 parents: randomly pick genes
    /// from parent 1 and 2
    /// </summary>
    public static Genome Crossover(MetaGenome metaGenome, Func<int, List<Genome>> PickNParents)
    {
        List<Genome> parents = PickNParents(2);
        List<float> genesValues = new List<float>();
        for (int i = 0; i < metaGenome.MetaGenes.Length; ++i)
        {
            if (RNG.Range(0, 1) == 0)
                genesValues.Add(parents[0].Genes[i].Value);
            else
                genesValues.Add(parents[1].Genes[i].Value);
        }
        return metaGenome.MakeGenome(genesValues.ToArray());
    }
}
