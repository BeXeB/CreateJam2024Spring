using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    #region Attributes
    public Sound[] sounds;
    
    private float soundVolume = 1f;
    private float musicVolume = 1f;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        foreach(Sound s in sounds)
        {
            
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.pitch = s.pitch;
            
            s.source.playOnAwake = s.playOnAwake;

            if(s.loopClip != null)
            {
                s.loopSource = gameObject.AddComponent<AudioSource>();

                s.loopSource.clip = s.loopClip;

                s.loopSource.pitch = s.pitch;

                s.loopSource.loop = s.isLooping;
            }
            else
            {
                s.source.loop = s.isLooping;
            }

            if(s.playOnAwake)
            {
                StartCoroutine(FadeInBehaviour(s, 1f));
            }
        }
        
        SetMasterVolume();
    }
    #endregion

    #region Normal Methods
    public void PlayAudioClip(string soundName, bool oneShot = true)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(soundName));

        if(s == null)
        {
            return;
        }
        
        if(s.isMusic)
        {
            s.source.volume = s.volume;
                              //* musicVolume;

            if(s.loopSource)
            {
                s.loopSource.volume = s.volume;
                //* musicVolume;
            }
        }
        else
        {
            s.source.volume = s.volume;
            //* soundVolume;
            
            if(s.loopSource)
            {
                s.loopSource.volume = s.volume;
                //* soundVolume;
            }
        }

        if(oneShot && s.oneShotInstances < s.oneShotInstancesMax)
        {
            s.oneShotInstances++;
            
            s.source.PlayOneShot(s.clip);

            StartCoroutine(OneShotTimer(s));
        }
        else if(!oneShot)
        {
            s.source.Play();
            
            if(s.loopClip)
            {
                s.loopSource.PlayDelayed(s.clip.length - 0.05f);
            }
        }
    }

    public void Stop(string soundName, bool fadeOut = false, float fadeOutDuration = 0.5f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
            
        if(s == null)
        {
            return;
        }

        if(fadeOut)
        {
            StartCoroutine(FadeOutBehaviour(s, fadeOutDuration));
        }
        else
        {
            s.source.Stop();
        }
    }

    public void StopAllSounds()
    {
        foreach(Sound sound in sounds)
        {
            if(IsPlaying(sound.name))
            {
                StartCoroutine(FadeOutBehaviour(sound, 0.5f));
            }
        }
    }

    public bool IsPlaying(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        
        if(!s.source)
        {
            return false;
        }

        if(s.loopSource)
        {
            return s.source.isPlaying || s.loopSource.isPlaying;
        }
        
        return s.source.isPlaying;
    }

    public void SetVolume(string soundName, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        
        if(s == null)
        {
            return;
        }
        
        s.source.volume = volume;

        if(s.loopSource)
        {
            s.loopSource.volume = volume;
        }
    }

    private void SetVolumeToAllSources()
    {
        foreach(Sound s in sounds)
        {
            if(s.source == null)
            {
                continue;
            }

            if(s.isMusic)
            {
                s.source.volume = s.volume * musicVolume;

                if(s.loopSource)
                {
                    s.loopSource.volume = s.volume * musicVolume;
                }
            }
            else
            {
                s.source.volume = s.volume * soundVolume;
            }
        }
    }

    public void SetMasterVolume()
    {
        if(!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        }
        else
        {
            soundVolume = PlayerPrefs.GetFloat("SoundVolume");
        }

        if(!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }
        else
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }
        
        if(!PlayerPrefs.HasKey("SoundMuted"))
        {
            PlayerPrefs.SetInt("SoundMuted", 0);
        }
        else if(PlayerPrefs.GetInt("SoundMuted") == 1)
        {
            soundVolume = 0f;
        }

        if(!PlayerPrefs.HasKey("MusicMuted"))
        {
            PlayerPrefs.SetInt("MusicMuted", 0);
        }
        else if(PlayerPrefs.GetInt("MusicMuted") == 1)
        {
            musicVolume = 0f;
        }
        
        SetVolumeToAllSources();
    }

    public float GetSoundVolume()
    {
        return soundVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
    #endregion

    #region Coroutines
    private IEnumerator FadeOutBehaviour(Sound s, float fadeOutDuration)
    {
        float fadeRate = 1f / fadeOutDuration / 50f;

        AudioSource source = s.source;

        if(s.loopSource && s.loopSource.isPlaying)
        {
            source = s.loopSource;
        }

        while(source.volume > 0f)
        {
            SetVolume(s.name, source.volume - fadeRate);
            
            yield return new WaitForSecondsRealtime(0.02f);
        }
        
        SetVolume(s.name, 0f);
        
        source.Stop();
    }
    
    private IEnumerator FadeInBehaviour(Sound s, float fadeInDuration)
    {
        yield return new WaitForEndOfFrame();

        float ogVolume;
        
        if(s.isMusic)
        {
            ogVolume = s.volume * musicVolume;
        }
        else
        {
            ogVolume = s.volume * soundVolume;
        }
        
        float fadeRate = 1f / fadeInDuration / 50f;

        s.source.volume = 1f;

        s.source.Play();

        if(s.loopClip)
        {
            s.loopSource.PlayDelayed(s.clip.length - 0.05f);
        }
        
        while(s.source.volume < ogVolume)
        {
            SetVolume(s.name, s.source.volume + fadeRate);

            yield return new WaitForSecondsRealtime(0.02f);
        }
        
        SetVolume(s.name, ogVolume);
    }

    private IEnumerator OneShotTimer(Sound s)
    {
        yield return new WaitForSecondsRealtime(s.clip.length);

        s.oneShotInstances--;
    }
    #endregion
}
[Serializable]
public class Sound
{
    #region Attributes
    public string name;

    public AudioSource source;
    public AudioSource loopSource;
    public AudioClip clip;
    public AudioClip loopClip;

    [Range(0f,1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch;

    public int oneShotInstances;
    public int oneShotInstancesMax = 3;

    public bool isLooping;
    public bool playOnAwake;
    public bool isMusic;
    #endregion
}
