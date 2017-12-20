using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace SEACompliance.Core.Common
{
    /// <summary>
    ///StringHelper 的摘要说明
    /// </summary>
    public sealed class StringHelper
    {
        private StringHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 防SQL注入
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string FilterSql(string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;
            source = source.Replace("'", "''");
            //source = source.Replace(" ", "&nbsp;");
            source = source.Replace("\"", "&#34;");
            source = source.Replace(";", "&#59;");
            source = source.Replace("<", "&#60;");
            source = source.Replace(">", "&#62;");
            source = source.Replace("%", "&#37;");
            source = source.Replace("\\", "&#92;");
            source = source.Replace("--", "&#45;&#45;");
            return source;
        }


        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string MyRandom(int min, int max)
        {
            Random Rnd = new Random();
            return Rnd.Next(min, max).ToString();
        }

        public static bool TryRegex(string input, RegularType type)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regularExpression = string.Empty;

            switch (type)
            {
                case RegularType.Int:
                    regularExpression = @"^\d+$";
                    break;
                case RegularType.PositiveInt:
                    regularExpression = @"^\+?[1-9][0-9]*$";
                    break;
                case RegularType.Money:
                    regularExpression = @"^[0-9]+(.[0-9]{2})?$";
                    break;
                case RegularType.Mail:
                    regularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    break;
                case RegularType.Postalcode:
                    regularExpression = @"^d{6}$";
                    break;
                case RegularType.Phone:
                    regularExpression = @"^(\d{3,4}|\d{3,4}-)?\d{7,8}$";
                    break;
                case RegularType.Mobile:
                    regularExpression = @"^0?1[3|4|5|6|7|8|9][0-9]\d{8}$";
                    break;
                case RegularType.InternetUrl:
                    regularExpression = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
                    break;
                case RegularType.IdCard: //身份证号(15位或18位数字)
                    regularExpression = @"^\d{15}|\d{18}$";
                    break;
                case RegularType.Date:   //日期范围:1900-2099;简单验证1-12月,1-31日
                    regularExpression = @"^(19|20)\d{2}-(0?\d|1[012])-(0?\d|[12]\d|3[01])$";
                    break;
                case RegularType.Ip:
                    regularExpression = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
                    break;
                case RegularType.QQ:
                    regularExpression = @"^[1-9]\d{5,12}$";
                    break;
                case RegularType.ChineseName:
                    regularExpression = @"[\u4e00-\u9fa5]{2,4}";
                    break;
                case RegularType.NickName:
                    regularExpression = @"^[^\d_+]([^x00-xff]|[\S]){1,14}$";
                    break;
                default:
                    break;
            }

            Regex regex = new Regex(regularExpression);

            return regex.Match(input).Success;
        }

        // / <summary>
        /// 把Unicode解码为普通文字 
        /// </summary> ///
        /// <param name="unicodeString">要解码的Unicode字符集</param> 
        /// <returns>解码后的字符串</returns> 
        public static string ConvertToGB(string unicodeString)
        {
            string sourceStr = unicodeString;
            Regex regex = new Regex(@"\\u(\w{4})");
            string result = regex.Replace(sourceStr, delegate(Match m)
            {
                string hexStr = m.Groups[1].Value;
                string charStr = ((char)int.Parse(hexStr, System.Globalization.NumberStyles.HexNumber)).ToString();
                return charStr;
            });
            return result;
        }
        public static string RandomDocId()
        {
            string year = DateTime.Now.Year.ToString().Substring(2);
            string month = DateTime.Now.Month.ToString().PadLeft(2, '0');
            string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string hour = DateTime.Now.Hour.ToString().PadLeft(2, '0');
            string mi = DateTime.Now.Minute.ToString().PadLeft(2, '0');
            string second = DateTime.Now.Second.ToString().PadLeft(2, '0');
            string misecond = DateTime.Now.Millisecond.ToString();
            string guid = GenerateRandomNumber(6);
            string result = year + month + day + guid + second + misecond + GenerateRandomNumber(1);
            return result;
        }
        private static char[] constant =
          {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        // <summary>
        /// 获取当前时间刻度数（唯一）
        /// </summary>
        /// <returns></returns>
        public static long GetTicksForFileName()
        {
            Mutex mutex = new Mutex();
            mutex.WaitOne();

            long ticks = DateTime.Now.Ticks;
            Thread.Sleep(50);

            mutex.ReleaseMutex();

            return ticks;
        }

        /// <summary>
        /// XML特殊字符处理方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string XMLSpecialCharToConvert(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            str = str.Replace("<", "<![CDATA[<]]>");
            str = str.Replace("&", "<![CDATA[&]]>");

            return str;
        }

    }
    /// <summary>
    /// RegularType
    /// </summary>
    public enum RegularType
    {
        Int = 1,
        PositiveInt = 2,
        Money = 3,
        Mail = 4,
        Postalcode = 5,
        Phone = 6,
        Mobile = 7,
        InternetUrl = 8,
        IdCard = 9,
        Date = 10,
        Ip = 11,
        QQ = 12,
        ChineseName = 13,
        /// <summary>
        /// 不以数字开头的昵称,长度为5-20位
        /// </summary>
        NickName = 14
    }

}