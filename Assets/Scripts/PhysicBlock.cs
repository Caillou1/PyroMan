using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBlock : MonoBehaviour {

    protected AudioSource source;
    protected Rigidbody rb;
    private bool despawn;

    void Start()
    {
        Init();
    }

    protected void Init()
    {
        despawn = false;
        source = GameObject.Find("Player").GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(despawn)
        {
            transform.Translate(Vector3.down * Time.deltaTime);
        }
    }

    public void Throw(Vector3 pos, float ExplosionForce, float ExplosionRadius)
    {
        GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, pos, ExplosionRadius);
    }

    public void ThrowRandom(float Force)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)) * Force);
    }

    public void ToggleRigidBody(bool toggle)
    {
        rb.isKinematic = !toggle;
    }
}