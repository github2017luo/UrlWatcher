using System;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.ServiceModel.Syndication;
using System.Xml;

namespace UrlWatcher
{
    internal static class RssChecker
    {
        private static DateTimeOffset _lastOffset;
        private static XmlReader reader;

        internal static void Refresh()
        {
            reader = XmlReader.Create("https://osu.ppy.sh/feed/ranked/");
            var feed = SyndicationFeed.Load(reader);
            if (feed.Items.Count() == 0)
            {
                Program.PrintError("SyndicationFeed item count is zero.");
                return;
            }

            var latestItem = feed.Items.First();
            var latestOffset = latestItem.PublishDate;
            if (_lastOffset != DateTimeOffset.MinValue
                && !_lastOffset.Equals(latestOffset))
            {
                var link = latestItem.Links.First().Uri.AbsoluteUri;
                Program.PrintSuccess("New feed found.");
                Program.FlashWindow();
                Process.Start(link);
                PlayAudio();
            }
            _lastOffset = latestOffset;
        }

        internal static void PlayAudio()
        {
            var stream = Properties.Resources.ApexVictory;
            var player = new SoundPlayer(stream);
            player.Play();
        }
    }
}
