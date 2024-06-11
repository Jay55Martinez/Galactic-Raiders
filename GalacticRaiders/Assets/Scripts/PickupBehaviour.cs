using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
