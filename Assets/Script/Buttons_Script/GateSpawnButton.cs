using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GateSpawnButton : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private int direction = 1;

    //public GameObject gate1;
    //public GameObject gate2;
    [SerializeField] private float speed = .2f;

    private void Update()
    {
        //Debug.Log("Direction: " + direction);

        Vector2 target = CurrentMovementTarget();

        if (direction == 1)
        {
            platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);
        }
        else if (direction == -1)
        {

            platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 target = CurrentMovementTarget();

        if (collision.gameObject.CompareTag("Playerr"))
        {
            //gate1.transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, Time.deltaTime * speed);
            //gate1.SetActive(true);
            //gate2.SetActive(true);
        }

        float distance = (target - (Vector2)platform.position).magnitude;

        direction *= -1;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Vector2 target = CurrentMovementTarget();

        //if (collision.gameObject.CompareTag("Playerr"))
        {
            //gate1.transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, Time.deltaTime * speed);
            //gate1.SetActive(true);
            //gate2.SetActive(true);
        }
        float distance = (target - (Vector2)platform.position).magnitude;

        direction *= -1;
    }

    Vector2 CurrentMovementTarget()
    {
        if (direction == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;
        }
    }

}
