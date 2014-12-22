/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/23 18:37:25
 *  FileName:Drawing.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/

using System;
using System.Drawing;

namespace Example.ConsoleApp
{
    public class Drawing
    {
        private Color m_AxisColor = Color.Black; //轴线颜色
        private Color m_AxisTextColor = Color.Black; //轴说明文字颜色
        private Color m_BgColor = Color.Snow; //背景
        private Color m_BorderColor = Color.AliceBlue; //整体边框颜色
        private Color m_CurveColor = Color.FromArgb(51, 184, 193); //曲线颜色
        private int m_Height = 236; //图像高度
        private string[] m_Keys = {"9-12", "9-12", "9-12", "9-12", "9-12", "9-12", "9-12"}; //键
        private Color m_SliceColor = Color.Black; //刻度颜色
        private Color m_SliceTextColor = Color.Black; //刻度文字颜色
        private float m_Tension;
        private Color m_TextColor = Color.Black; //文字颜色
        private string m_Title = "近一个月产品曝光量统计"; //Title
        private float[] m_Values = {0, 55, 99, 111, 222, 333, 222}; //值
        private int m_Width = 440; //图像宽度
        private string m_XAxisText = "月份"; //X轴说明文字
        private float m_XSlice = 50; //X轴刻度宽度
        private string m_YAxisText = "数量"; //Y轴说明文字
        private float m_YSlice = 20; //Y轴刻度宽度
        private float m_YSliceValue = 60; //Y轴刻度的数值宽度
        private Bitmap objBitmap; //位图对象
        private Graphics objGraphics; //Graphics 类提供将对象绘制到显示设备的方法

        public Drawing()
        {
            YSliceBegin = 0;
        }

        /// <summary>
        ///     图像宽度
        /// </summary>
        public int Width
        {
            set
            {
                if (value < 300)
                {
                    m_Width = 300;
                }
                else
                {
                    m_Width = value;
                }
            }
            get { return m_Width; }
        }

        /// <summary>
        ///     图像高度
        /// </summary>
        public int Height
        {
            set
            {
                if (value < 200)
                {
                    m_Height = 200;
                }
                else
                {
                    m_Height = value;
                }
            }
            get { return m_Height; }
        }

        /// <summary>
        ///     X轴刻度宽度
        /// </summary>
        public float XSlice
        {
            set { m_XSlice = value; }
            get { return m_XSlice; }
        }

        /// <summary>
        ///     Y轴刻度宽度
        /// </summary>
        public float YSlice
        {
            set { m_YSlice = value; }
            get { return m_YSlice; }
        }

        /// <summary>
        ///     Y轴刻度的数值宽度
        /// </summary>
        public float YSliceValue
        {
            set { m_YSliceValue = value; }
            get { return m_YSliceValue; }
        }

        /// <summary>
        ///     Y轴刻度开始值
        /// </summary>
        public float YSliceBegin { get; set; }

        /// <summary>
        ///     曲线的张力
        /// </summary>
        public float Tension
        {
            set
            {
                if (value < 0.0f && value > 1.0f)
                {
                    m_Tension = 0.5f;
                }
                else
                {
                    m_Tension = value;
                }
            }
            get { return m_Tension; }
        }

        /// <summary>
        ///     图片标题
        /// </summary>
        public string Title
        {
            set { m_Title = value; }
            get { return m_Title; }
        }

        /// <summary>
        ///     X轴坐标值
        /// </summary>
        public string[] Keys
        {
            set { m_Keys = value; }
            get { return m_Keys; }
        }

        /// <summary>
        ///     Y轴坐标值
        /// </summary>
        public float[] Values
        {
            set { m_Values = value; }
            get { return m_Values; }
        }

        /// <summary>
        ///     背景色
        /// </summary>
        public Color BgColor
        {
            set { m_BgColor = value; }
            get { return m_BgColor; }
        }

        /// <summary>
        ///     文字背景
        /// </summary>
        public Color TextColor
        {
            set { m_TextColor = value; }
            get { return m_TextColor; }
        }

        /// <summary>
        ///     整体边框色
        /// </summary>
        public Color BorderColor
        {
            set { m_BorderColor = value; }
            get { return m_BorderColor; }
        }

        /// <summary>
        ///     轴的颜色
        /// </summary>
        public Color AxisColor
        {
            set { m_AxisColor = value; }
            get { return m_AxisColor; }
        }

        /// <summary>
        ///     X轴说明文字
        /// </summary>
        public string XAxisText
        {
            set { m_XAxisText = value; }
            get { return m_XAxisText; }
        }

        /// <summary>
        ///     Y轴说明文字
        /// </summary>
        public string YAxisText
        {
            set { m_YAxisText = value; }
            get { return m_YAxisText; }
        }

        /// <summary>
        ///     轴的说明文字颜色
        /// </summary>
        public Color AxisTextColor
        {
            set { m_AxisTextColor = value; }
            get { return m_AxisTextColor; }
        }

        /// <summary>
        ///     刻度文字颜色
        /// </summary>
        public Color SliceTextColor
        {
            set { m_SliceTextColor = value; }
            get { return m_SliceTextColor; }
        }

        /// <summary>
        ///     刻度颜色
        /// </summary>
        public Color SliceColor
        {
            set { m_SliceColor = value; }
            get { return m_SliceColor; }
        }

        /// <summary>
        ///     曲线颜色
        /// </summary>
        public Color CurveColor
        {
            set { m_CurveColor = value; }
            get { return m_CurveColor; }
        }

