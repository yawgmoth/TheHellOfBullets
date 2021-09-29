using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Unit
{
    public enum MoveType
    {
        PATROL, CIRCUIT, RANDOM
    }

    public enum EnemyType
    {
        SOLDIER, MINELAYER, SHOTGUN
    }
    public GameObject player;
    public GameObject a;
    public GameObject b;

    public GameObject[] waypoints;
    public int current_target;
    public MoveType move_type;
    float t;

    public EnemyType enemy_type;

    public GameObject maxhealthbar;
    public GameObject healthbar;
    // Start is called before the first frame update
    void Start()
    {
        team = Team.ENEMIES;
        t = 0;
        current_target = 1;
        switch (move_type)
        {
            case MoveType.PATROL:
                a = waypoints[0];
                b = waypoints[1];
                break;
            case MoveType.CIRCUIT:
                a = waypoints[0];
                b = waypoints[1];
                break;
            case MoveType.RANDOM:
                a = waypoints[0];
                b = waypoints[Random.Range(1,waypoints.Length)];
                break;
        }
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            // Different Enemy types shoot projectiles with different properties (damage, lifetime, speed)
            switch (enemy_type)
            {
                case EnemyType.SOLDIER:
                    Fire(10, 8, 15, player.transform.position - transform.position);
                    break;
                case EnemyType.MINELAYER:
                    Fire(25, 15, 0.3f, (new Vector3(Random.Range(0.5f, 1.5f), 0, Random.Range(0.5f, 1.5f))).normalized, 0.25f, 2);
                    break;
                case EnemyType.SHOTGUN:
                    Fire(45, 0.1f, 25, player.transform.position - transform.position, 0.1f, 1);
                    break;
            }
            yield return new WaitForSeconds(firerate);
        }
    }

    public override void OnDeath()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime*0.2f;

        if (t > 1)
        {
            t = 0;
            /* Enemies use one of three movement types:
             *     PATROL: When the target is reached, the start and end points are flipped, i.e. the enemy will just patrol back and forth
             *     CIRCUIT: The enemy goes to each of the waypoints in order
             *     RANDOM: When arriving at a waypoint, the enemy will choose the next waypoint to go to at random
             */
            switch (move_type)
            {
                case MoveType.PATROL:
                    var tmp = b;
                    b = a;
                    a = tmp;
                    break;
                case MoveType.CIRCUIT:
                    a = b;
                    current_target += 1;
                    if (current_target >= waypoints.Length)
                        current_target = 0;
                    b = waypoints[current_target];
                    break;
                case MoveType.RANDOM:
                    int i = current_target;
                    while (i == current_target)
                    {
                        i = Random.Range(0, waypoints.Length);
                    }
                    a = b;
                    current_target = i;
                    b = waypoints[current_target];
                    break;
            }
        }

        transform.position = a.transform.position + t * (b.transform.position - a.transform.position);
    }
}
