using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private float xRange=0.5f;
    [SerializeField] private float yRange =1;
    [SerializeField] private int instanceCount = 100;
    [SerializeField] private GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        ground.transform.localScale = new Vector3(xRange*0.2f ,1, yRange*0.2f);
        GeneratePrefab();
    }

    private void GeneratePrefab()
    {
        for(int i = 0; i < instanceCount; i++)
        {
            float x = Random.Range(-xRange, xRange);
            float y = Random.Range(-yRange, yRange);
            Instantiate(prefab,new Vector3(x,0,y),Quaternion.identity,transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 topLeft = new Vector3(-xRange, 0, yRange);
        Vector3 topRight = new Vector3(xRange, 0, yRange);
        Vector3 bottomLeft = new Vector3(-xRange, 0, -yRange);
        Vector3 bottomRight = new Vector3(xRange, 0, -yRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(topLeft,topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);

    }
}
