using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nonoblock : MonoBehaviour
{
    public int x;
    public int y;

    public bool state = false;

    public GameObject cube;

    public Material StandardMaterial;
    public Material SelectedMaterial;

    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        this.requireMeshRenderer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void requireMeshRenderer() {
        if(this.meshRenderer == null) {
            this.meshRenderer = this.cube.GetComponent<MeshRenderer>();
        }
    }

    private void changeMaterial(Material material)
    {
        this.requireMeshRenderer();
        this.meshRenderer.material = material;
    }

    /// <summary>
    /// Toggle between selected and non-selected states.
    /// </summary>
    public void ToggleSelected()
    {
        this.state = !this.state;
        this.changeMaterial(this.state ? this.SelectedMaterial : this.StandardMaterial);
    }

    public void identify()
    {
        Debug.Log(this.x + " " + this.y + " ");
    }
}
