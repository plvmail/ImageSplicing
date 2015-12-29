using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ImageSplicing
{
    public partial class Form1 : Form
    {
        #region Data

        // thread
        Thread thread;
        // graphics
        private Graphics g;

        // imageSplicing
        ImageSplicing imageSplicing = new ImageSplicing();
        string m_canvasName = "";
        string m_folderName = "";

        // buffer memory
        protected BufferedGraphics m_memoryG = null;
        protected BufferedGraphicsContext m_context = BufferedGraphicsManager.Current;
        private Bitmap m_bufferBmp = null;
        private Graphics m_bufferG = null;

        // usr data changed or not
        bool m_checkCanvasChanged = true;
        bool m_checkImageFolderChanged = true;

        // show on screen
        ImageUnit m_imageUnitOnShow = null;
        int m_imageUnitOnShowPosX = 0;
        int m_imageUnitOnShowPosY = 0;

        #endregion

        #region progress
        private delegate void SetPos(int ipos);
        private delegate void InvokeOpt();
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        public void UpdateProgressBar(int ipos)
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(UpdateProgressBar);
                this.Invoke(setpos, new object[] { ipos });
            }
            else
            {
                // update value
                this.progressBar1.Value = Convert.ToInt32(ipos);

                // show status
                if (this.progressBar1.Value < this.progressBar1.Maximum)
                {
                    this.labelStatus.Text =
                        this.progressBar1.Value.ToString() + "/" + this.progressBar1.Maximum.ToString();
                }
                else
                {
                    this.labelStatus.Text = "Done!";
                    this.btnStatus.BackColor = Color.Green;
                }
            }
        }
        public void SetMaxValueOfProgressBar(int ipos)
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(SetMaxValueOfProgressBar);
                this.Invoke(setpos, new object[] { ipos });
            }
            else
            {
                this.progressBar1.Maximum = Convert.ToInt32(ipos);
                this.btnStatus.BackColor = Color.Red;
            }
        }

        private void LockUserOperation()
        {
            if (this.InvokeRequired)
            {
                InvokeOpt invokeOpt = new InvokeOpt(LockUserOperation);
                this.Invoke(invokeOpt, new object[] { });
            }
            {
                this.btnLoadCanvas.Enabled = false;
                this.btnLoadImage.Enabled = false;
                this.btnMatch.Enabled = false;
            }
        }
        private void UnlockUserOperation()
        {
            if (this.InvokeRequired)
            {
                InvokeOpt invokeOpt = new InvokeOpt(UnlockUserOperation);
                this.Invoke(invokeOpt, new object[] { });
            }
            {
                this.btnLoadCanvas.Enabled = true;
                this.btnLoadImage.Enabled = true;
                this.btnMatch.Enabled = true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            m_memoryG.Graphics.DrawImage(this.m_bufferBmp, 0, 0);

            m_memoryG.Render(e.Graphics);

            this.PaintCurrentShow(e.Graphics);
        }
        private void PaintCurrentShow(Graphics g)
        {
            if (this.m_imageUnitOnShow == null)
                return;

            int x = this.m_imageUnitOnShowPosX;
            int y = this.m_imageUnitOnShowPosY;
            int width = this.m_imageUnitOnShow.Width;
            int height = this.m_imageUnitOnShow.Height;

            // check  border
            if (x + width > Param.Canvas_Width)
                x -= width;
            if (y + height > Param.Canvas_Height)
                y -= height;

            // paint
            g.DrawRectangle(new Pen(Color.Green, 8), x, y, width, height);
            this.m_imageUnitOnShow.DrawImage(g, x, y, width, height);
        }

        
        // load canvas
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter="所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.m_canvasName = fileDialog.FileName;
                this.m_checkCanvasChanged = true;  
                // load canvas
                if (this.m_checkCanvasChanged)
                {
                    // new thread
                    thread = new Thread(new ThreadStart(Thread_LoadCanvas));
                    thread.Start();

                    this.m_checkCanvasChanged = false;
                }
            }

          
        }
        private void Thread_LoadCanvas()
        {
            this.LockUserOperation();
            this.imageSplicing.LoadCanvas(this.m_canvasName);
            this.UnlockUserOperation();
        }

        // load image units
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_folderName = dialog.SelectedPath;
                this.m_checkImageFolderChanged = true;
                // folder of image unit
                if (this.m_checkImageFolderChanged)
                {
                    // new thread
                    thread = new Thread(new ThreadStart(Thread_LoadImageUnits));
                    thread.Start();

                    this.m_checkImageFolderChanged = false;
                }
            }
           
        }
        private void Thread_LoadImageUnits()
        {
            this.LockUserOperation();
            this.imageSplicing.LoadImageUnits(this.m_folderName);
            this.UnlockUserOperation();
        }

        // start matching
        private void button3_Click(object sender, EventArgs e)
        {
            // new thread
            thread = new Thread(Thread_StartMatching);
            thread.Start();
        }
        private void Thread_StartMatching()
        {
            this.LockUserOperation();

            // start match
            imageSplicing.MatchAllUnits();

            this.UnlockUserOperation();

            try
            {
                imageSplicing.Draw(m_bufferG);

            }
            catch (Exception ex)
            {
            }
            this.Invalidate();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StaticData.Init();
            
            // progress bar
            imageSplicing.form = this;

            // double buffer
            this.DoubleBuffered = true; 

            // manual double buffer
            try
            {
                this.m_memoryG = m_context.Allocate(this.CreateGraphics(),
                      new Rectangle(0, 0, 
                          Param.Canvas_Width + Param.Thumbnail_Width + 2*Param.Space, 
                          Param.Canvas_Height));

                m_bufferBmp = new Bitmap(
                          Param.Canvas_Width + Param.Thumbnail_Width + 2 * Param.Space,
                          Param.Canvas_Height);

                g = this.m_memoryG.Graphics;
                m_bufferG = Graphics.FromImage(m_bufferBmp);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot allocate buffer memory!" + ex.Message);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            m_imageUnitOnShow = null;
            this.m_imageUnitOnShowPosX = e.X;
            this.m_imageUnitOnShowPosY = e.Y;


            int unitX = Param.Canvas_Width / Param.Unit_Count;
            int unitY = Param.Canvas_Height / Param.Unit_Count;

            int ix = e.X / unitX;
            int iy = e.Y / unitY;

            if (ix >= 0 && ix < Param.Unit_Count && iy >= 0 && iy < Param.Unit_Count)
            {
                try
                {
                    m_imageUnitOnShow = this.imageSplicing.ListMatchedUnit[iy, ix];
                }
                catch (Exception ex)
                { }
            }
            Invalidate();
        }
        
    }
}
