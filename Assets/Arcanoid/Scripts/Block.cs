using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    private BlockType _blockType = BlockType.Normal;

    public List<Material> materialType;


    public void SetType(int type)
    {

        transform.GetComponent<MeshRenderer>().material = materialType[type];

        switch (type)
        {
            case 0:
                _blockType = BlockType.Speed;

                break;
            case 1 :
                _blockType = BlockType.HalfCollision;

                break;
            case 2 :
                _blockType = BlockType.PointTop;

                break;
            case 3 :
                _blockType = BlockType.PointDown;
                break;
            case 4 :
                _blockType = BlockType.Normal;
                break;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            
            
            switch (_blockType)
            {
                case  BlockType.Speed:
                    other.transform.GetComponent<Ball>().move.y *= 1.1f;

                    break;
                case BlockType.HalfCollision :

                    break;
                case  BlockType.PointTop :

                    break;
                case BlockType.PointDown :
                    break;
                case BlockType.Normal :
                    break;
            }
            
            
            
            other.transform.GetComponent<Ball>().move.y *= -1;
            BlockGenerator.instance.RemoveBlockFromActive(this);
            //BlockGenerator.instance.RemoveBlockFromActive();
        }
        
    }
}
