using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public enum ImageFormat
    {
        GrayScale,
        RGB
    }

    public class DataSourceToBitmap<T>
    {
        private static Byte Scale(T value)
        {
            return (Byte)(Convert.ToSingle(value) * 255);
        }

        public static Bitmap Do(DataSource<T> dataSource, ImageFormat imageFormat, bool scale)
        {
            var width = dataSource.Shape[1];
            var height = dataSource.Shape[2];

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(Point.Empty, bitmap.Size),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var t = dataSource.Data;
            unsafe {
                byte* p = (byte*)bitmapData.Scan0;
                int dataLength = width * height;

                switch (imageFormat)
                {
                    case ImageFormat.GrayScale:
                        for (int i = 0; i < dataLength; ++i)
                        {
                            Byte value;
                            if (scale)
                                value = Scale(t[i]);
                            else
                                value = Convert.ToByte(t[i]);

                            *p++ = value; // B
                            *p++ = value; // G
                            *p++ = value; // R
                            *p++ = 255;   // A
                        }
                        break;

                    case ImageFormat.RGB:
                        for (int i = 0; i < dataLength; ++i)
                        {
                            if (scale)
                            {
                                *p++ = Scale(t[i * 3 + 2]);
                                *p++ = Scale(t[i * 3 + 1]);
                                *p++ = Scale(t[i * 3]);
                            }
                            else
                            {
                                *p++ = Convert.ToByte(t[i * 3 + 2]);
                                *p++ = Convert.ToByte(t[i * 3 + 1]);
                                *p++ = Convert.ToByte(t[i * 3]);
                            }
                            *p++ = 255;
                        }
                        break;
                }
            }
 
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
