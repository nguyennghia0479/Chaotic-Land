using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string entranceName;

    /// <summary>
    /// Handles to change scene when entered.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Player _)) return;

        if (LevelManager.Instance != null && AreaManager.Instance != null)
        {
            AreaManager.Instance.SetAreaEntranceName(entranceName);
            LevelManager.Instance.SaveGame();
            LevelManager.Instance.LoadScene(sceneToLoad);
        }
    }
}
