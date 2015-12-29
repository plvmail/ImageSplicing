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
    public class Canvas
    {
        #region Data
        // color
        private Byte[,] m_arrayR = null;
        private Byte[,] m_arrayG = null;
        private Byte[,] m_arrayB = null;

        private int[,] m_arrayUnitR = null;
        private int[,] m_arrayUnitG = null;
        private int[,] m_arrayUnitB = null;

        // size
        private int m_width = 0;
        private int m_height = 0;

        // iamge
        private Bitmap m_imageData = null;
        private string m_imageName = "";
        #endregion

        #region Get& set
        public int[,] ArrayUnitR
        {
            get { return this.m_arrayUnitR; }
        }
        public int[,] ArrayUnitG
        {
            get { return this.m_arrayUnitG; }
        }
        public int[,] ArrayUnitB
        {
            get { return this.m_arrayUnitB; }
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

        #region Function
        public void LoadCanvas(string fileName)
        {

            // save file name
            this.m_imageName = fileName;

            // read image
            Bitmap orgImage = new Bitmap(fileName);

            // scale image
            m_imageData = new Bitmap(orgImage, Param.Canvas_Width, Param.Canvas_Height);

            this.m_width = this.m_imageData.Width;
            this.m_height = this.m_imageData.Height;

            // create store
            this.m_arrayR = new Byte[this.m_height, this.m_width];
            this.m_arrayG = new Byte[this.m_height, this.m_width];
            this.m_arrayB = new Byte[this.m_height, this.m_width];

            this.m_arrayUnitR = new int[Param.Unit_Count, Param.Unit_Count];
            this.m_arrayUnitG = new int[Param.Unit_Count, Param.Unit_Count];
            this.m_arrayUnitB = new int[Param.Unit_Count, Param.Unit_Count];

        }
        public void StoreCanvasData()
        {
            byte[] PixelValues = null;

            try
            {
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                // lock
                BitmapData bmpData = this.ImageData.LockBits(rect, 
                    System.Drawing.Imaging.ImageLockMode.ReadWrite, 
                    PixelFormat.Format24bppRgb);

                IntPtr iPtr = bmpData.Scan0;
                int iBytes = this.Width * this.Height * 3;
                PixelValues = new byte[iBytes];
                System.Runtime.InteropServices.Marshal.Copy(iPtr, PixelValues, 0, iBytes);

                // unlock
                this.ImageData.UnlockBits(bmpData);
                
            }
            catch (Exception e)
            {
                throw new Exception("Cannot Lock Canvas Data!" + e.Message);
            }

            try
            {
                int iPoint = 0;

                for (int i = 0; i < this.Height; i++)
                {
                    iPoint = i * this.Width * 3;
                    for (int j = 0; j < this.Width; j++)
                    {
                        
                        // B
                        this.m_arrayB[i, j] = PixelValues[iPoint];
                        ++iPoint;
                        // G
                        this.m_arrayG[i, j] = PixelValues[iPoint];
                        ++iPoint;
                        // R
                        this.m_arrayR[i, j] = PixelValues[iPoint];
                        ++iPoint;

                    }
                }

                return;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot Store Data to Canvas!" + e.Message);
            }
 
        }
        public void CalcAverageUnits()
        {

            // each unit image
            int unitWidth = this.Width / Param.Unit_Count;
            int unitHeight = this.Height / Param.Unit_Count;
            int unitTotal = unitWidth * unitHeight;

            try
            {
                // calc r,g,b on of each unit
                int r, g, b;
                for (int i = 0; i < Param.Unit_Count; ++i)
                {
                    for (int j = 0; j < Param.Unit_Count; ++j)
                    {
                        r = 0;
                        g = 0;
                        b = 0;
                        int baseWidth = j * unitWidth;
                        int baseHeight = i * unitHeight;
                        for (int k = 0; k < unitHeight; ++k)
                        {
                            for (int h = 0; h < unitWidth; ++h)
                            {
                                r += this.m_arrayR[baseHeight + k, baseWidth + h];
                                g += this.m_arrayG[baseHeight + k, baseWidth + h];
                                b += this.m_arrayB[baseHeight + k, baseWidth + h];                 
                            }
                        }
                        this.m_arrayUnitR[i, j] = r / unitTotal;
                        this.m_arrayUnitG[i, j] = g / unitTotal;
                        this.m_arrayUnitB[i, j] = b / unitTotal;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Cannot Calc Canvas Units!" + e.Message);
            }
        }
        #endregion
    }
}
