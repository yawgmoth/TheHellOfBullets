using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum Team
    {
        PLAYER, ENEMIES
    }
    /// <summary>
    /// How often can the unit shoot (every <firerate> seconds)
    /// </summary>
    public float firerate;

    /// <summary>
    /// How much health does the unit have?
    /// </summary>
    public float health;

    /// <summary>
    /// How much health does the Unit have at most?
    /// </summary>
    public float maxhealth;

    /// <summary>
    /// What should we shoot?
    /// </summary>
    public GameObject projectile;

    /// <summary>
    /// Which team are we on?
    /// </summary>
    public Team team;

    // Remember when the last shot was fired
    public float last_shot;

    // Start is called before the first frame update
    void Start()
    {
        last_shot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void OnDeath()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void Fire(float damage, float lifetime, float speed, Vector3 direction, float size=0.15f, int color=0)
    {
        if (last_shot + firerate > Time.time) return;
        GameObject proj = Instantiate(projectile, transform.position + direction.normalized * 0.5f, Quaternion.LookRotation(direction));
        var projcontroller = proj.GetComponent<ProjectileController>();
        projcontroller.damage = damage;
        projcontroller.lifetime = lifetime;
        projcontroller.speed = speed;
        projcontroller.owner = team;
        proj.transform.localScale = new Vector3(size, size, size);
        switch(color)
        {
            case 1:
                proj.GetComponent<MeshRenderer>().material.color = Color.magenta;
                break;
            case 2:
                proj.GetComponent<MeshRenderer>().material.color = Color.cyan;
                break;
        }
        last_shot = Time.time;
    }
}
