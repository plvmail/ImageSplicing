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
    public class ImageUnit
    {
        #region Data

        // color
        private float m_R = 0;
        private float m_G = 0;
        private float m_B = 0;

        // size
        private int m_width = 0;
        private int m_height = 0;

        // iamge
        private Bitmap m_imageData = null;
        private string m_imageName = "";

        #endregion

        #region Get & Set
        public float R
        {
            get { return this.m_R; }
        }
        public float G
        {
            get { return this.m_G; }
        }
        public float B
        {
            get { return this.m_B; }
        }

        public int Width
        {
            get { return this.m_width; }
        }
        public int Height
        {
            get { return this.m_height; }
        }

        public Bitmap ImageData
        {
            get { return this.m_imageData; }
        }
        public string ImageName
        {
            get { return this.m_imageName; }
        }

        #endregion

        #region Construction
        #endregion

        #region Function

        public void LoadImage(string fileName)
        {
            try
            {
                // save file name
                this.m_imageName = fileName;

                // read image
                StaticData.bitmap  = new Bitmap(fileName);

                // scale image
                m_imageData = new Bitmap(StaticData.bitmap, Param.Image_Width, Param.Image_Height);

                StaticData.bitmap.Dispose();

            }
            catch (Exception e)
            {
                throw new Exception("Cannot load image unit!" + e.Message);
            }

            this.m_width = this.m_imageData.Width;
            this.m_height = this.m_imageData.Height;
        }
        public void CalcImageTone()
        {
            try
            {
                int width = this.ImageData.Width;
                int height = this.ImageData.Height;
                Rectangle rect = new Rectangle(0, 0, width, height);

                // lock
                BitmapData bmpData = this.ImageData.LockBits(rect, 
                    System.Drawing.Imaging.ImageLockMode.ReadWrite, 
                    PixelFormat.Format24bppRgb);
                
                IntPtr iPtr = bmpData.Scan0;
                int iBytes = width * height * 3;
                System.Runtime.InteropServices.Marshal.Copy(iPtr, StaticData.ImageUnitBitmapData, 0, iBytes);

                // unlock
                this.ImageData.UnlockBits(bmpData);

                float r, g, b;

                r = 0;
                g = 0;
                b = 0;

                Color c;

                int iPoint = 0;
                for (int i = 0; i < height; i++)
                {
                    iPoint = i * width * 3;
                    for (int j = 0; j < width; j++)
                    {
                        b += StaticData.ImageUnitBitmapData[iPoint];
                        ++iPoint;
                        g += StaticData.ImageUnitBitmapData[iPoint];
                        ++iPoint;
                        r += StaticData.ImageUnitBitmapData[iPoint];
                    }
                }

                // weighted sum
                int totalPixel = Param.Image_Width * Param.Image_Height;
                this.m_R = r / totalPixel;
                this.m_G = g / totalPixel;
                this.m_B = b / totalPixel;
                
                return ;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot Calc Image Tones successful!" + e.Message);
            }
            return;
        }
        public void DrawImage(Graphics g, int startX, int startY, int drawWidth, int drawHeight)
        {
            if (this.m_imageData == null)
                return;

            g.DrawImage(this.m_imageData, startX, startY, drawWidth, drawHeight);
            return;
        }
        #endregion
    }
}
