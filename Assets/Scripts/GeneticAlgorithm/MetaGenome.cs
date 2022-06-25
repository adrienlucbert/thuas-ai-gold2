using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

public class MetaGenome
{
    public MetaGene[] MetaGenes;
    public Func<Genome, object, float> FitnessFn;
    public Func<Genome, object> Init;
    public Action<Genome, object> Update;

    private Genome MakeGenomeFromGenes(Gene[] genes)
    {
        Genome newGenome = new Genome { MetaGenome = this, Genes = genes };
        newGenome.Init();
        return newGenome;
    }

    public Genome MakeGenome(float[] values)
    {
        Debug.Assert(values.Length == this.MetaGenes.Length, $"Expected {this.MetaGenes.Length} values, got {values.Length}");
        List<Gene> genes = new List<Gene>();
        for (int i = 0; i < this.MetaGenes.Length; ++i)
            genes.Add(this.MetaGenes[i].MakeGene(values[i]));
        return this.MakeGenomeFromGenes(genes.ToArray());
    }

    public Genome MakeRandomGenome()
    {
        List<Gene> genes = new List<Gene>();
        foreach (MetaGene metagene in this.MetaGenes)
            genes.Add(metagene.MakeRandomGene());
        return this.MakeGenomeFromGenes(genes.ToArray());
    }
}
