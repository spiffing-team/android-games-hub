using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0.1f * Time.deltaTime, 0f, 0f);
        if( transform.position.x < -10 || transform.position.x > Screen.width )
        {
            Destroy(this);
        }
    }
}
