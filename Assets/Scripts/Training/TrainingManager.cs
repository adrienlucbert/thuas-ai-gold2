using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrainingManager : MonoBehaviour
{
    public GameObject CarPrefab;
    public Transform StartPosition;
    public uint GenerationsCount = 3;
    public uint GenerationsSize = 10;
    public uint MaxSimulateousCars = 10;
    private List<CarController> _cars = new List<CarController>();
    private CarController _bestGenomeInstance = null;
    private Genome _bestGenome;
    public Genome BestGenome
    {
        get => this._bestGenome;
        set
        {
            this._bestGenome = value;
            this.OnNewBestGenome?.Invoke(value);
        }
    }

    public UnityEvent<Genome> OnNewBestGenome;
    public UnityEvent<Population, bool> OnEndGeneration;
    public UnityEvent<Population, bool> OnStartGeneration;

    private object GenomeToCar(Genome genome)
    {
        CarController.Parameters parameter = new CarController.Parameters
        {
            FrontHasTraction = genome["FrontHasTraction"].Value > 0.5f,
            BackHasTraction = genome["BackHasTraction"].Value > 0.5f,
            AnchorFrontWheel = new Vector2(genome["AnchorFrontWheelX"].Value, genome["AnchorFrontWheelY"].Value),
            AnchorBackWheel = new Vector2(genome["AnchorBackWheelX"].Value, genome["AnchorBackWheelY"].Value),
            FrontWheelSize = genome["FrontWheelSize"].Value,
            BackWheelSize = genome["BackWheelSize"].Value,
            CarBodySize = genome["CarBodySize"].Value
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
        this.Run();
    }

    public void Run()
    {
        if (this._bestGenomeInstance != null)
            Destroy(this._bestGenomeInstance.transform.parent.gameObject);
        GeneticAlgorithm ga = new GeneticAlgorithm
        {
            MetaPopulation = new MetaPopulation
            {
                MetaGenome = new MetaGenome
                {
                    MetaGenes = new MetaGene[] {
                        new MetaGene { Name = "FrontHasTraction", Bounds = (0f, 1f) },
                        new MetaGene { Name = "BackHasTraction", Bounds = (0f, 1f) },
                        new MetaGene { Name = "AnchorFrontWheelX", Bounds = (0f, 2f) },
                        new MetaGene { Name = "AnchorFrontWheelY", Bounds = (-1.2f, 0.5f) },
                        new MetaGene { Name = "AnchorBackWheelX", Bounds = (-1.4f, 0f) },
                        new MetaGene { Name = "AnchorBackWheelY", Bounds = (-1.2f, 0.5f) },
                        new MetaGene { Name = "BackWheelSize", Bounds = (0.3f, 1.2f) },
                        new MetaGene { Name = "FrontWheelSize", Bounds = (0.3f, 1.2f) },
                        new MetaGene { Name = "CarBodySize", Bounds = (0.5f, 1.5f) }
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
            IsVerbose = true,
            OnStartGeneration = this.OnStartGenerationHandler,
            OnEndGeneration = this.OnEndGenerationHandler,
            OnEvaluateGenome = this.OnEvaluateGenome
        };
        StartCoroutine(ga.Run());
    }

    public void PlayBestGenome()
    {
        if (this._bestGenomeInstance != null)
            Destroy(this._bestGenomeInstance.transform.parent.gameObject);
        this._bestGenomeInstance = (CarController)this.GenomeToCar(this.BestGenome);
    }

    private void OnEvaluateGenome(Genome genome)
    {
        if (this.BestGenome == null || genome.Fitness.Value > this.BestGenome.Fitness.Value)
        {
            this.BestGenome = genome;
        }
    }

    private void OnStartGenerationHandler(Population generation, bool isLast)
    {
        this.OnStartGeneration?.Invoke(generation, isLast);
    }

    private void OnEndGenerationHandler(Population generation, bool isLast)
    {
        this.OnEndGeneration?.Invoke(generation, isLast);
    }

    private float EvaluateFitness(Genome genome)
    {
        CarController carController = (CarController)genome.Context;
        float distance = carController.RigidBody.position.x - this.StartPosition.position.x;
        float time = Time.time - carController.StartTime;
        Destroy(carController.transform.parent.gameObject);
        return distance - time;
    }
}
