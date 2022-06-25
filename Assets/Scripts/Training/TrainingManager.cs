using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    private class Car
    {
        public GameObject GameObject;
        public CarController CarController;
        public Rigidbody2D RigidBody;
	    public bool IsDead = false;
	    private float _timer = 0.0f;

	    public static Car Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	    {
		    GameObject carGameObject = Instantiate(prefab, position, rotation);
		    return new Car
		    {
		        GameObject = carGameObject,
		        RigidBody = carGameObject.GetComponent<Rigidbody2D>(),
                CarController = carGameObject.GetComponent<CarController>()
		    };
	    }

	    public void Init()
	    {
	    }

	    public void FixedUpdate()
	    {
		    this._timer += Time.fixedDeltaTime;
		    if (this._timer > 1.0f && this.RigidBody.velocity.magnitude < 0.3f)
			    this.IsDead = true;
	    }
    }

    public GameObject CarPrefab;
    public Transform StartPosition;
    public int MaxSimulateousCars = 10;
    private List<Car> _cars = new List<Car>();

    private void Start()
    {
        /*GeneticAlgorithm ga = new GeneticAlgorithm
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
        StartCoroutine(this.SyncRun(ga));*/
    }

    private void FixedUpdate()
    {
        // Kill stuck cars and spawn new ones in the limits of this.MaxSimulateousCars
	    foreach (Car car in this._cars)
	    {
		    car.FixedUpdate();
		    if (car.IsDead)
		    {
		    }
	    }
    }

    private float EvaluateFitness(GameObject car)
    {
        return car.GetComponent<Rigidbody2D>().position.x - GameObject.Find("Start").transform.position.x;
    }
}
