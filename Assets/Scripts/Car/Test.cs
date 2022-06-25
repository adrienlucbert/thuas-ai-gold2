using System.Linq;
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
                    FitnessFn = this.Evaluate
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
        Population[] populations = ga.Run(this.Seed);
    }

    private float Evaluate(float[] values)
    {
        return values.Sum();
    }
}
