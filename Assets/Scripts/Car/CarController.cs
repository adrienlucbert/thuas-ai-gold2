using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public WheelJoint2D frontwheel;
    public WheelJoint2D backwheel;

    JointMotor2D motorFront;
    JointMotor2D motorBack;
    
    public Rigidbody2D RigidBody;

    private float _startTime;

    public class Parameters
    {
        public float SpeedFront = 1000f;
        public float SpeedBack = -800f;
        public float TorqueFront = 10000f;
        public float TorqueBack = 10000f;
        public bool FrontHasTraction = true;
        public bool BackHasTraction = true;
        public float RotationSpeed = 70f;
        public Vector2 AnchorFrontWheel;
        public Vector2 AnchorBackWheel;
    }

    public void Init(Parameters parameters)
    {
        this.frontwheel.anchor = parameters.AnchorFrontWheel;
        this.backwheel.anchor = parameters.AnchorBackWheel;

        if (parameters.FrontHasTraction)
        {
            this.motorFront.motorSpeed = parameters.SpeedFront * -1;
            this.motorFront.maxMotorTorque = parameters.TorqueFront;
            this.frontwheel.motor = this.motorFront;
        }

        if (parameters.BackHasTraction)
        {
            this.motorBack.motorSpeed = parameters.SpeedFront * -1;
            this.motorBack.maxMotorTorque = parameters.TorqueFront;
            this.backwheel.motor = this.motorBack;
        }

        this._startTime = Time.time;
    }

    public void UpdateGenome(Genome genome)
    {
        if (Time.time - this._startTime > 1.0f && this.RigidBody.velocity.magnitude < 0.3f) 
            genome.Evaluate();
    }
}