using System;
using System.Web;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// RequestEx
    /// 功能：HttpRequest辅助类。
    /// 作者：王先荣
    /// 时间：2015年2月8日
    /// </summary>
    public static class RequestEx
    {
        //静态方法
        /// <summary>
        /// 尝试获取QueryString值
        /// </summary>
        /// <param name="name">QueryString参数名</param>
        /// <param name="defaultValue">如果不存在指定的参数，返回的默认值</param>
        /// <param name="request">HTTP请求</param>
        /// <returns>返回获取到的QueryString值</returns>
        public static string TryGetQueryString(string name, string defaultValue = "", HttpRequest request = null)
        {
            string result = null;
            if (request == null)
                request = HttpContext.Current.Request;
            if (request != null)
                result = request.QueryString[name];
            if (result == null)
                result = defaultValue;
            return result;
        }

        /// <summary>
        /// 尝试获取FORM值
        /// </summary>
        /// <param name="name">FORM参数名</param>
        /// <param name="defaultValue">如果FORM参数不存在，需要返回的默认值</param>
        /// <param name="request">HTTP请求</param>
        /// <returns>返回FORM值</returns>
        public static string TryGetForm(string name, string defaultValue = "", HttpRequest request = null)
        {
            string result = null;
            if (request == null)
                request = HttpContext.Current.Request;
            if (request != null)
                result = request.Form[name];
            if (result == null)
                result = defaultValue;
            return result;
        }

        /// <summary>
        /// 尝试获取 QueryString、Form、Cookies 或 ServerVariables 的值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">如果参数不存在，需要返回的默认值</param>
        /// <param name="request">HTTP请求</param>
        /// <returns>返回参数值</returns>
        public static string TryGet(string name, string defaultValue = "", HttpRequest request = null)
        {
            string result = null;
            if (request == null)
                request = HttpContext.Current.Request;
            if (request != null)
                result = request[name];
            if (result == null)
                result = defaultValue;
            return result;
        }

        /// <summary>
        /// 将带“~”的虚拟路径转换成不带“~”的绝对虚拟路径
        /// </summary>
        /// <param name="url">源虚拟路径</param>
        /// <param name="request">HTTP请求</param>
        /// <returns>返回转换之后的绝对虚拟路径</returns>
        public static string TranslateToAbsoluteUrl(string url, HttpRequest request = null)
        {
            string absoluteUrl = "";
            if (url.StartsWith("~"))	//只转换以“~”开始的虚拟路径
            {
                if (request == null)
                    request = HttpContext.Current.Request;
                string applicationPath = request != null ? request.ApplicationPath : "";
                if (applicationPath.EndsWith("/"))	//如果应用程序路径以“/”结尾，去掉最后的“/”
                    applicationPath = applicationPath.Remove(applicationPath.Length - 1);
                absoluteUrl = applicationPath + url.Substring(1);	//绝对虚拟路径 = 应用程序路径 + 去掉“~”之后的路径
            }
            else
                absoluteUrl = url;
            return absoluteUrl;
        }

        /// <summary>
        /// 得到应用程序的根目录url（最后没有“/”）
        /// </summary>
        /// <param name="request">HTTP请求</param>
        /// <returns>返回应用程序的根目录url</returns>
        public static string GetApplicationRootUrl(HttpRequest request = null)
        {
            string rootUrl = "";
            if (request == null)
                request = HttpContext.Current.Request;
            if (request != null)
            {
                string applicationPath = request.ApplicationPath;	//应用程序路径，例如“/书店网站”
                string absoluteUri = request.Url.AbsoluteUri;		//当前页面的完整uri，例如“http://www.contoso.com/catalog/shownew.htm?date=today”
                string absolutePath = request.Url.AbsolutePath;		//当前页面的绝对路径（不包括方案、主机名或 URI 的查询部分），录入“/catalog/shownew.htm”
                int pos = absoluteUri.IndexOf(absolutePath);
                string websiteUrl = absoluteUri.Substring(0, pos);	//当前页所在的站点根路径，例如“http://www.contoso.com”
                rootUrl = websiteUrl + applicationPath;
            }
            return rootUrl;
        }
    }
}
