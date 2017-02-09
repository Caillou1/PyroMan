using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : PhysicBlock {
    public float Flammability;
    public float TimeBeforeExpansion;
    public float PMTimeBeforeExpansion = .5f;
    public GameObject Flamme;
    public float GrowthRate = .01f;
    public float GrowthTime = .1f;
    public float MaxScale = 1f;
    public float ExpansionDistance = 1f;
    public float LifeTime = 5f;
    public float PMLifeTime = 1f;
    public AudioClip FlameSound;

    private bool isCoroutineStarted = false;
    private GameObject mFlamme;

    public void StartFlame(Vector3 hit)
    {
        if(!isCoroutineStarted)
        {
            isCoroutineStarted = true;
            StartCoroutine(WaitForFlame(hit));
        }
    }

    public void StopFlame()
    {
        StopAllCoroutines();
        isCoroutineStarted = false;
    }

    void StartFire(Vector3 hit)
    {
        mFlamme = Instantiate(Flamme, hit, Quaternion.identity, transform);
        mFlamme.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        StartCoroutine(ScaleFlame());
    }

    void Expand()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, ExpansionDistance);

        foreach(Collider hit in hits)
        {
            PhysicBlock physicBlock = hit.transform.GetComponent<Flammable>();
            if (physicBlock != null)
            {
                ((Flammable)(physicBlock)).StartFlame(hit.transform.position);
            } else
            {
                physicBlock = hit.transform.GetComponent<Explosive>();
                if(physicBlock != null)
                {
                    ((Explosive)(physicBlock)).CountDown();
                }
            }
        }
    }

    IEnumerator ScaleFlame()
    {
        yield return new WaitForSeconds(GrowthTime);
        mFlamme.transform.localScale += new Vector3(GrowthRate, GrowthRate, GrowthRate);

        ParticleSystem ps = mFlamme.GetComponent<ParticleSystem>();
        var em = ps.emission;
        if (em.rateOverTime.constant < 5)
            em.rateOverTime = new ParticleSystem.MinMaxCurve(ps.emission.rateOverTime.constant + .1f);

        if (mFlamme.transform.localScale.x < MaxScale)
            StartCoroutine(ScaleFlame());
        else
            StartCoroutine(Kill());
    }

    IEnumerator WaitForFlame(Vector3 hit)
    {
        yield return new WaitForSeconds(1 / Flammability);
        StartFire(hit);
        StartCoroutine(Expansion());
    }

    IEnumerator Expansion()
    {
        yield return new WaitForSeconds(TimeBeforeExpansion + Random.Range(-PMTimeBeforeExpansion,PMTimeBeforeExpansion));
        Expand();
        StartCoroutine(Expansion());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(LifeTime + Random.Range(-PMLifeTime, PMLifeTime));
        Explosive exp = GetComponent<Explosive>();
        if (exp != null)
        {
            exp.CountDown();
        } else
        {
            Destroy(gameObject);
        }
    }
}