using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SoundManagerMenu : MonoBehaviour
{
    public static SoundManagerMenu Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip hoverSFX;
    public AudioClip clickSFX;
    public AudioClip destroyCandySFX;

    [Header("Audio Clips de Daño")]
    public AudioClip playerDamageSFX;
    public AudioClip enemyDamageSFX;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;

    [Header("Scene Music")]
    public List<SceneMusic> sceneMusicList;

    [Header("Sonidos por Tag")]
    public List<TagSound> tagSounds;

    private Dictionary<string, AudioClip> tagToSound = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sceneMusicMap = new Dictionary<string, AudioClip>();
    private AudioClip currentMusicClip = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (musicSource != null) musicSource.volume = musicVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;

        foreach (var sceneMusic in sceneMusicList)
        {
            sceneMusicMap[sceneMusic.sceneName] = sceneMusic.musicClip;
        }

        foreach (var tagSound in tagSounds)
        {
            if (!tagToSound.ContainsKey(tagSound.tag) && tagSound.sound != null)
            {
                tagToSound.Add(tagSound.tag, tagSound.sound);
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null && clip != null)
        {
            if (currentMusicClip != clip)
                StartCoroutine(CrossFadeMusic(clip));
            else
            {
                musicSource.clip = clip;
                musicSource.loop = loop;
                musicSource.Play();
            }
            currentMusicClip = clip;
        }
    }

    public void StopMusic() => musicSource?.Stop();

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    public void StopSFX() => sfxSource?.Stop();

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneMusicMap.ContainsKey(scene.name))
        {
            AudioClip musicClip = sceneMusicMap[scene.name];
            PlayMusic(musicClip);
        }
    }

    private IEnumerator CrossFadeMusic(AudioClip newClip)
    {
        float fadeDuration = 1f;
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.volume = startVolume;
        musicSource.Play();

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void CheckCollision(GameObject other)
    {
        string tag = other.tag;
        if (tagToSound.TryGetValue(tag, out AudioClip clip))
        {
            PlaySFX(clip);
        }
    }

    public void PlayPlayerDamageSFX()
    {
        PlaySFX(playerDamageSFX);
    }

    public void PlayEnemyDamageSFX()
    {
        PlaySFX(enemyDamageSFX);
    }

    [System.Serializable]
    public struct SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    [System.Serializable]
    public struct TagSound
    {
        public string tag;
        public AudioClip sound;
    }
}