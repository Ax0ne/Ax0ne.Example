using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Example.Payment.Alipay
{
    /// <summary>
    /// 类名：Notify
    /// 功能：支付宝通知处理类
    /// 详细：处理支付宝各接口通知返回
    /// 版本：3.3
    /// 修改日期：2011-07-05
    /// //////////////////////注意/////////////////////////////
    /// 调试通知返回时，可查看或改写log日志的写入TXT里的数据，来检查通知返回是否正常 
    /// </summary>
    public class Notify
    {
        #region 字段
        //合作身份者ID
        private readonly string _partner;               
        //支付宝的公钥
        private readonly string _alipayPublicKey;            
        //编码格式
        private readonly string _inputCharset;         
        //签名方式
        private readonly string _signType;       
        //支付宝消息验证地址
        private readonly string _httpsVeryfyUrl = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
        #endregion


        /// <summary>
        /// 构造函数
        /// 从配置文件中初始化变量
        /// </summary>
        public Notify()
        {
            //初始化基础配置信息
            _partner = AliPayUtils.AliPayConfig.Partner.Trim();
            _alipayPublicKey = GetPublicKeyStr(AliPayUtils.AliPayConfig.AlipayPublicKey.Trim());
            _inputCharset = AliPayUtils.AliPayConfig.InputCharset.Trim().ToLower();
            _signType = AliPayUtils.AliPayConfig.SignType.Trim().ToUpper();
        }

        /// <summary>
        /// 从文件读取公钥转公钥字符串
        /// </summary>
        /// <param name="path">公钥文件路径</param>
        public static string GetPublicKeyStr(string path)
        {
            var sr = new StreamReader(path);
            var pubkey = sr.ReadToEnd();
            sr.Close();
            pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
            pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
            pubkey = pubkey.Replace("\r", "");
            pubkey = pubkey.Replace("\n", "");

            return pubkey;
        }

        /// <summary>
        ///  验证消息是否是支付宝发出的合法消息
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="notifyId">通知验证ID</param>
        /// <param name="sign">支付宝生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string notifyId, string sign)
        {
            //获取返回时的签名验证结果
            bool isSign = GetSignVeryfy(inputPara, sign);
            //获取是否是支付宝服务器发来的请求的验证结果
            var responseTxt = "false";
            if (!string.IsNullOrEmpty(notifyId)) { responseTxt = GetResponseTxt(notifyId); }

            //写日志记录（若要调试，请取消下面两行注释）
            //string sWord = "responseTxt=" + responseTxt + "\n isSign=" + isSign.ToString() + "\n 返回回来的参数：" + GetPreSignStr(inputPara) + "\n ";
            //Core.LogResult(sWord);

            //判断responsetTxt是否为true，isSign是否为true
            //responsetTxt的结果不是true，与服务器设置问题、合作身份者ID、notify_id一分钟失效有关
            //isSign不是true，与安全校验码、请求时的参数格式（如：带自定义参数等）、编码格式有关
            if (responseTxt == "true" && isSign)//验证成功
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取待签名字符串（调试用）
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <returns>待签名字符串</returns>
        // ReSharper disable once UnusedMember.Local
        private string GetPreSignStr(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤空值、sign与sign_type参数
            sPara = AliPayUtils.FilterPara(inputPara);

            //获取待签名字符串
            string preSignStr = AliPayUtils.CreateLinkString(sPara);

            return preSignStr;
        }

        /// <summary>
        /// 获取返回时的签名验证结果
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="sign">对比的签名结果</param>
        /// <returns>签名验证结果</returns>
        private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign)
        {
            //过滤空值、sign与sign_type参数
            var sPara = AliPayUtils.FilterPara(inputPara);
            //获取待签名字符串
            var preSignStr = AliPayUtils.CreateLinkString(sPara);

            //获得签名验证结果
            var isSgin = false;
            if (!string.IsNullOrEmpty(sign))
            {
                if (_signType == "RSA")
                {
                    isSgin = RSA.Verify(preSignStr, sign, _alipayPublicKey, _inputCharset);
                }
            }

            return isSgin;
        }

        /// <summary>
        /// 获取是否是支付宝服务器发来的请求的验证结果
        /// </summary>
        /// <param name="notify_id">通知验证ID</param>
        /// <returns>验证结果</returns>
        private string GetResponseTxt(string notify_id)
        {
            string veryfy_url = _httpsVeryfyUrl + "partner=" + _partner + "&notify_id=" + notify_id;

            //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
            string responseTxt = Get_Http(veryfy_url, 120000);

            return responseTxt;
        }

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl">指定URL路径地址</param>
        /// <param name="timeout">超时时间设置</param>
        /// <returns>服务器ATN结果</returns>
        private string Get_Http(string strUrl, int timeout)
        {
            string strResult=null;
            try
            {
                var myReq = (HttpWebRequest)WebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                var httpWResp = (HttpWebResponse)myReq.GetResponse();
                var myStream = httpWResp.GetResponseStream();
                if (myStream != null)
                {
                    var sr = new StreamReader(myStream, Encoding.Default);
                    var strBuilder = new StringBuilder();
                    while (-1 != sr.Peek())
                    {
                        strBuilder.Append(sr.ReadLine());
                    }

                    strResult = strBuilder.ToString();
                }
                return strResult;
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
                return strResult;
            }
        }
    }
}
