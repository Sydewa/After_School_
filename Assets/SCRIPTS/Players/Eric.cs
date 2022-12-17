using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eric : MonoBehaviour
{
    [SerializeField]Stats stats;
    
    private CharacterController controller;
    
    //Variables de movimiento
    Vector3 velocity;
    [SerializeField]float smoothTimeMove;
    [SerializeField]float smoothTimeLookAtMouse;
    [SerializeField]float accelerationTime;
    float timePassed;

    //Variables Smooth rotacion
    [SerializeField]float turnSmoothTime;
    float turnSmoothVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        if(move != Vector3.zero && Input.GetButton("Fire1") == false)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        
        else if(Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = hit.point - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
            }
        } 
        if(move != Vector3.zero)
        {
            float x = 0;
            timePassed += Time.deltaTime;
            float acceleration = timePassed / accelerationTime; // Calculate the change in x over each unit of time

            x = Mathf.Lerp(0, 1, acceleration);

            float currentSpeed = Mathf.Lerp(0, stats.speed, x); // Gradually increase the speed
            controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            timePassed = 0;
        }
    }

    void OnCollisionEnter()
    {
        stats.vida = stats.vida - 20;
        Debug.Log(stats.vida);
        if(stats.vida < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
