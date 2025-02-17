using Godot;
using System;
using System.Collections.Generic;

public class Sound
{
	private List<AudioStreamPlayer> _audioPlayers = new List<AudioStreamPlayer>();
	private AudioStreamPlayer _music = new AudioStreamPlayer();
	private string _soundPath = "res://Data/Sounds/Effects/";
	private string _musicPath = "res://Data/Sounds/Musics/";
	private Random _random = new Random();

    public Sound()
	{
		_music.Bus = "Music";
		for (int i = 0; i < 15; i++)
		{
            _audioPlayers.Add(new AudioStreamPlayer());
			_audioPlayers[i].Bus = "Sound";
        }
	}

	public void PlaySound(AudioStreamPlayer streamPlayer, string sound)
	{
		streamPlayer.Bus = "Sound";
		streamPlayer.Stream = ResourceLoader.Load<AudioStream>(_soundPath + sound);
		streamPlayer.Play();
	}

    public void PlaySound(AudioStreamPlayer streamPlayer, string sound, float diffusion)
    {
		streamPlayer.PitchScale = 1 + _random.NextSingle() * (diffusion * 2) - diffusion;
		PlaySound(streamPlayer, sound);
    }

    public void PlaySound(string sound) =>
        PlaySound(_audioPlayers.Find(x => !x.Playing), sound);

    public void PlaySound(string sound, float diffusion) =>
        PlaySound(_audioPlayers.Find(x => !x.Playing), sound, diffusion);

    public void PlayMusic(string music)
    {
        _music.Stream = ResourceLoader.Load<AudioStream>(_musicPath + music);
        _music.Play();
    }
}
