using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioSource[] bossTracks;
    [SerializeField] private AudioSource[] normalTracks;

    private int currentTrack;
    private Coroutine musicCoroutine;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayRandomNormalMusic();
        musicCoroutine = StartCoroutine(PlayMusicRoutine());
    }

    public void TooglePauseMusic()
    {
        if (normalTracks[currentTrack].isPlaying)
        {
            StartCoroutine(ChangeMusicRoutine(normalTracks[currentTrack], true));
        }
        else
        {
            StartCoroutine(ChangeMusicRoutine(bossTracks[currentTrack], false));
        }
    }

    private void PlayBossBattleMusic()
    {
        currentTrack = Random.Range(0, bossTracks.Length);
        bossTracks[currentTrack].loop = true;
        bossTracks[currentTrack].Play();
    }

    private void PlayRandomNormalMusic()
    {
        foreach (var track in normalTracks)
        {
            track.Stop();
        }

        currentTrack = Random.Range(0, normalTracks.Length);
        normalTracks[currentTrack].Play();
    }

    private IEnumerator PlayMusicRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(normalTracks[currentTrack].clip.length);
            PlayRandomNormalMusic();
        }
    }

    private IEnumerator ChangeMusicRoutine(AudioSource _audioSource, bool _isBossBattle)
    {
        float defaultVolume = _audioSource.volume;

        while (_audioSource.volume > .1f)
        {
            _audioSource.volume -= .1f;
            yield return new WaitForSeconds(.2f);

            if (_audioSource.volume <= .1f)
            {
                _audioSource.Stop();
                _audioSource.volume = defaultVolume;

                if (_isBossBattle)
                {
                    StopCoroutine(musicCoroutine);
                    PlayBossBattleMusic();
                }
                else
                {
                    PlayRandomNormalMusic();
                    musicCoroutine = StartCoroutine(PlayMusicRoutine());
                }

                break;
            }
        }
    }
}
