using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundPrefabs;

    private void Awake()
    {
        GameObject newBGPrefab = backgroundPrefabs[Random.Range(0, backgroundPrefabs.Length)];
        Instantiate(newBGPrefab, transform);
    }
}
