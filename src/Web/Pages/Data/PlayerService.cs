using Podcast.Pages.Models;

namespace Podcast.Pages.Data;

public class PlayerService
{
    public event Action<EpisodeInfo?>? EpisodeChanged;
    public event Action<bool>? PlayingChanged;
    public event Action<bool>? MutedChanged;
    public event Action<int>? VolumeChanged;
    public event Action<double?>? DurationChanged;
    public event Action<double?>? CurrentTimeChanged;
    public event Action<double?>? TimeSought;

    private EpisodeInfo? episode;
    public EpisodeInfo? Episode
    {
        get => episode;
        set
        {
            episode = value;
            EpisodeChanged?.Invoke(value);
        }
    }

    private bool isPlaying = false;
    public bool IsPlaying
    {
        get => isPlaying;
        set
        {
            isPlaying = value;
            PlayingChanged?.Invoke(value);
        }
    }

    private bool isMuted = false;
    public bool IsMuted
    {
        get => isMuted;
        set
        {
            isMuted = value;
            MutedChanged?.Invoke(value);
        }
    }

    private int volume = 50;
    public int Volume
    {
        get => volume;
        set
        {
            volume = value;
            VolumeChanged?.Invoke(value);
        }
    }

    private double? duration;
    public double? Duration
    {
        get => duration;
        set
        {
            duration = value;
            DurationChanged?.Invoke(value);
        }
    }

    private double? currentTime;
    public double? CurrentTime
    {
        get => currentTime;
        set
        {
            currentTime = value;
            CurrentTimeChanged?.Invoke(value);
        }
    }

    public void SeekTime(double time)
    {
        currentTime = time;
        TimeSought?.Invoke(time);
    }

    public void Play(EpisodeInfo episode)
    {
        Episode = episode;
        IsPlaying = true;
    }

    public void Resume()
    {
        IsPlaying = true;
    }

    public void Pause()
    {
        IsPlaying = false;
    }
}