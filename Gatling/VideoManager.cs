using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatling
{
    class VideoManager
    {
        Process process;

        public void TakeScreen(string path)
        {
            using (var engine = new Engine())
            {
                var mp4 = new MediaFile { Filename = path };

                engine.GetMetadata(mp4);

                //var i = 0;
                //while (i < mp4.Metadata.Duration.Seconds)
                //{
                    var options = new ConversionOptions { Seek = TimeSpan.FromMilliseconds(1)};
                    var outputFile = new MediaFile { Filename = string.Format("{0}\\image-shot.jpeg", @"C:\Users\Ya-Allah\Pictures\TEST") };
                    engine.GetThumbnail(mp4, outputFile, options);

               // }
            }
        }

        public bool TrueSize(string path)
        {
            var inputFile = new MediaFile { Filename = path };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
            }

            Debug.WriteLine(inputFile.Metadata.VideoData.FrameSize);
            return true;
        }
    }
}
