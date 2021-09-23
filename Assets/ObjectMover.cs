using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

public class ObjectMover : MonoBehaviour
{
    // Destination and current location
    [SerializeField] Transform target;
    [SerializeField] float landingOffset;
    [SerializeField] double landingDeacceleration = 1;
    [SerializeField] float deaccelarationDistance;
    [SerializeField] float landingDistance;
    [SerializeField] GameObject tracer;
    

    // Constants
    private float speed = 0.5f;
    private float tracerDistance = 0.5f;
    private float traceRemain = 15f;

    // State variables
    Transform previousTracer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Trace();
        Orient();
    }

    public void SetSpeed(PinchSlider slide)
    {
        speed = slide.SliderValue;
    }

    public void SetDistance(PinchSlider slide)
    {
        tracerDistance = slide.SliderValue;
    }

    public void SetRemain(PinchSlider slide)
    {
        traceRemain = slide.SliderValue * 30f;
    }

    private void Move()
    {
        if (GetDistance() > landingOffset)
        {
                transform.position = Vector3.MoveTowards(transform.position, target.position, GetSpeed());
        }
    }

    private float GetDistance()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    private float GetSpeed()
    {
        float distance = GetDistance();
        if (deaccelarationDistance < distance)
        {
            return Time.deltaTime * speed;
        }
        else
        {
            float adjustedSpeed = speed * (float)Math.Pow(distance / deaccelarationDistance, landingDeacceleration);
            return Time.deltaTime * adjustedSpeed;
        }
    }

    private void Trace()
    {
        if (previousTracer == null || 
            Vector3.Distance(previousTracer.position, transform.position) >= tracerDistance)
        {
            GameObject tracerInst = Instantiate(tracer, transform.position, transform.rotation);
            previousTracer = tracerInst.transform;
            Destroy(tracerInst, traceRemain);
        }
    }

    private void Orient()
    {
        transform.LookAt(target.position);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < deaccelarationDistance)
        {
            if (distance / deaccelarationDistance < landingDistance)
            {
                transform.Rotate(180, 0, 0);
            }
            else
            {
                float rotation = 180f * (1 - (distance - deaccelarationDistance * landingDistance) / (deaccelarationDistance * (1 - landingDistance)));
                transform.Rotate(rotation, 0, 0);
            }
        }
    }
}
;
