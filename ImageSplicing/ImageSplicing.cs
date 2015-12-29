using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace ImageSplicing
{
    public class ImageSplicing
    {
        #region Data
        private List<ImageUnit> m_listImageUnit = new List<ImageUnit>();
        private Canvas m_canvas = new Canvas();
        private ImageUnit[,] m_listMatchedUnit = new ImageUnit[Param.Unit_Count, Param.Unit_Count];
        
        public Form1 form = null;
        #endregion

        #region Get & Set
        public List<ImageUnit> ListImageUnit
        {
            get { return this.m_listImageUnit; }
        }
        public ImageUnit[,] ListMatchedUnit
        {
            get { return this.m_listMatchedUnit; }
        }
        public Canvas Canvas
        {
            get { return this.m_canvas; }
        }
        #endregion

        #region Function
        public void LoadImageUnits(string folder)
        {
            string fileName = "";

            try
            {
                DirectoryInfo TheFolder = new DirectoryInfo(folder);

                // clear
                this.m_listImageUnit.Clear();

                // progress bar
                int nCount = TheFolder.GetFiles().Count();
                this.form.SetMaxValueOfProgressBar(nCount);

                int ind = 0;
                int border = 0;

                // for each
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    fileName = folder + "\\" + NextFile.Name;

                    // create image
                    ImageUnit image = new ImageUnit();
                    image.LoadImage(fileName);

                    // calc image tone
                    image.CalcImageTone();

                    // add
                    this.m_listImageUnit.Add(image);

                    // update progress bar
                    ++ind;
                    this.form.UpdateProgressBar(ind);

                    // memory collect
                    ++border;
                    if (border >= 50)
                    {
                        GC.Collect();
                        border = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Load Image Units failed [" + fileName + "]" + ex.Message);
            }

        }
        public void LoadCanvas(string fileName)
        {
            try
            {
                this.form.SetMaxValueOfProgressBar(3);

                // load
                this.m_canvas.LoadCanvas(fileName);
                this.form.UpdateProgressBar(1);

                // init canvas
                this.m_canvas.StoreCanvasData();
                this.form.UpdateProgressBar(2);

                // average units
                this.m_canvas.CalcAverageUnits();
                this.form.UpdateProgressBar(3);
            }
            catch (Exception ex)
            {
                throw new Exception("Load Canvas failed [" + fileName + "]" + ex.Message);
            }
        }
        public void MatchAllUnits()
        {
            // progressbar
            int curValue = 0;
            this.form.SetMaxValueOfProgressBar(Param.Unit_Count * Param.Unit_Count);

            try
            {
                for (int i = 0; i < Param.Unit_Count; ++i)
                {
                    for (int j = 0; j < Param.Unit_Count; ++j)
                    {
                        // match each
                        this.m_listMatchedUnit[i, j] = this.MatchEachUnit(
                            this.Canvas.ArrayUnitR[i, j],
                            this.Canvas.ArrayUnitG[i, j],
                            this.Canvas.ArrayUnitB[i, j]);

                        // update bar
                        ++curValue;
                        this.form.UpdateProgressBar(curValue);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Calc Matching failed!" + ex.Message);
            }
        }
        private ImageUnit MatchEachUnit(int r, int g, int b)
        {
            float tmp;
            float minValue = Int32.MaxValue;
            ImageUnit minImageUnit = null;

            foreach (ImageUnit image in this.m_listImageUnit)
            {
                tmp = this.CalcDistance(r, g, b, image.R, image.G, image.B);
                if (tmp < minValue)
                {
                    minValue = tmp;
                    minImageUnit = image;
                }
            }
            return minImageUnit;
        }
        private float CalcDistance(float r, float g, float b, float rr, float rg, float rb)
        {
            return (r - rr) * (r - rr) + (g - rg) * (g - rg) + (b - rb) * (b - rb);
        }
        public void Draw(Graphics g)
        {
            int nCount = Param.Unit_Count;
            int unitWidth = Param.Canvas_Width / nCount;
            int unitHeight = Param.Canvas_Height / nCount;

            // show final image splicing
            try
            {
                for (int i = 0; i < nCount; ++i)
                {
                    for (int j = 0; j < nCount; ++j)
                    {
                        if (this.m_listMatchedUnit[i, j] == null)
                            return;

                        this.m_listMatchedUnit[i, j].DrawImage(g,
                            j * unitWidth, i * unitHeight, unitWidth, unitHeight);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed Drawing Mathed Image Units!" + ex.Message);
            }

            int baseX = Param.Canvas_Width + Param.Space ;
            int baseY = Param.Space;

            // show true unit colors
            try
            {
                int thumbnailWidth = Param.Thumbnail_Width / Param.Unit_Count;
                int thumbnailHeight = Param.Thumbnail_Height / Param.Unit_Count;
                for (int i = 0; i < nCount; ++i)
                {
                    for (int j = 0; j < nCount; ++j)
                    {
                        if (this.m_listMatchedUnit[i, j] == null)
                            return;

                        g.FillRectangle(
                            new SolidBrush(Color.FromArgb(this.Canvas.ArrayUnitR[i, j],
                            this.Canvas.ArrayUnitG[i, j],
                            this.Canvas.ArrayUnitB[i, j])),
                            baseX + j * thumbnailWidth, baseY + i * thumbnailHeight,
                            thumbnailWidth, thumbnailHeight);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed Drawing Canvas Units Colors!" + ex.Message);
            }

            // show thumbnail
            try
            {
                baseY += Param.Thumbnail_Height + Param.Space;
                g.DrawImage(this.Canvas.ImageData, baseX, baseY,
                    Param.Thumbnail_Width, Param.Thumbnail_Height);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed Drawing Canvas thumbnail!" + ex.Message);
            }
        }
        #endregion


    }
}
