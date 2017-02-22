using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : PhysicBlock {
    public float BreakForce;

    private Transform[] Shards;

    private void Start()
    {
        Shards = GetComponentsInChildren<Transform>();
    }

    public void Break()
    {
        foreach (var shard in Shards)
        {
            shard.parent = null;
            shard.GetComponent<Rigidbody>().isKinematic = false;
            shard.GetComponent<MeshRenderer>().enabled = true;
            shard.GetComponent<BoxCollider>().enabled = true;
            Destroy(shard.gameObject, Random.Range(10,20));
        }

        Destroy(gameObject);

        ThrowRandom(BreakForce);
    }
}
