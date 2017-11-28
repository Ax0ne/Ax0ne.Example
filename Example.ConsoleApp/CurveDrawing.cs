/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/23 18:13:29
 *  FileName:Class2.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Example.ConsoleApp
{
    public class CurveDrawing
    {
        private readonly List<ArrayList> itemlist;
        private readonly double yMax;
        private readonly double yMin;
        private string title, title2;
        private string xtitle;
        private string ytitle;

        public CurveDrawing(List<ArrayList> itemlist, string title, string title2 = "")
        {
            this.itemlist = itemlist;
            this.title = title;
            this.title2 = title2;

            yMax = -100000000;
            yMin = 100000000;
            for (int i = 0; i < itemlist.Count; i++)
            {
                if (Convert.ToDouble(itemlist[i][1]) > yMax)
                    yMax = Convert.ToDouble(itemlist[i][1]);
                if (Convert.ToDouble(itemlist[i][1]) < yMin)
                    yMin = Convert.ToDouble(itemlist[i][1]);
            }
        }

        /// <summary>
        ///     X坐标的标题
        /// </summary>
        public string Xtitle
        {
            get { return xtitle; }
            set { xtitle = value; }
        }

        /// <summary>
        ///     Y坐标的标题
        /// </summary>
        public string Ytitle
        {
            get { return ytitle; }
            set { ytitle = value; }
        }

        /// <summary>
        ///     副标题
        /// </summary>
        public string Title2
        {
            get { return title2; }
            set { title2 = value; }
        }

        /// <summary>
        ///     主标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        ///     创建并输出图片
        /// </summary>
        /// <returns>生成的文件路径</returns>
        public string Draw()
        {
            #region 基础定义

            //取得记录数量
            int count = itemlist.Count;

            //记算图表宽度
            int wd = 80 + 50*(count - 1);
            //设置最小宽度为640
            if (wd < 640) wd = 640;
            //生成Bitmap对像
            var img = new Bitmap(wd, 400);
            //定义黑色画笔
            var Bp = new Pen(Color.Black);
            //加粗的黑色
            var BBp = new Pen(Color.Black, 2);
            //定义红色画笔
            var Rp = new Pen(Color.Red);
            //定义银灰色画笔
            var Sp = new Pen(Color.Silver);
            //定义大标题字体
            var Bfont = new Font("黑体", 12, FontStyle.Bold);
            //定义一般字体
            var font = new Font("Arial", 8);
            //定义大点的字体
            var Tfont = new Font("Arial", 9);
            //定义黑色过渡型笔刷
            var brush = new LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), Color.Black, Color.Black,
                1.2F, true);
            //定义蓝色过渡型笔刷
            var Bluebrush = new LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), Color.Blue, Color.Blue,
                1.2F, true);
            var Silverbrush = new LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), Color.Silver,
                Color.Silver, 1.2F, true);

            #endregion

            //生成绘图对像
            try
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    #region 绘制图表

                    //绘制底色
                    g.DrawRectangle(new Pen(Color.White, 400), 0, 0, img.Width, img.Height);
                    //绘制大标题
                    g.DrawString(title, Bfont, brush, wd/2 - title.Length*10, 5);
                    //绘制小标题
                    g.DrawString(title2, Tfont, Silverbrush, wd/2 - title.Length*10 + 40, 25);
                    //绘制图片边框
                    g.DrawRectangle(Bp, 0, 0, img.Width - 1, img.Height - 1);

                    //绘制Y坐标线
                    for (int i = 0; i < (count < 12 ? 12 : count); i++)
                        g.DrawLine(Sp, 40 + 50*i, 60, 40 + 50*i, 360);
                    //绘制X轴坐标标签
                    for (int i = 0; i < count; i++)
                        g.DrawString(itemlist[i][0].ToString(), font, brush, 30 + 50*i, 370);
                    //绘制X坐标线
                    for (int i = 0; i < 11; i++)
                    {
                        g.DrawLine(Sp, 40, 60 + 30*i, 40 + 50*((count < 12 ? 12 : count) - 1), 60 + 30*i);
                        double s = yMax - (yMax + Math.Abs(yMin))/10*i; //最大的Y坐标值
                        g.DrawString(Math.Floor(s).ToString(), font, brush, 10, 55 + 30*i);
                    }

                    //绘制Y坐标轴
                    g.DrawLine(BBp, 40, 50, 40, 360);
                    //绘制X坐标轴
                    g.DrawLine(BBp, 40, 360, 40 + 50*((count < 12 ? 12 : count) - 1) + 10, 360);

                    #endregion

                    #region 绘制曲线

                    //定义曲线转折点
                    var p = new Point[count];
                    for (int i = 0; i < count; i++)
                    {
                        p[i].X = 40 + 50*i;
                        p[i].Y = 360 -
                                 (int)
                                     (((Convert.ToDouble(itemlist[i][1]) + Math.Abs(yMin))/((yMax + Math.Abs(yMin))/10))*
                                      30);
                    }
                    //绘制发送曲线
                    g.DrawLines(Rp, p);

                    for (int i = 0; i < count; i++)
                    {
                        //绘制发送记录点的数值
                        g.DrawString(itemlist[i][1].ToString(), font, Bluebrush, p[i].X + 5, p[i].Y - 10);
                        //绘制发送记录点
                        g.DrawRectangle(Rp, p[i].X - 2, p[i].Y - 2, 4, 4);
                    }

                    #endregion

                    //绘制Y坐标标题
                    g.DrawString(ytitle, Tfont, brush, 10, 40);
                    //绘制X坐标标题
                    g.DrawString(xtitle, Tfont, brush, 30, 385);
                    //图片质量
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    //保存绘制的图片
                    string basePath = HttpContext.Current.Server.MapPath("/Curve/"),
                        fileName = Guid.NewGuid() + ".jpg";

                    using (var fs = new FileStream(basePath + fileName, FileMode.CreateNew))
                    {
                        if (!Directory.Exists(basePath))
                            Directory.CreateDirectory(basePath);
                        img.Save(fs, ImageFormat.Jpeg);
                        return "/Curve/" + fileName;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}