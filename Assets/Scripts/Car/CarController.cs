using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public WheelJoint2D frontwheel;
    public WheelJoint2D backwheel;

    JointMotor2D motorFront;
    JointMotor2D motorBack;
    
    public Rigidbody2D RigidBody;

    [HideInInspector] public float StartTime;

    public class Parameters
    {
        public float SpeedFront = 1000f;
        public float SpeedBack = -800f;
        public float TorqueFront = 10000f;
        public float TorqueBack = 10000f;
        public bool FrontHasTraction = true;
        public bool BackHasTraction = true;
        public Vector2 AnchorFrontWheel;
        public Vector2 AnchorBackWheel;
        public float FrontWheelSize;
        public float BackWheelSize;
        public float CarBodySize;
    }

    public void Init(Parameters parameters)
    {
        this.frontwheel.anchor = parameters.AnchorFrontWheel;
        this.backwheel.anchor = parameters.AnchorBackWheel;
        this.frontwheel.connectedBody.gameObject.transform.localScale = new Vector3(parameters.FrontWheelSize, parameters.FrontWheelSize, parameters.FrontWheelSize);
        this.backwheel.connectedBody.gameObject.transform.localScale = new Vector3(parameters.BackWheelSize, parameters.BackWheelSize, parameters.BackWheelSize);
        this.transform.localScale = new Vector3(parameters.CarBodySize, parameters.CarBodySize, parameters.CarBodySize);

        if (parameters.FrontHasTraction)
        {
            this.motorFront.motorSpeed = parameters.SpeedFront * -1;
            this.motorFront.maxMotorTorque = parameters.TorqueFront;
            this.frontwheel.motor = this.motorFront;
        }

        if (parameters.BackHasTraction)
        {
            this.motorBack.motorSpeed = parameters.SpeedBack * -1;
            this.motorBack.maxMotorTorque = parameters.TorqueBack;
            this.backwheel.motor = this.motorBack;
        }

        this.StartTime = Time.time;
    }

    public void UpdateGenome(Genome genome)
    {
        if (Time.time - this.StartTime > 1.0f && this.RigidBody.velocity.magnitude < 0.3f) 
            genome.Evaluate();
    }
}