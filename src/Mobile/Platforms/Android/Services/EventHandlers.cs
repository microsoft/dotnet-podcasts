using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.NetConf2021.Maui.Platforms.Android.Services
{
    public delegate void StatusChangedEventHandler(object sender, EventArgs e);

    public delegate void BufferingEventHandler(object sender, EventArgs e);

    public delegate void CoverReloadedEventHandler(object sender, EventArgs e);

    public delegate void PlayingEventHandler(object sender, EventArgs e);
}
