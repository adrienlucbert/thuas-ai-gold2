using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public int Seed;
    public GameObject CarPrefab;
    public Transform StartPosition;
    public uint GenerationsCount = 3;
    public uint GenerationsSize = 10;
    public uint MaxSimulateousCars = 10;
    private List<CarController> _cars = new List<CarController>();

    private object GenomeToCar(Genome genome)
    {
        CarController.Parameters parameter = new CarController.Parameters
        {
            AnchorFrontWheel = new Vector2(Random.Range(0f, 2f), Random.Range(-1.2f, 0.5f)),
            AnchorBackWheel = new Vector2(Random.Range(-1.4f, 0f), Random.Range(-1.2f, 0.5f))
    };
        return this.SpawnCar(parameter);
    }

    private CarController SpawnCar(CarController.Parameters parameters)
    {
        GameObject carGameObject = Instantiate(this.CarPrefab, this.StartPosition.position, this.StartPosition.rotation);
        CarController carController = carGameObject.GetComponentInChildren<CarController>();
        carController.Init(parameters);
        this._cars.Add(carController);
        return carController;
    }

    private void UpdateCar(Genome genome)
    {
        ((CarController)genome.Context).UpdateGenome(genome);
    }

    private void Start()
    {
        GeneticAlgorithm ga = new GeneticAlgorithm
        {
            MetaPopulation = new MetaPopulation
            {
                MetaGenome = new MetaGenome
                {
                    MetaGenes = new MetaGene[] {
                        new MetaGene { Name = "AnchorFrontWheelX", Bounds = (0f, 2f) },
                        new MetaGene { Name = "AnchorFrontWheelY", Bounds = (-1.2f, 0.5f) },
                        new MetaGene { Name = "AnchorBackWheelX", Bounds = (-1.4f, 0f) },
                        new MetaGene { Name = "AnchorBackWheelY", Bounds = (-1.2f, 0.5f) }
                    },
                    FitnessFn = this.EvaluateFitness,
                    Init = this.GenomeToCar,
                    Update = this.UpdateCar
                },
                Size = this.GenerationsSize
            },
            GenerationsCount = this.GenerationsCount,
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
            MaxSimulateousCars = this.MaxSimulateousCars,
            IsVerbose = true
        };
        StartCoroutine(ga.Run(this.Seed, this.OnAfterUpdate));
    }

    private IEnumerator OnAfterUpdate()
    {
        yield return new WaitForFixedUpdate();
    }

    private float EvaluateFitness(Genome genome)
    {
        float fitness = ((CarController)genome.Context).RigidBody.position.x - this.StartPosition.position.x;
        Destroy(((CarController)genome.Context).transform.parent.gameObject);
        return fitness;
    }
}
