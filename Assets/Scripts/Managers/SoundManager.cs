using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource[] menus;
    [SerializeField] private AudioSource[] upgrades;
    [SerializeField] private AudioSource[] attributes;
    [SerializeField] private AudioSource[] denies;
    [SerializeField] private AudioSource[] equips;
    [SerializeField] private AudioSource[] unequips;
    [SerializeField] private AudioSource[] openMaps;
    [SerializeField] private AudioSource[] drops;
    [SerializeField] private AudioSource[] pickups;
    [SerializeField] private AudioSource[] attacks;
    [SerializeField] private AudioSource[] hitAttacks;
    [SerializeField] private AudioSource[] missAttacks;
    [SerializeField] private AudioSource[] jumps;
    [SerializeField] private AudioSource[] jumpLands;
    [SerializeField] private AudioSource[] dashes;
    [SerializeField] private AudioSource[] footsteps;
    [SerializeField] private AudioSource[] potions;
    [SerializeField] private AudioSource[] throwSwords;
    [SerializeField] private AudioSource[] catchSwords;
    [SerializeField] private AudioSource[] spinSwords;
    [SerializeField] private AudioSource[] crystalExplosions;
    [SerializeField] private AudioSource[] crystalHits;
    [SerializeField] private AudioSource[] blocks;
    [SerializeField] private AudioSource[] parries;
    [SerializeField] private AudioSource[] iceDrills;
    [SerializeField] private AudioSource[] iceDrillHits;
    [SerializeField] private AudioSource[] iceBerges;
    [SerializeField] private AudioSource[] fireSpins;
    [SerializeField] private AudioSource[] fireLoops;
    [SerializeField] private AudioSource[] brightFires;
    [SerializeField] private AudioSource[] thunders;
    [SerializeField] private AudioSource[] lightTorches;
    [SerializeField] private AudioSource[] pains;
    [SerializeField] private AudioSource[] deaths;

    private AudioSource audioSource;
    private readonly float menuVolume = .1f;
    private readonly float distanceToHear = 10f;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public void PlayMenuSound()
    {
        PlaySound(menus, Camera.main.transform.position, menuVolume);
    }

    public void PlayUpgradeSound()
    {
        PlaySound(upgrades, Camera.main.transform.position, menuVolume);
    }

    public void PlayAttributeSound()
    {
        PlaySound(attributes, Camera.main.transform.position, menuVolume);
    }

    public void PlayDenySound()
    {
        PlaySound(denies, Camera.main.transform.position, menuVolume);
    }

    public void PlayOpenPanelSound()
    {
        PlaySound(openMaps, Camera.main.transform.position, menuVolume);
    }

    public void PlayEquipItemSound()
    {
        PlaySound(equips, Camera.main.transform.position, menuVolume);
    }

    public void PlayUnequipItemSound()
    {
        PlaySound(unequips, Camera.main.transform.position, menuVolume);
    }

    public void PlayDropItemSound()
    {
        PlaySound(drops, Camera.main.transform.position, menuVolume);
    }

    public void PlayCraftItemSound()
    {
        PlaySound(pickups, Camera.main.transform.position, menuVolume);
    }

    public void PlayPickupItemSound(Vector3 _position)
    {
        PlaySound(pickups, _position);
    }

    public void PlayAttackSound(Vector3 _position)
    {
        PlaySound(attacks, _position);
    }

    public void PlayHitAttackSound(Vector3 _position)
    {
        PlaySound(hitAttacks, _position);
    }

    public void PlayMissAttackSound(Vector3 _position)
    {
        PlaySound(missAttacks, _position);
    }

    public void PlayJumpSound(Vector3 _position)
    {
        PlaySound(jumps, _position);
    }

    public void PlayJumpLandSound(Vector3 _position)
    {
        PlaySound(jumpLands, _position);
    }

    public void PlayDashSound(Vector3 _position)
    {
        PlaySound(dashes, _position);
    }

    public void PlayFootstepSound(Vector3 _position)
    {
        if (CanHearSound(_position))
        {
            PlaySound(footsteps, _position);
        }
    }

    public void PlayUsePotionSound(Vector3 _position)
    {
        PlaySound(potions, _position);
    }

    public void PlayThrowSwordSound(Vector3 _position)
    {
        PlaySound(throwSwords, _position);
    }

    public void PlayCatchSwordSound(Vector3 _position)
    {
        PlaySound(catchSwords, _position);
    }

    public void PlaySpinSwordSound(Vector3 _position)
    {
        PlaySound(spinSwords, _position);
    }

    public void PlayCrystalExplosionSound(Vector3 _position)
    {
        PlaySound(crystalExplosions, _position);
    }

    public void PlayCrystalHitSound(Vector3 _position)
    {
        PlaySound(crystalHits, _position);
    }

    public void PlayBlockSound(Vector3 _position)
    {
        PlaySound(blocks, _position);
    }

    public void PlayParrySound(Vector3 _position)
    {
        PlaySound(parries, _position);
    }

    public void PlayIceDrillSound(Vector3 _position)
    {
        PlaySound(iceDrills, _position);
    }

    public void PlayIceDrillHitSound(Vector3 _position)
    {
        PlaySound(iceDrillHits, _position);
    }

    public void PlayIceBergeSound(Vector3 _position)
    {
        PlaySound(iceBerges, _position);
    }

    public void PlayFireSpinSound(Vector3 _position)
    {
        PlaySound(fireSpins, _position);
    }

    public void PlayFireLoopSound(Vector3 _position)
    {
        PlaySound(fireLoops, _position);
    }

    public void PlayBrightFireSound(Vector3 _position)
    {
        PlaySound(brightFires, _position);
    }

    public void PlayThunderSound(Vector3 _position)
    {
        PlaySound(thunders, _position);
    }

    public void PlayLightTorchSound(Vector3 _position)
    {
        PlaySound(lightTorches, _position);
    }

    public void PlayPainSound(Vector3 _position)
    {
        PlaySound(pains, _position);
    }

    public void PlayDeathSound(Vector3 _position)
    {
        PlaySound(deaths, _position);
    }

    public void StopPlaySound(AudioSource _audioSource)
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }

    private void PlaySound(AudioSource[] _audioSources, Vector3 _position, float _volume = 1f)
    {
        if (_audioSources == null || _audioSources.Length <= 0)
        {
            Debug.LogWarning("Audio source is null or empty");
            return;
        }

        audioSource = _audioSources[Random.Range(0, _audioSources.Length)];
        audioSource.transform.position = _position;
        audioSource.spatialBlend = 1;
        audioSource.volume = _volume;
        audioSource.Play();
    }

    private bool CanHearSound(Vector3 _position)
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.Player == null) return false;

        Vector3 playerPos = PlayerManager.Instance.Player.transform.position;

        return Vector3.Distance(playerPos, _position) < distanceToHear;
    }

    public AudioSource AudioSource
    {
        get { return audioSource; }
    }
}
