using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace CH.Toolbox
{
    #region 同步调用
    /// <summary>
    /// Http连接操作帮助类
    /// <example>
    /// 下面是个例子大家可以看一下
    ///
    ///  HttpLibSyncRequestItem item = new HttpLibSyncRequestItem()
    ///  {
    ///   URL = "http://www.baidu.net",//URL     必需项
    ///   Encoding = "gbk",//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
    ///   Method = "get",//URL     可选项 默认为Get
    ///   //Timeout = 100000,//连接超时时间     可选项默认为100000
    ///   //ReadWriteTimeout = 30000,//写入Post数据超时时间    30000
    ///   //IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转 可选项默认为小写
    ///   Cookie = "",//字符串Cookie     可选项
    ///   // UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
    ///   // Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
    ///   // ContentType = "text/html",//返回类型    可选项有默认值
    ///   Referer = "http://www.baidu.net",//来源URL     可选项
    ///   //Allowautoredirect = true,//是否根据３０１跳转     可选项
    ///   //CerPath = "d:\\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数
    ///   //Connectionlimit = 1024,//最大连接数     可选项 默认为1024
    ///   //Postdata = "username=sufei&pwd=cckan.net",//Post数据     可选项GET时不需要写
    ///   //ProxyIp = "192.168.1.105",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数
    ///   //ProxyPwd = "123456",//代理服务器密码     可选项
    ///   // ProxyUserName = "administrator",//代理服务器账户名     可选项
    ///  };
    ///  //得到HTML代码
    ///  string html = HttpLibSyncRequest.GetHtml(item);
    ///
    ///  //取出返回的Cookie
    ///  string cookie = item.Cookie;
    ///  //取出返回的Request
    ///  HttpWebRequest request = item.Request;
    ///  //取出返回的Response
    ///  HttpWebResponse response = item.Response;
    ///  //取出返回的Reader
    ///  StreamReader reader = item.Reader;
    ///  //取出返回的Headers
    ///  WebHeaderCollection header = response.Headers;
    /// </example>
    /// </summary>
    public static class HttpLibSyncRequest
    {
        #region 私有方法

        //回调验证证书问题
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受
            return true;
        }

        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);

            // write the required bytes
            while (bytesRead > 0)
            {
                stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return stream;
        }

        /// <summary>
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>
        /// </returns>
        private static string GetUrl(string url)
        {
            if (!(url.Contains("http://") || url.Contains("https://")))
            {
                url = "http://" + url;
            }
            return url;
        }

        #endregion 私有方法

        #region 公共方法

        public static string Get(string url)
        {
            return Get(new HttpLibSyncRequestItem() { Url = url });
        }

        public static string Get(HttpLibSyncRequestItem objhttpItem)
        {
            #region 初始设置
            //默认的编码
            Encoding encoding = Encoding.UTF8;
            //读取流的对象
            StreamReader reader = null;

            //HttpWebRequest对象用来发起请求
            HttpWebRequest request = null;

            //获取影响流的数据对象
            HttpWebResponse response = null;

            //需要返回的数据对象
            string returnData = "String Error";


            #region 验证证书

            if (!string.IsNullOrEmpty(objhttpItem.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback =
                    new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.Url));
                //创建证书文件
                X509Certificate objx509 = new X509Certificate(objhttpItem.CerPath);

                //添加到请求里
                request.ClientCertificates.Add(objx509);
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.Url));
            }

            #endregion 验证证书

            #region 设置代理

            if (string.IsNullOrEmpty(objhttpItem.ProxyUserName) && string.IsNullOrEmpty(objhttpItem.ProxyPwd) && string.IsNullOrEmpty(objhttpItem.ProxyIp))
            {
                //不需要设置
            }
            else
            {
                //设置代理服务器
                WebProxy myProxy = new WebProxy(objhttpItem.ProxyIp, false);

                //建议连接
                myProxy.Credentials = new NetworkCredential(objhttpItem.ProxyUserName, objhttpItem.ProxyPwd);

                //给当前请求对象
                request.Proxy = myProxy;
                //设置安全凭证
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }

            #endregion 设置代理

            //请求方式Get或者Post
            request.Method = objhttpItem.Method;
            request.Timeout = objhttpItem.Timeout;
            request.ReadWriteTimeout = objhttpItem.ReadWriteTimeout;
            //Accept
            request.Accept = objhttpItem.Accept;
            //ContentType返回类型
            request.ContentType = objhttpItem.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = objhttpItem.UserAgent;

            #region 编码

            if (string.IsNullOrEmpty(objhttpItem.Encoding) || objhttpItem.Encoding.ToLower().Trim() == "null")
            {
                //读取数据时的编码方式
                encoding = Encoding.UTF8;
            }
            else
            {
                //读取数据时的编码方式
                encoding = System.Text.Encoding.GetEncoding(objhttpItem.Encoding);
            }

            #endregion 编码

            #region Cookie

            if (!string.IsNullOrEmpty(objhttpItem.Cookie))
            {
                //Cookie
                request.Headers[HttpRequestHeader.Cookie] = objhttpItem.Cookie;
            }

            //设置Cookie
            if (objhttpItem.CookieCollection != null)
            {
                if (request.CookieContainer.Count == 0)
                {
                    request.CookieContainer.Add(objhttpItem.CookieCollection);
                }
                else
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(objhttpItem.CookieCollection);
                }
            }

            #endregion Cookie

            //来源地址
            request.Referer = objhttpItem.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = objhttpItem.Allowautoredirect;

            #region Post数据

            //验证在得到结果时是否有传入数据
            if (!string.IsNullOrEmpty(objhttpItem.Postdata) && request.Method.Trim().ToLower().Contains("post"))
            {
                byte[] buffer = encoding.GetBytes(objhttpItem.Postdata);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }

            #endregion Post数据

            //设置最大连接
            if (objhttpItem.Connectionlimit > 0)
            {
                request.ServicePoint.ConnectionLimit = objhttpItem.Connectionlimit;
            }
            #endregion

            #region 读取数据

            try
            {

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.Cookies != null)
                    {
                        objhttpItem.CookieCollection = response.Cookies;
                    }
                    if (response.Headers["set-cookie"] != null)
                    {
                        objhttpItem.Cookie = response.Headers["set-cookie"];
                    }
                    objhttpItem.Response = response;
                    objhttpItem.Request = request;

                    //从这里开始我们要无视编码了
                    if (encoding == null)
                    {
                        MemoryStream stream = new MemoryStream();
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            objhttpItem.Reader = reader;

                            //开始读取流并设置编码方式
                            //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(stream, 10240);
                            //.net4.0以下写法
                            stream = GetMemoryStream(response.GetResponseStream());
                        }
                        else
                        {
                            objhttpItem.Reader = reader;

                            //response.GetResponseStream().CopyTo(stream, 10240);
                            // .net4.0以下写法
                            stream = GetMemoryStream(response.GetResponseStream());
                        }
                        byte[] RawResponse = stream.ToArray();
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);

                        //<meta(.*?)charset([\s]?)=[^>](.*?)>
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                        returnData = encoding.GetString(RawResponse);
                    }
                    else
                    {
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //开始读取流并设置编码方式
                            using (reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding))
                            {
                                objhttpItem.Reader = reader;
                                returnData = reader.ReadToEnd();
                            }
                        }
                        else
                        {
                            //开始读取流并设置编码方式
                            using (reader = new StreamReader(response.GetResponseStream(), encoding))
                            {
                                objhttpItem.Reader = reader;
                                returnData = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                returnData = "String Error";
                response = (HttpWebResponse)ex.Response;
                objhttpItem.Response = response;
            }
            if (objhttpItem.IsToLower)
            {
                returnData = returnData.ToLower();
            }
            return returnData;
            #endregion
        }

        #endregion 公共方法
    }

    /// <summary>
    /// Http请求参考类
    /// </summary>
    public class HttpLibSyncRequestItem
    {
        private string accept = "text/html, application/xhtml+xml, */*";
        private Boolean allowautoredirect = true;
        private string cerPath = string.Empty;
        private int connectionlimit = 1024;
        private string contentType = "text/html";
        private string cookie = string.Empty;
        private CookieCollection cookiecollection = null;
        private string encoding = "utf-8";
        private Boolean isToLower = false;
        private string method = "GET";
        private string postdata;
        private string proxyip = string.Empty;
        private string proxypwd = string.Empty;
        private string proxyusername = string.Empty;
        private StreamReader reader = null;
        private int readWriteTimeout = 30000;
        private string referer = string.Empty;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;
        private int timeout = 100000;
        private string url;

        private string userAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get { return accept; }
            set { accept = value; }
        }

        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public Boolean Allowautoredirect
        {
            get { return allowautoredirect; }
            set { allowautoredirect = value; }
        }

        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath
        {
            get { return cerPath; }
            set { cerPath = value; }
        }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }

        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie
        {
            get { return cookie; }
            set { cookie = value; }
        }

        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }

        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public Boolean IsToLower
        {
            get { return isToLower; }
            set { isToLower = value; }
        }

        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        /// <summary>
        /// Post请求时要发送的Post数据
        /// </summary>
        public string Postdata
        {
            get { return postdata; }
            set { postdata = value; }
        }

        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp
        {
            get { return proxyip; }
            set { proxyip = value; }
        }

        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd
        {
            get { return proxypwd; }
            set { proxypwd = value; }
        }

        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return proxyusername; }
            set { proxyusername = value; }
        }

        /// <summary>
        ///  读取流的对象
        /// </summary>
        public StreamReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return readWriteTimeout; }
            set { readWriteTimeout = value; }
        }

        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer
        {
            get { return referer; }
            set { referer = value; }
        }

        /// <summary>
        /// HttpWebRequest对象用来发起请求
        /// </summary>
        public HttpWebRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        /// <summary>
        /// 获取影响流的数据对象
        /// </summary>
        public HttpWebResponse Response
        {
            get { return response; }
            set { response = value; }
        }

        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }
    }

    #endregion
}
