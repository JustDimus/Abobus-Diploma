using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.Helpers
{
    public class ImageHelper
    {
        public static ImageSource Create(Stream sourceStream)
            => ImageSource.FromStream(() =>
            {
                var newMemoryStream = new MemoryStream();

                sourceStream.Seek(0, SeekOrigin.Begin);
                sourceStream.CopyTo(newMemoryStream);

                newMemoryStream.Seek(0, SeekOrigin.Begin);

                return newMemoryStream;
            });
    }
}
