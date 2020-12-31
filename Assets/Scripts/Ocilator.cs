using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocilator : MonoBehaviour
{
    public Vector3 movementVector = new Vector3(5f , 0f, 0f);
    float movementFactor;
    Vector3 initialPos;
    Vector3 offset;

    public float period = 2f;

    private void Awake()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        if (period == 0f) 
        {
            period = 1;
        }
        float Cycles = Time.time / period;
        const float tau = Mathf.PI;
        float sinWave = Mathf.Sin(tau * Cycles);

        offset = movementVector * sinWave;

        transform.position = initialPos + offset;
    }

}
