using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesCountText : MonoBehaviour
{
    public new GameObject gameObject;
    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void requireTextMesh() {
        if(this.textMesh == null) {
            this.textMesh = this.gameObject.GetComponent<TextMesh>();
        }
    }

    public void SetText(string text) {
        this.requireTextMesh();
        this.textMesh.text = text;
    }

    public void SetSize(float size) {
        this.gameObject.transform.localScale = new Vector3(size, size, size);
    }
}
