using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private AreaGate entranceName;

    /// <summary>
    /// Handles to set player position when load scene done.
    /// </summary>
    private void Start()
    {
        if (AreaManager.Instance != null && entranceName.ToString() == AreaManager.Instance.AreaSpawn)
        {
            if (PlayerManager.Instance != null || PlayerManager.Instance.Player)
            {
                PlayerManager.Instance.Player.transform.position = transform.position;
            }
        }
    }
}
