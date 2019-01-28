using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;
    [Range(0f, 0.5f)]
    public float volumeVariance = 0.1f;
    [Range(0f, 0.5f)]
    public float pitchVariance = 0.1f;
    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + UnityEngine.Random.Range(-volumeVariance/2f, volumeVariance / 2f));
        source.pitch = pitch * (1 + UnityEngine.Random.Range(-pitchVariance / 2f, pitchVariance / 2f));
        source.Play();
    }
    
    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this) Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        PlaySound("Music");
    }

    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null) Debug.LogWarning("Audio Manager: Sound not found - " + name);
        else sound.Play();
    }

    public void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null) Debug.LogWarning("Audio Manager: Sound not found - " + name);
        else sound.Stop();
    }

}
