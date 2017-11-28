/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/23 18:37:25
 *  FileName:Drawing.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/

using System;
using System.Drawing;

namespace Example.ConsoleApp
{
    public class Drawing
    {
        private Color _mAxisColor = Color.Black; //轴线颜色
        private Color _mAxisTextColor = Color.Black; //轴说明文字颜色

        private Color _mCurveColor = Color.FromArgb(51, 184, 193); //曲线颜色
        private int _mHeight = 236; //图像高度

        private Color _mSliceColor = Color.Black; //刻度颜色
        private Color _mSliceTextColor = Color.Black; //刻度文字颜色
        private float _mTension;
        private Color _mTextColor = Color.Black; //文字颜色
        private string _mTitle = "近一个月产品曝光量统计"; //Title

        private int _mWidth = 440; //图像宽度
        private string _mXAxisText = "月份"; //X轴说明文字

        private string _mYAxisText = "数量"; //Y轴说明文字

        private Bitmap _objBitmap; //位图对象
        private Graphics _objGraphics; //Graphics 类提供将对象绘制到显示设备的方法

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
                this._mWidth = value < 300 ? 300 : value;
            }
            get { return _mWidth; }
        }

        /// <summary>
        ///     图像高度
        /// </summary>
        public int Height
        {
            set
            {
                _mHeight = value < 200 ? 200 : value;
            }
            get { return _mHeight; }
        }

        /// <summary>
        ///     X轴刻度宽度
        /// </summary>
        public float XSlice { set; get; } = 50;

        /// <summary>
        ///     Y轴刻度宽度
        /// </summary>
        public float YSlice { set; get; } = 20;

        /// <summary>
        ///     Y轴刻度的数值宽度
        /// </summary>
        public float YSliceValue { set; get; } = 60;

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
                    _mTension = 0.5f;
                }
                else
                {
                    _mTension = value;
                }
            }
            get { return _mTension; }
        }

        /// <summary>
        ///     图片标题
        /// </summary>
        public string Title
        {
            set { _mTitle = value; }
            get { return _mTitle; }
        }

        /// <summary>
        ///     X轴坐标值
        /// </summary>
        public string[] Keys { set; get; }

        /// <summary>
        ///     Y轴坐标值
        /// </summary>
        public float[] Values { set; get; }

        /// <summary>
        ///     背景色
        /// </summary>
        public Color BgColor { set; get; } = Color.Snow;

        /// <summary>
        ///     文字背景
        /// </summary>
        public Color TextColor
        {
            set { _mTextColor = value; }
            get { return _mTextColor; }
        }

        /// <summary>
        ///     整体边框色
        /// </summary>
        public Color BorderColor { set; get; } = Color.AliceBlue;

        /// <summary>
        ///     轴的颜色
        /// </summary>
        public Color AxisColor
        {
            set { _mAxisColor = value; }
            get { return _mAxisColor; }
        }

        /// <summary>
        ///     X轴说明文字
        /// </summary>
        public string XAxisText
        {
            set { _mXAxisText = value; }
            get { return _mXAxisText; }
        }

        /// <summary>
        ///     Y轴说明文字
        /// </summary>
        public string YAxisText
        {
            set { _mYAxisText = value; }
            get { return _mYAxisText; }
        }

        /// <summary>
        ///     轴的说明文字颜色
        /// </summary>
        public Color AxisTextColor
        {
            set { _mAxisTextColor = value; }
            get { return _mAxisTextColor; }
        }

        /// <summary>
        ///     刻度文字颜色
        /// </summary>
        public Color SliceTextColor
        {
            set { _mSliceTextColor = value; }
            get { return _mSliceTextColor; }
        }

        /// <summary>
        ///     刻度颜色
        /// </summary>
        public Color SliceColor
        {
            set { _mSliceColor = value; }
            get { return _mSliceColor; }
        }

        /// <summary>
        ///     曲线颜色
        /// </summary>
        public Color CurveColor
        {
            set { _mCurveColor = value; }
            get { return _mCurveColor; }
        }

        /// <summary>
        ///     生成图像
        /// </summary>
        public Bitmap CreateImage()
        {
            InitializeGraph();

            DrawContent(ref _objGraphics);
            return _objBitmap;
            //objBitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            // 释放资源
            //objBitmap.Dispose();
            //objGraphics.Dispose();
        }

        //初始化和填充图像区域，画出边框，初始标题
        private void InitializeGraph()
        {
            //根据给定的高度和宽度创建一个位图图像
            _objBitmap = new Bitmap(Width, Height);

            //从指定的 objBitmap 对象创建 objGraphics 对象 (即在objBitmap对象中画图)
            _objGraphics = Graphics.FromImage(_objBitmap);

            //根据给定颜色(LightGray)填充图像的矩形区域 (背景)
            _objGraphics.DrawRectangle(new Pen(BorderColor, 2), 0, 0, Width, Height);
            _objGraphics.FillRectangle(new SolidBrush(BgColor), 1, 1, Width - 2, Height - 2);

            //画X轴,pen,x1,y1,x2,y2 注意图像的原始X轴和Y轴计算是以左上角为原点，向右和向下计算的
            _objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 100, Height - 50, Width - 50, Height - 50);

            //画Y轴,pen,x1,y1,x2,y2
            _objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 80, Height - 50, 80, 50);

            //初始化轴线说明文字
            SetAxisText(ref _objGraphics);

            //初始化X轴上的刻度和文字
            SetXAxis(ref _objGraphics);

            //初始化Y轴上的刻度和文字
            SetYAxis(ref _objGraphics);

            //初始化标题
            CreateTitle(ref _objGraphics);
        }

        private void SetAxisText(ref Graphics objGraphics)
        {
            objGraphics.DrawString(XAxisText, new Font("宋体", 10), new SolidBrush(AxisTextColor), Width / 2 - 25,
                Height - 20);

            var x = 4;
            var y = (Height / 2) - 20;
            for (var i = 0; i < YAxisText.Length; i++)
            {
                objGraphics.DrawString(YAxisText[i].ToString(), new Font("宋体", 10), new SolidBrush(AxisTextColor), x, y);
                y += 15;
            }
        }

        private void SetXAxis(ref Graphics objGraphics)
        {
            const int x1 = 80;
            var y1 = Height - 110;
            var y2 = Height - 40;
            var iCount = 0;
            var iSliceCount = 1;
            var iWidth = (int)((Width - 100) * (50 / XSlice));

            objGraphics.DrawString(Keys[0], new Font("宋体", 10), new SolidBrush(SliceTextColor), 65, Height - 40);

            for (var i = 0; i <= iWidth; i += 10)
            {
                var scale = i * (XSlice / 50);

                if (iCount == 5)
                {
                    //objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor)), x1 + Scale, y1, x2 + Scale, y2);
                    //The Point!这里显示X轴刻度
                    if (iSliceCount <= Keys.Length - 1)
                    {
                        objGraphics.DrawString(Keys[iSliceCount], new Font("宋体", 10), new SolidBrush(SliceTextColor),
                            x1 + scale - 15, y2);
                    }
                    iCount = 0;
                    iSliceCount++;
                    if (x1 + scale > Width - 100)
                    {
                        break;
                    }
                }
                iCount++;
            }
        }

        private void SetYAxis(ref Graphics objGraphics)
        {
            var y1 = (int)(Height - 50 - 10 * (YSlice / 50));
            var iCount = 1;
            var iSliceCount = 1;

            var iHeight = (int)((Height - 100) * (50 / YSlice));

            objGraphics.DrawString(YSliceBegin.ToString(), new Font("宋体", 10), new SolidBrush(SliceTextColor), 24,
                Height - 60);

            for (var i = 0; i < iHeight; i += 10)
            {
                var scale = i * (this.YSlice / 50);

                if (iCount == 5)
                {
                    //objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor)), x1 - 5, y1 - Scale, x2 + 5, y2 - Scale);
                    //The Point!这里显示Y轴刻度
                    objGraphics.DrawString(
                        Convert.ToString(YSliceValue * iSliceCount + YSliceBegin),
                        new Font("宋体", 10),
                        new SolidBrush(SliceTextColor),
                        24,
                        y1 - scale);

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
                var curvePen = new Pen(CurveColor, 3);
                var curvePointF = new PointF[Keys.Length];
                var offset1 = (Height - 50) + YSliceBegin;
                var offset2 = (YSlice / 50) * (50 / YSliceValue);

                for (var i = 0; i < Keys.Length; i++)
                {
                    var keys = this.XSlice * i + 80;
                    var values = offset1 - this.Values[i] * offset2;
                    curvePointF[i] = new PointF(keys, values);
                }
                objGraphics.DrawCurve(curvePen, curvePointF, Tension);
            }
            else
            {
                objGraphics.DrawString("Error!The length of Keys and Values must be same!", new Font("宋体", 16),
                    new SolidBrush(TextColor), new Point(150, Height / 2));
            }
        }

        //初始化标题
        private void CreateTitle(ref Graphics objGraphics)
        {
            objGraphics.DrawString(Title, new Font("宋体", 14), new SolidBrush(TextColor), new Point(Width / 2 - 120, 30));
        }
    }
}