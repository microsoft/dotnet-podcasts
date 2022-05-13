namespace SharedMauiLib.Platforms.Android
{
    public interface IAudioActivity
    {
        public MediaPlayerServiceBinder Binder { get; set; }

        public event StatusChangedEventHandler StatusChanged;

        public event CoverReloadedEventHandler CoverReloaded;

        public event PlayingEventHandler Playing;

        public event BufferingEventHandler Buffering;
    }
}
