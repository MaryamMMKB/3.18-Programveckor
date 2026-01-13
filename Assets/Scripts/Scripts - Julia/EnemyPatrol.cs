using UnityEngine;


public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform[] waypoints;

    [Header("Movement Settings")]
    public float speed = 2f;
    public bool loop = true;

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        Transform target = waypoints[currentIndex];

        // Flytta enemy mot target
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // När fienden når punkten
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                if (loop)
                    currentIndex = 0;
                else
                    currentIndex = waypoints.Length - 1;
            }
        }

        // Vänd sprite/rotation beroende på riktning
        if (target.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
}