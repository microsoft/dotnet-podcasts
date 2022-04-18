using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPodsMauiBlazor.Services
{
    internal interface INativeAudioService
    {
        Task InitializeAsync(string audioURI);
        Task PlayAsync(double position = 0);
        Task PauseAsync();
        bool IsPlaying { get; }
        double CurrentPosition { get; }
    }
}
