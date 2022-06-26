using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int Seed = 0;

    [ContextMenu("TestGA")]
    public void TestGA()
    {
        GeneticAlgorithm ga = new GeneticAlgorithm
        {
            MetaPopulation = new MetaPopulation
            {
                MetaGenome = new MetaGenome
                {
                    MetaGenes = new MetaGene[] {
                        new MetaGene { Name = "x", Bounds = (0f, 1f) },
                        new MetaGene { Name = "y", Bounds = (0f, 5f) }
                    },
                    FitnessFn = this.Evaluate,
                    Init = this.InitGenomeContext,
                    Update = this.UpdateGenomeContext
                },
                Size = 3
            },
            GenerationsCount = 20,
            CullingRatio = 0.42f,
            GeneticOperations = new GeneticAlgorithm.GeneticOperationInfo[]
            {
                new GeneticAlgorithm.GeneticOperationInfo
                {
                    Function = GeneticOperations.FullMutation,
                    Weight = 0.3f
                },
                new GeneticAlgorithm.GeneticOperationInfo
                {
                    Function = GeneticOperations.PartialMutation,
                    Weight = 0.4f
                },
                new GeneticAlgorithm.GeneticOperationInfo
                {
                    Function = GeneticOperations.Crossover,
                    Weight = 0.3f
                }
            },
            BestIs = GeneticAlgorithm.BestIsEnum.Maximum,
            IsVerbose = true
        };
        StartCoroutine(this.SyncRun(ga));
    }

    private IEnumerator OnAfterUpdate()
    {
        yield return new WaitForFixedUpdate();
    }

    private IEnumerator SyncRun(GeneticAlgorithm ga)
    {
	    yield return ga.Run(this.Seed, this.OnAfterUpdate);
    }

    private object InitGenomeContext(Genome genome)
    {
        return null;
    }
    
    private void UpdateGenomeContext(Genome genome)
    {
        genome.Evaluate();
    }

    private float Evaluate(Genome genome)
    {
        float sum = 0f;
        foreach (Gene gene in genome.Genes)
            sum += gene.Value;
	    return sum;
    }
}
