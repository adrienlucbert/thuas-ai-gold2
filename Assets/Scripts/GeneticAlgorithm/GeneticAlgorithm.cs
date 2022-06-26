using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm
{
    public enum BestIsEnum
    {
        Maximum,
        Minimum
    }
    public struct GeneticOperationInfo
    {
        public Func<MetaGenome, Func<int, List<Genome>>, Genome> Function;
        public float Weight;
    }

    public MetaPopulation MetaPopulation;
    public uint GenerationsCount;
    public float CullingRatio;
    public GeneticOperationInfo[] GeneticOperations;
    public BestIsEnum BestIs = BestIsEnum.Maximum;
    public uint MaxSimulateousCars = 1;
    public bool IsVerbose = false;
    public Action<Population, bool> OnStartGeneration = null;
    public Action<Population, bool> OnEndGeneration = null;
    public Action<Genome> OnEvaluateGenome = null;

    public IEnumerator Run(int? seed = null)
    {
        if (seed.HasValue)
            RNG.Seed(seed.Value);

        this.Log($"Running genetic algorithm over {this.GenerationsCount} generations, best is: {this.BestIs}");
        List<Population> populations = new List<Population>();
        populations.Add(this.MetaPopulation.MakePopulation(0));

        for (uint i = 0; i < this.GenerationsCount; ++i)
        {
            Population population = (Population)populations.Last().Clone();
            population.GenerationId += 1;
            populations.Add(population);
            bool isLastGeneration = i == this.GenerationsCount - 1;
            this.OnStartGeneration?.Invoke(population, isLastGeneration);

            // Evaluate genomes
	        bool evaluationDone = false;
	        while (!evaluationDone)
	        {
		        evaluationDone = true;
                uint activeCars = 0;
		        foreach (Genome genome in population.Genomes)
		        {
                    if (genome.Fitness.HasValue || activeCars >= this.MaxSimulateousCars)
				        continue;
                    if (!genome.IsInit)
                        genome.Init();
                    ++activeCars;
                    genome.Update();
                    if (genome.Fitness.HasValue)
                        this.OnEvaluateGenome?.Invoke(genome);
			        evaluationDone = false;
		        }
		        yield return new WaitForFixedUpdate();
	        }

            // Sort genomes by fitness
            Array.Sort(population.Genomes, (Genome a, Genome b) =>
            {
                if (this.BestIs == BestIsEnum.Maximum)
                    return a.CompareTo(b);
                else
                    return b.CompareTo(a);
            });
            this.Log($"{population.GenerationId}, best genome: {population.Genomes[0]}");
            this.OnEndGeneration?.Invoke(population, isLastGeneration);

            if (isLastGeneration)
                // This is the last population to evaluate, we can skip
                // the culling and populating parts
                continue;

            // Cull the population
            int sizeAfterCulling = (int)Math.Ceiling(population.Genomes.Length * (1 - this.CullingRatio));
            int nToPopulate = (int)this.MetaPopulation.Size - sizeAfterCulling;
            for (int j = 0; j < nToPopulate; ++j)
            {
                int genomeIndex = sizeAfterCulling + j;
                var operation = this.PickRandomGeneticOperation();
                population.Genomes[genomeIndex] = operation(
                    this.MetaPopulation.MetaGenome,
                    n => this.PickParentsFromPopulation(population, n));
            }
        }
    }

    private List<Genome> PickParentsFromPopulation(Population population, int parentsCount)
    {
        List<Genome> parents = new List<Genome>();
        for (int i = 0; i < parentsCount; ++i)
        {
            int index = RNG.Range(0, population.Genomes.Length - 1);
            parents.Add(population.Genomes[index]);
        }
        return parents;
    }

    private Func<MetaGenome, Func<int, List<Genome>>, Genome> PickRandomGeneticOperation()
    {
        float weightsSum = this.GeneticOperations.Sum(info => info.Weight);
        float threshold = RNG.Range(0, weightsSum);
        float accumulator = 0f;
        foreach (GeneticOperationInfo operation in this.GeneticOperations)
        {
            accumulator += operation.Weight;
            if (accumulator >= threshold)
                return operation.Function;
        }
        throw new Exception("Failed to pick a genetic operation");
    }

    private void Log(string value)
    {
        if (this.IsVerbose)
            Debug.Log(value);
    }
}
