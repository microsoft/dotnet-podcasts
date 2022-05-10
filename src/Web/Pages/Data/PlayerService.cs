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
    public event Action<double?>? PlaybackRateChanged;

    private EpisodeInfo? _episode;
    public EpisodeInfo? Episode
    {
        get => _episode;
        set => EpisodeChanged?.Invoke(_episode = value);
    }

    private bool _isPlaying = false;
    public bool IsPlaying
    {
        get => _isPlaying;
        set => PlayingChanged?.Invoke(_isPlaying = value);
    }

    private bool _isMuted = false;
    public bool IsMuted
    {
        get => _isMuted;
        set => MutedChanged?.Invoke(_isMuted = value);
    }

    private int _volume = 50;
    public int Volume
    {
        get => _volume;
        set => VolumeChanged?.Invoke(_volume = value);
    }

    private double? _duration;
    public double? Duration
    {
        get => _duration;
        set => DurationChanged?.Invoke(_duration = value);
    }

    private double? _currentTime;
    public double? CurrentTime
    {
        get => _currentTime;
        set => CurrentTimeChanged?.Invoke(_currentTime = value);
    }

    private double? _playbackRate;
    public double PlaybackRate
    {
        get => _playbackRate ?? 1;
        set => PlaybackRateChanged?.Invoke(_playbackRate = value);
    }

    public void SeekTime(double time) => TimeSought?.Invoke(_currentTime = time);

    public void Play(EpisodeInfo episode)
    {
        Episode = episode;
        IsPlaying = true;
    }

    public void Resume() => IsPlaying = true;

    public void Pause() => IsPlaying = false;
}