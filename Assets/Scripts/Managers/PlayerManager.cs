using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private Player player;

    protected override void Awake()
    {
        base.Awake();
    }

    public Player Player { get { return player; } }
}
