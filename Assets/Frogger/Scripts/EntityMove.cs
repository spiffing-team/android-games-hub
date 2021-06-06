using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float velocity;
    public SpriteRenderer rendererRef;
    void Start()
    {
        rendererRef = GetComponent<SpriteRenderer>();
        if (transform.position.x == 0)
        {
            transform.position -= new Vector3(rendererRef.bounds.size.x/2, 0f, 0f);
        }
        else
        {
            transform.position += new Vector3(rendererRef.bounds.size.x / 2, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(velocity * Time.deltaTime, 0f, 0f);
        if (transform.position.x < -10 - rendererRef.bounds.size.x / 2 || transform.position.x > Screen.width + rendererRef.bounds.size.x / 2 + 10)
        {
            Destroy(this);
        }
    }
}
