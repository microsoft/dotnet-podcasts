using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using static Android.App.Notification;
using static Android.Resource;
using AndroidMedia = Android.Media;


namespace SharedMauiLib.Platforms.Android;

public static class NotificationHelper
{
    public static readonly string CHANNEL_ID = "location_notification";
    private const int NotificationId = 1000;

    internal static Notification.Action GenerateActionCompat(Context context, int icon, string title, string intentAction)
    {
        Intent intent = new Intent(context, typeof(MediaPlayerService));
        intent.SetAction(intentAction);

        PendingIntentFlags flags = PendingIntentFlags.UpdateCurrent;
        if (intentAction.Equals(MediaPlayerService.ActionStop))
            flags = PendingIntentFlags.CancelCurrent;

        flags |= PendingIntentFlags.Mutable;

        PendingIntent pendingIntent = PendingIntent.GetService(context, 1, intent, flags);

        return new Notification.Action.Builder(icon, title, pendingIntent).Build();
    }

    internal static void StopNotification(Context context)
    {
        NotificationManagerCompat nm = NotificationManagerCompat.From(context);
        nm.CancelAll();
    }

    public static void CreateNotificationChannel(Context context)
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        {
            // Notification channels are new in API 26 (and not a part of the
            // support library). There is no need to create a notification
            // channel on older versions of Android.
            return;
        }

        var name = "Local Notifications";
        var description = "The count from MainActivity.";
        var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
        {
            Description = description
        };

        var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
        notificationManager.CreateNotificationChannel(channel);
    }

    internal static void StartNotification(
        Context context,
        MediaMetadata mediaMetadata,
        AndroidMedia.Session.MediaSession mediaSession,
        object largeIcon,
        bool isPlaying)
    {
        var pendingIntent = PendingIntent.GetActivity(
            context,
            0,
            new Intent(context, typeof(Activity)),
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable);
        MediaMetadata currentTrack = mediaMetadata;

        MediaStyle style = new MediaStyle();
        style.SetMediaSession(mediaSession.SessionToken);

        var builder = new Builder(context, CHANNEL_ID)
            .SetStyle(style)
            .SetContentTitle(currentTrack.GetString(MediaMetadata.MetadataKeyTitle))
            .SetContentText(currentTrack.GetString(MediaMetadata.MetadataKeyArtist))
            .SetSubText(currentTrack.GetString(MediaMetadata.MetadataKeyAlbum))
            .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha) //TODO player_play
            .SetLargeIcon(largeIcon as Bitmap)
            .SetContentIntent(pendingIntent)
            .SetShowWhen(false)
            .SetOngoing(isPlaying)
            .SetVisibility(NotificationVisibility.Public);

        builder.AddAction(GenerateActionCompat(context, Drawable.IcMediaPrevious, "Previous", MediaPlayerService.ActionPrevious));
        AddPlayPauseActionCompat(builder, context, isPlaying);
        builder.AddAction(GenerateActionCompat(context, Drawable.IcMediaNext, "Next", MediaPlayerService.ActionNext));
        style.SetShowActionsInCompactView(0, 1, 2);

        NotificationManagerCompat.From(context).Notify(NotificationId, builder.Build());
    }

    private static void AddPlayPauseActionCompat(
        Builder builder,
        Context context,
        bool isPlaying)
    {
        if (isPlaying)
            builder.AddAction(GenerateActionCompat(context, Drawable.IcMediaPause, "Pause", MediaPlayerService.ActionPause));
        else
            builder.AddAction(GenerateActionCompat(context, Drawable.IcMediaPlay, "Play", MediaPlayerService.ActionPlay));
    }
}
