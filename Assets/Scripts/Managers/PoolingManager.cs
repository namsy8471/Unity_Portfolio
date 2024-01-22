using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private GameObject[] prefabs;

    private List<GameObject>[] pools;
    // Start is called before the first frame update
    // void Awake()
    // {
    //     prefabs = new GameObject[] { GameManager.Instance.particle.mouseClickParticle };
    //     
    //     pools = new List<GameObject>[prefabs.Length];
    //
    //     for (int index = 0; index < pools.Length; index++)
    //     {
    //         pools[index] = new List<GameObject>();
    //     }
    //     
    //     Debug.Log(pools.Length);
    // }
    //
    // public GameObject Get(int index)
    // {
    //     GameObject select = null;
    //
    //     foreach (GameObject item in pools[index])
    //     {
    //         if (!item.activeSelf)
    //         {
    //             select = item;
    //             select.SetActive(true);
    //             break;
    //         }
    //     }
    //
    //     if (!select)
    //     {
    //         select = Instantiate(prefabs[index], transform);
    //         pools[index].Add(select);
    //     }
    //
    //     return select;
    // }
}
