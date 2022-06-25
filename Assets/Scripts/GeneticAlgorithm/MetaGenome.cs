using System;
using System.Diagnostics;
using System.Collections.Generic;

public class MetaGenome
{
    public MetaGene[] MetaGenes;
    public Func<float[], float> FitnessFn;

    public Genome MakeGenome(float[] values = null)
    {
        Debug.Assert(values.Length == this.MetaGenes.Length, $"Expected {this.MetaGenes.Length} values, got {values.Length}");
        List<Gene> genes = new List<Gene>();
        for (int i = 0; i < this.MetaGenes.Length; ++i)
            genes.Add(this.MetaGenes[i].MakeGene(values[i]));
        return new Genome { MetaGenome = this, Genes = genes.ToArray() };
    }

    public Genome MakeRandomGenome()
    {
        List<Gene> genes = new List<Gene>();
        foreach (MetaGene metagene in this.MetaGenes)
            genes.Add(metagene.MakeRandomGene());
        return new Genome { MetaGenome = this, Genes = genes.ToArray() };
    }
}
