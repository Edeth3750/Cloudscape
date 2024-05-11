using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    [Header("Firefly Movement")]
    [SerializeField] private float fireflySpeed = 1f;
    [SerializeField] private float fireflyAcceleration = 2f;
    [SerializeField] private float initialMinFlyTimer = 0.5f;
    [SerializeField] private float initialMaxFlyTimer = 2f;
    [SerializeField] private float minFloatTimer = 1f;
    [SerializeField] private float maxFloatTimer = 2.5f;
    [SerializeField] private float ignoreSpeedLimitTimer = 0.2f;

    private float initialTimer;
    private float timer;
    private float setTimer;
    private float ignoreTimer;
    private float direction;
    
    // Start is called before the first frame update
    void Start()
    {
        initialTimer = Random.Range(initialMinFlyTimer, initialMaxFlyTimer);
        setTimer = Random.Range(minFloatTimer, maxFloatTimer);
        timer = setTimer;
        ignoreTimer = 0f;
        direction = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // At initial state, have the fireflies float up for an added time before refering to its normal float time
        if (initialTimer <= 0f)
        {
            // Wait before the timer runs out so that it reverses the hovering direction. Also reset ignore timers every time the normal timer runs out
            if (timer <= 0f)
            {
                direction *= -1f;
                timer = setTimer;
                ignoreTimer = ignoreSpeedLimitTimer;
            }
            else
            {
                timer -= Time.deltaTime;
                ignoreTimer -= Time.deltaTime;
            }
        }
        else
        {
            initialTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // The driving force of moving the firefly under a speed limit. Ignores speed limit whenever the ignore timer is active so that it can change directions
        if (this.GetComponent<Rigidbody>().velocity.magnitude < fireflySpeed || ignoreTimer > 0f)
        {
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0f, fireflyAcceleration * direction, 0f), ForceMode.Acceleration);
        }
    }
}
