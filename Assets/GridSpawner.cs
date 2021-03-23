using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private int m_width = 50;
    [SerializeField] private int m_height = 50;
    [SerializeField] private float m_gridOffset = 1.25f;
    [SerializeField] private GameObject m_cubePrefab;


    [SerializeField] private int m_seed;

    [SerializeField] private bool m_generateOnEnable;

    void OnEnable()
    {
        if (m_generateOnEnable)
        {
            Generate();
        }
    }

    public void Generate() 
    {
        if (m_seed == 0)
            m_seed = UnityEngine.Random.Range(10000, 100000);
        SpawnGrid();
        
    } 

    private void SpawnGrid()
    {
        for (int x = 0; x < m_width; x++)
        {
            for (int z = 0; z < m_height; z++)
            {
                var y = GetYCoordinate(x, z);

                Instantiate(m_cubePrefab, new Vector3(m_gridOffset * x, y, m_gridOffset * z), transform.rotation, transform);
            }
        }
    }

    private float GetYCoordinate(int x, int z)
    {
        return Mathf.PerlinNoise(x * 0.3f, z * 0.3f);
        // return Mathf.PerlinNoise(x * 0.3f + m_seed, z * 0.3f + m_seed);
    }
}
