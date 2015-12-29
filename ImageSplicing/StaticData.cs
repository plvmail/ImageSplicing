using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ImageSplicing
{
    public static class StaticData
    {
        #region Data
        public static int[] ArrayR = new int[16];
        public static int[] ArrayG = new int[16];
        public static int[] ArrayB = new int[16];

        public static byte[] ImageUnitBitmapData = null;

        public static Bitmap bitmap = null;
        #endregion

        #region Function
        public static void Init()
        {
            try
            {
                ImageUnitBitmapData = new byte[Param.Image_Height * Param.Image_Width * 3];
            }
            catch (Exception ex)
            {
                throw new Exception("Static Data Init Failed!" + ex.Message);
            }
        }
        public static void InitRGBArray()
        {
            for (int i = 0; i < 16; ++i)
            {
                ArrayR[i] = 0;
                ArrayG[i] = 0;
                ArrayB[i] = 0;
            }
            return;
        }
        public static void RGBtoHSV(byte r, byte g, byte b, ref float h, ref float s, ref float v)
        {
            byte min, max;
            float delta;

            min = r > g ? g : r;
            min = min > b ? b : min;
            max = r > g ? r : g;
            max = max > b ? max : b;

            v = max; // v
            delta = max - min;
            if( max != 0 )
            {
                s = delta / max; // s
            }
            else
            {
                s = 0;
                h = -1;
                return;
            }
            if( r == max )
            {
                h = ( g - b ) / delta; // between yellow & magenta
            }
            else if( g == max )
            {
                h = 2 + ( b - r ) / delta; // between cyan & yellow
            }
            else
            {
                h = 4 + ( r - g ) / delta; // between magenta & cyan
            }
            
            h *= 60; // degrees
            
            if( h < 0 )
            {
                h += 360;
            }
        }
        #endregion
    }
}
