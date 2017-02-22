using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : PhysicBlock {

    public float ExplosionForce;
    public float ExplosionRadius;
    public float TimeBeforeExplosion;
    public float PMTimeBeforeExplosion;
    public GameObject Explosion;
    public AudioClip ExplosionSound;

    private bool Exploded = false;

    public void Explode()
    {
        if (!Exploded)
        {
            Exploded = true;
            source.PlayOneShot(ExplosionSound);
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(Instantiate(Explosion, transform.position, Quaternion.identity), 5);
            Destroy(gameObject);
            Collider[] hits = Physics.OverlapSphere(transform.position, 10);
            foreach (var hit in hits)
            {
                var flam = hit.GetComponent<PhysicBlock>();
                if (flam != null)
                {
                    flam.Throw(transform.position, ExplosionForce, ExplosionRadius);
                }

                var b1 = hit.GetComponent<Breakable>();
                if(b1 != null)
                {
                    b1.Break();
                }

                var fl = hit.GetComponent<Flammable>();
                if (fl != null)
                {
                    fl.StartFlame(fl.transform.position);
                } else
                {
                    var exp = hit.GetComponent<Explosive>();
                    if (exp != null)
                    {
                        if (!exp.HasExploded())
                            exp.CountDown();
                    }
                }
            }
        }
    }

    public void CountDown()
    {
        StartCoroutine(WaitForExplosion());
    }

    IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(TimeBeforeExplosion + Random.Range(-PMTimeBeforeExplosion, PMTimeBeforeExplosion));
        Explode();
    }

    public bool HasExploded()
    {
        return Exploded;
    }
}
