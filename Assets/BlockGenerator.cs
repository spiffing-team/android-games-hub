using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockGenerator : MonoBehaviour
{
    public static BlockGenerator instance;
    [SerializeField] private Block block;

    public List<Block> grid = new List<Block>();
    public List<Block> activeGrid = new List<Block>();
    public List<Block> deactiveGrid = new List<Block>();

    public int startBlocks = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        var startpos = transform.position;
        var position = transform.position;
        
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
               var element =  Instantiate( block, position, Quaternion.identity, transform);
               position += Vector3.right *0.8f;
               
               grid.Add(element);
               activeGrid.Add(element);
            }
            position = startpos;
            position += Vector3.down * 0.5f * i;
        }
        Debug.Log("ile weszlo w counta " + grid.Count );

        RemoveExtraBlocks();
    }

    private void RemoveExtraBlocks()
    {
        var x = grid.Count;
        Debug.Log("FOR ZROBI TYLE OKRAZEN" +  x);

        for (int i = 0; i < x - startBlocks; i++)
        {
            var id = Random.Range(0, activeGrid.Count);
            RemoveBlockFromActive(activeGrid[id]);
        }
    }

    public void RemoveBlockFromActive(Block block)
    {
       deactiveGrid.Add(block);
       activeGrid.Remove(block);
       block.gameObject.SetActive(false);
    }

    public void ActivateBlock()
    {
        if (deactiveGrid.Count > 0)
        {
            var index = Random.Range(0, deactiveGrid.Count - 1);
            deactiveGrid[index].gameObject.SetActive(true);
            deactiveGrid.RemoveAt(index);
        }
    }
}
