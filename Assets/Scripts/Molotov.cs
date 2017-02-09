using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour {

    public float CheckRadius;
    public float FuelQuantity;
    public float EjectionForce;

    public GameObject Fuel;

    private Transform tf;
    private Flammable flammable;

    private void Start()
    {
        tf = transform;
        StartCoroutine(WaitForGround());
        flammable = GetComponent<Flammable>();
    }

    private bool isGrounded()
    {
        return Physics.CheckSphere(tf.position, CheckRadius);
    }

    private void SpreadFuel()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        flammable.StartFlame(tf.position);
        for (int i = 0; i< FuelQuantity; i++)
        {
            var fuel = Instantiate(Fuel, tf.position + new Vector3(Random.Range(.1f, .5f), Random.Range(.1f, .5f), Random.Range(.1f, .5f)), Quaternion.identity);
            fuel.GetComponent<PhysicBlock>().ThrowRandom(EjectionForce);
        }
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, .5f);
    }

    IEnumerator WaitForGround()
    {
        yield return new WaitForSeconds(.5f);
        yield return new WaitUntil(() => isGrounded());
        SpreadFuel();
    }
}
