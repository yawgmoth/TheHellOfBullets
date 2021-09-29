using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    
    public float speed;
    public float damage;
    public float lifetime;
    public Unit.Team owner;
    public GameObject impact;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherunit = other.gameObject.GetComponent<Unit>();
        if (otherunit != null && otherunit.team != owner)
        {
            otherunit.TakeDamage(damage);
            var imp = Instantiate(impact, transform.position, transform.rotation);

            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