        /// <summary>
        ///     生成图像
        /// </summary>
        /// <param name="savePath">图像存放目录</param>
        public Bitmap CreateImage()
        {
            InitializeGraph();

            DrawContent(ref objGraphics);
            return objBitmap;
            //objBitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            // 释放资源
            //objBitmap.Dispose();
            //objGraphics.Dispose();
        }

        //初始化和填充图像区域，画出边框，初始标题
        private void InitializeGraph()
        {
            //根据给定的高度和宽度创建一个位图图像
            objBitmap = new Bitmap(Width, Height);

            //从指定的 objBitmap 对象创建 objGraphics 对象 (即在objBitmap对象中画图)
            objGraphics = Graphics.FromImage(objBitmap);

            //根据给定颜色(LightGray)填充图像的矩形区域 (背景)
            objGraphics.DrawRectangle(new Pen(BorderColor, 2), 0, 0, Width, Height);
            objGraphics.FillRectangle(new SolidBrush(BgColor), 1, 1, Width - 2, Height - 2);

            //画X轴,pen,x1,y1,x2,y2 注意图像的原始X轴和Y轴计算是以左上角为原点，向右和向下计算的
            objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 100, Height - 50, Width - 50, Height - 50);

            //画Y轴,pen,x1,y1,x2,y2
            objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 80, Height - 50, 80, 50);

            //初始化轴线说明文字
            SetAxisText(ref objGraphics);

            //初始化X轴上的刻度和文字
            SetXAxis(ref objGraphics);

            //初始化Y轴上的刻度和文字
            SetYAxis(ref objGraphics);

            //初始化标题
            CreateTitle(ref objGraphics);
        }

        private void SetAxisText(ref Graphics objGraphics)
        {
            objGraphics.DrawString(XAxisText, new Font("宋体", 10), new SolidBrush(AxisTextColor), Width/2 - 25,
                Height - 20);

            int X = 4;
            int Y = (Height/2) - 20;
            for (int i = 0; i < YAxisText.Length; i++)
            {
                objGraphics.DrawString(YAxisText[i].ToString(), new Font("宋体", 10), new SolidBrush(AxisTextColor), X, Y);
                Y += 15;
            }
        }

        private void SetXAxis(ref Graphics objGraphics)
        {
            int x1 = 80;
            int y1 = Height - 110;
            int y2 = Height - 40;
            int iCount = 0;
            int iSliceCount = 1;
            float Scale = 0;
            var iWidth = (int) ((Width - 100)*(50/XSlice));

            objGraphics.DrawString(Keys[0], new Font("宋体", 10), new SolidBrush(SliceTextColor), 65, Height - 40);

            for (int i = 0; i <= iWidth; i += 10)
            {
                Scale = i*(XSlice/50);

                if (iCount == 5)
                {
                    //objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor)), x1 + Scale, y1, x2 + Scale, y2);
                    //The Point!这里显示X轴刻度
                    if (iSliceCount <= Keys.Length - 1)
                    {
                        objGraphics.DrawString(Keys[iSliceCount], new Font("宋体", 10), new SolidBrush(SliceTextColor),
                            x1 + Scale - 15, y2);
                    }
                    iCount = 0;
                    iSliceCount++;
                    if (x1 + Scale > Width - 100)
                    {
                        break;
                    }
                }
                iCount++;
            }
        }

        private void SetYAxis(ref Graphics objGraphics)
        {
            int x1 = 95;
            var y1 = (int) (Height - 50 - 10*(YSlice/50));
            int x2 = 105;
            var y2 = (int) (Height - 50 - 10*(YSlice/50));
            int iCount = 1;
            float Scale = 0;
            int iSliceCount = 1;

            var iHeight = (int) ((Height - 100)*(50/YSlice));

            objGraphics.DrawString(YSliceBegin.ToString(), new Font("宋体", 10), new SolidBrush(SliceTextColor), 24,
                Height - 60);

            for (int i = 0; i < iHeight; i += 10)
            {
                Scale = i*(YSlice/50);

                if (iCount == 5)
                {
                    //objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor)), x1 - 5, y1 - Scale, x2 + 5, y2 - Scale);
                    //The Point!这里显示Y轴刻度
                    objGraphics.DrawString(Convert.ToString(YSliceValue*iSliceCount + YSliceBegin), new Font("宋体", 10),
                        new SolidBrush(SliceTextColor), 24, y1 - Scale);

                    iCount = 0;
                    iSliceCount++;
                }
                iCount++;
            }
        }

        private void DrawContent(ref Graphics objGraphics)
        {
            if (Keys.Length == Values.Length)
            {
                var CurvePen = new Pen(CurveColor, 3);
                var CurvePointF = new PointF[Keys.Length];
                float keys = 0;
                float values = 0;
                float Offset1 = (Height - 50) + YSliceBegin;
                float Offset2 = (YSlice/50)*(50/YSliceValue);

                for (int i = 0; i < Keys.Length; i++)
                {
                    keys = XSlice*i + 80;
                    values = Offset1 - Values[i]*Offset2;
                    CurvePointF[i] = new PointF(keys, values);
                }
                objGraphics.DrawCurve(CurvePen, CurvePointF, Tension);
            }
            else
            {
                objGraphics.DrawString("Error!The length of Keys and Values must be same!", new Font("宋体", 16),
                    new SolidBrush(TextColor), new Point(150, Height/2));
            }
        }

        //初始化标题
        private void CreateTitle(ref Graphics objGraphics)
        {
            objGraphics.DrawString(Title, new Font("宋体", 14), new SolidBrush(TextColor), new Point(Width/2 - 120, 30));
        }
    }
}