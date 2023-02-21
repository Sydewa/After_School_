using UnityEngine;

public class FallingLeafState : LeafBaseState
{
    private Rigidbody leafRigidbody;
    private float fallSpeed = 4.2f;
    private float rotationSpeed = 10f;
    private float windStrength = 0.6f;
    private float drag = 0.01f;

    public override void EnterState(GameObject leaf)
    {
        leafRigidbody = leaf.GetComponent<Rigidbody>();

        // Apply a random rotation to the leaf to give it a natural look
        leaf.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Add an initial upward force to the leaf to give it some lift
        Vector3 initialForce = Vector3.up * Random.Range(0f, 1f);
        leafRigidbody.AddForce(initialForce, ForceMode.Impulse);

        leafRigidbody.mass = 1.8f;
        leafRigidbody.drag = drag;
    }

    public override void UpdateState(GameObject leaf)
    {
        // Apply a downward force to simulate gravity
        Vector3 gravity = Vector3.down * fallSpeed;
        leafRigidbody.AddForce(-gravity, ForceMode.Force);

        // Rotate the leaf as it falls
        leaf.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Apply a random force to simulate the effect of wind
        Vector3 windDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        leafRigidbody.AddForce(windDirection * windStrength * Time.deltaTime, ForceMode.Force);

        // Check if the leaf has collided with an object
        if (leaf.transform.position.y < 0f)
        {
            // Transition to another state, such as the idle state
            
        }
    }

    public override void ExitState(GameObject leaf)
    {
        // Reset the leaf's drag to its default value
        leafRigidbody.drag = 0f;
    }
}
