using System;
using System.Collections;
using System.Drawing;
using Ultima;

namespace Region_Editor
{
    public class Cache
    {
        private static Hashtable MapCache = new Hashtable();
        private static Hashtable TileCache = new Hashtable();

        public static Color GetColor(int id)
        {
            if (TileCache[id] != null)
                return ((TileInfo)TileCache[id]).Color;
            else
                return InitializeTile(id).Color;
        }

        public static Bitmap GetTile(int id, int scale)
        {
            if (TileCache[id] != null)
                return ((TileInfo)TileCache[id]).Images[scale];
            else
                return InitializeTile(id).Images[scale];
        }

        private static TileInfo InitializeTile(int id)
        {
            Bitmap bmp = Art.GetLand(id);

            if (bmp == null)
            {
                bmp = new Bitmap(44, 44);
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.Black);
                g.Dispose();
            }

            Bitmap[] images = new Bitmap[8];

            Color c = bmp.GetPixel(22, 22);

            images[0] = RotateTile(bmp, -45, 2, 2);
            images[1] = RotateTile(bmp, -45, 3, 3);
            images[2] = RotateTile(bmp, -45, 4, 4);
            images[3] = RotateTile(bmp, -45, 5, 5);
            images[4] = RotateTile(bmp, -45, 10, 10);
            images[5] = RotateTile(bmp, -45, 20, 20);
            images[6] = RotateTile(bmp, -45, 30, 30);

            TileInfo ti = new TileInfo(c, images);
            TileCache[id] = ti;

            bmp.Dispose();

            return ti;
        }

        private static Bitmap RotateTile(Bitmap b, float angle, int width, int height)
        {
            Bitmap bmp = new Bitmap(44, 44);
            Graphics g = Graphics.FromImage(bmp);

            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            g.DrawImage(b, 0, 0, 44, 44);
            g.Dispose();

            Bitmap newBmp = new Bitmap(width, height);
            g = Graphics.FromImage(newBmp);
            g.DrawImage(bmp, 0, 0, new RectangleF(6, 7, 30, 30), GraphicsUnit.Pixel);
            g.Dispose();

            return newBmp;
        }

        public struct TileInfo
        {
            private Color _Color;
            public Color Color { get { return _Color; } }

            private Bitmap[] _Images;
            public Bitmap[] Images { get { return _Images; } }

            public TileInfo(Color color, Bitmap[] images)
            {
                _Color = color;
                _Images = images;
            }
        }
    }
}