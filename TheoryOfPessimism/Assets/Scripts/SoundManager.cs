using UnityEngine;
using System;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{

    private static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [Serializable]
    public sealed class SoundEffect
    {

        public string title
        {
            get
            {
                return _clip.name;
            }
        }

        public Sound _soundOption;
        public AudioClip _clip;
    }

    [Serializable]
    public sealed class MusicTrack
    {

        public string title
        {
            get
            {
                return _track.name;
            }
        }

        public Music _musicOption;
        public AudioClip _track;
    }

    [Header("Data")]

    [SerializeField]
    private SoundEffect[] _soundEffects = null;
    private Dictionary<Sound, AudioClip> _soundEffectsLookup;

    [SerializeField]
    private MusicTrack[] _musicTracks = null;
    private Dictionary<Music, AudioClip> _musicTracksLookup;

    private AudioSource[] _bgmSources = null;
    private int _bgmSourceIndex;
    private float _bgmStartCrossFadeTime;
    private float _bgmCrossFadeDuration;

    [SerializeField]
    private int _numSoundEffectSources;
    private AudioSource[] _fxSources = null;
    private int _fxSourceIndex;

    private Music _currentMusic = Music.None;

    public SoundEffect[] SoundEffects
    {
        get
        {
            return _soundEffects;
        }
    }

    public MusicTrack[] MusicTracks
    {
        get
        {
            return _musicTracks;
        }
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Create BGM sources
            _bgmSourceIndex = 0;
            _bgmSources = new AudioSource[2] {
                gameObject.AddComponent<AudioSource>(),
                gameObject.AddComponent<AudioSource>()
            };

            _bgmSources[0].playOnAwake = _bgmSources[1].playOnAwake = false;
            _bgmSources[0].loop = _bgmSources[1].loop = true;

            // Create FX sources
            _fxSourceIndex = 0;
            _fxSources = new AudioSource[_numSoundEffectSources];
            for (int i = 0; i < _numSoundEffectSources; i++)
            {
                _fxSources[i] = gameObject.AddComponent<AudioSource>();
                _fxSources[i].playOnAwake = false;
            }

            // Load sound effects lookup
            _soundEffectsLookup = new Dictionary<Sound, AudioClip>();
            for (int i = 0; i < _soundEffects.Length; i++)
            {
                _soundEffectsLookup.Add(_soundEffects[i]._soundOption, _soundEffects[i]._clip);
            }

            // Load music tracks lookup
            _musicTracksLookup = new Dictionary<Music, AudioClip>();
            for (int i = 0; i < _musicTracks.Length; i++)
            {
                _musicTracksLookup.Add(_musicTracks[i]._musicOption, _musicTracks[i]._track);
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(Music track)
    {
        if (_currentMusic == track)
        {
            return;
        }

        _currentMusic = track;

        if (_bgmSources[_bgmSourceIndex].isPlaying)
        {
            _bgmSources[_bgmSourceIndex].Stop();
        }

        playMusicOnNextSource(track);
    }

    public void PlayMusicCrossFaded(Music track, float crossFadeTime)
    {
        if (_currentMusic == track)
        {
            return;
        }

        _currentMusic = track;

        playMusicOnNextSource(track);

        _bgmStartCrossFadeTime = Time.time;
        _bgmCrossFadeDuration = crossFadeTime;
    }

    public void PlaySound(Sound sound)
    {
        if (++_fxSourceIndex == _numSoundEffectSources)
        {
            _fxSourceIndex = 0;
        }

        _fxSources[_fxSourceIndex].PlayOneShot(_soundEffectsLookup[sound]);
    }

    private void playMusicOnNextSource(Music track)
    {
        _bgmSourceIndex = 1 - _bgmSourceIndex;
        _bgmSources[_bgmSourceIndex].clip = _musicTracksLookup[track];
        _bgmSources[_bgmSourceIndex].Play();
    }

    private void Update()
    {
        if (_bgmStartCrossFadeTime > 0f)
        {
            float volumeNew = Mathf.Clamp01((Time.time - _bgmStartCrossFadeTime) / _bgmCrossFadeDuration);
            _bgmSources[_bgmSourceIndex].volume = volumeNew;
            _bgmSources[1 - _bgmSourceIndex].volume = 1f - volumeNew;
            if (volumeNew == 1f)
            {
                _bgmStartCrossFadeTime = 0f;
                _bgmSources[1 - _bgmSourceIndex].Stop();
            }
        }
    }
}
