using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Transform closedPos;
    private Transform openPos;
    private bool closed;

    // Start is called before the first frame update
    void Start()
    {
        openPos = transform;
        closed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (closed) {
            transform.position = Vector3.Lerp(transform.position, closedPos.position, 5f * Time.deltaTime); 
        } else {
            transform.position = Vector3.Lerp(transform.position, openPos.position, 5f * Time.deltaTime); 
        }
    }

    public void Close() {
        closed = true;
    }

    public void Open() {
        closed = false;
    }
}
