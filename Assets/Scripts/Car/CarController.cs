using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public WheelJoint2D frontwheel;
    public WheelJoint2D backwheel;

    JointMotor2D motorFront;
    JointMotor2D motorBack;
    Rigidbody2D rb;

    public float speedF;
    public float speedB;
    public float torqueF;
    public float torqueB;
    public bool TractionFront = true;
    public bool TractionBack = true;
    public float carRotationSpeed;
    private float timer = 0.0f;
    private float startpositionx;
    private float startpositiony;
    private Quaternion startrotation;
    private float fscore;

    public void InitializePopulation()
    {
        // start population with random combinations
        frontwheel.anchor = new Vector2(Random.Range(0f, 2f), Random.Range(-1.2f, 0.5f));
        backwheel.anchor = new Vector2(Random.Range(-1.4f, 0f), Random.Range(-1.2f, 0.5f));
    }

    public void FitScore()
    {
        // determine the fit score
        fscore = rb.transform.position.x - GameObject.Find("Start").transform.position.x;
    }

    public void SelectFitess()
    {
        // based on the fit score make selection
    }

    public void ChangeChromosome()
    {
        // change the genes of the chromosomes by
        // crossing over and/or mutation
    }

    public void ApplyHeuristic()
    {
        // speed up the process by for example filtering out products from which the internal chromosome are too similar
        // can be skipped for a first proof of concept
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializePopulation();
        startpositionx = transform.position.x;
        startpositiony = transform.position.y;
        startrotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (TractionFront)
        {
            motorFront.motorSpeed = speedF * -1;
            motorFront.maxMotorTorque = torqueF;
            frontwheel.motor = motorFront;
        }

        if (TractionBack)
        {
            motorBack.motorSpeed = speedF * -1;
            motorBack.maxMotorTorque = torqueF;
            backwheel.motor = motorBack;
        }

        if (rb.velocity.magnitude < 0.3f && timer > 1.0f)
        {
            // save fitness score
            FitScore();
            GameObject.Find("Fitscore").GetComponent<Text>().text = fscore.ToString();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;
            transform.position = new Vector3(startpositionx, startpositiony, 0f);
            transform.rotation = startrotation;
            frontwheel.connectedBody.velocity = Vector2.zero;
            frontwheel.connectedBody.angularVelocity = 0f;
            backwheel.connectedBody.velocity = Vector2.zero;
            backwheel.connectedBody.angularVelocity = 0f;
            InitializePopulation(); // should not be init, but should spawn next in car collection of this generation
            timer = 0f;
        }
    }
}