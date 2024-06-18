using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehaviour : MonoBehaviour
{
    public int worth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
    }

    public void Die() {
        // add currency to gamemanager
        FindObjectOfType<LevelManager>().IncreaseCurrency(worth);
        Destroy(gameObject);
    }
}
