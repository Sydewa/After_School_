using UnityEngine;

public class FallingLeafState : LeafBaseState
{
    private Rigidbody leafRigidbody;
    private float fallSpeed = 2.7f;
    private float rotationSpeed = 10f;
    private float windStrength = 1f;
    private float drag = 0.5f;

    float elapsedTime;
    float maxTimeFalling = 8f;

    public override void EnterState(GameObject leaf)
    {
        leafRigidbody = leaf.GetComponent<Rigidbody>();

        // Apply a random rotation to the leaf to give it a natural look
        leaf.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 45f));

        // Add an initial upward force to the leaf to give it some lift
        Vector3 initialForce = Vector3.up * Random.Range(0f, 1f);
        leafRigidbody.AddForce(initialForce, ForceMode.Impulse);

        leafRigidbody.mass = 2.2f;
        leafRigidbody.drag = drag;

        //Reset elapsed time
        elapsedTime = 0f;
    }

    public override void UpdateState(GameObject leaf)
    {
        // Gravedad
        Vector3 gravity = Vector3.down * fallSpeed;
        leafRigidbody.AddForce(-gravity, ForceMode.Force);

        // Rotate the leaf as it falls
        leaf.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Apply a random force to simulate the effect of wind
        Vector3 windDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        leafRigidbody.AddForce(windDirection * windStrength * Time.deltaTime, ForceMode.Force);

        elapsedTime += Time.deltaTime;
        // Check if the leaf has collided with an object
        if (elapsedTime >= maxTimeFalling)
        {
            leaf.GetComponent<LeafScript>().SwitchState(leaf.GetComponent<LeafScript>().LeafOnGround);
            
        }
    }

    public override void ExitState(GameObject leaf)
    {
        // Reset the leaf's drag to its default value
    }
}
