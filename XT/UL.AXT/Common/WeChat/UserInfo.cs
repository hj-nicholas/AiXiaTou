using System;
using System.Text;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 是否已订阅
        /// </summary>
        public bool? subscribe { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public SexEnum? sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public LanguageEnum? language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime? subscribe_time { get; set; }
        /// <summary>
        /// 用户特权信息
        /// </summary>
        string[] privilege;
        /// <summary>
        /// Union Id
        /// </summary>
        public string unionid { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <param name="sex"></param>
        /// <param name="language"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="country"></param>
        /// <param name="headimgurl"></param>
        /// <param name="subscribe_time"></param>
        /// <param name="unionid"></param>
        public UserInfo(bool subscribe, string openid, string nickname, SexEnum sex,
            LanguageEnum language, string city, string province, string country,
            string headimgurl, DateTime? subscribe_time, string unionid)
        {
            this.subscribe = subscribe;
            this.openid = openid;
            this.nickname = nickname;
            this.sex = sex;
            this.language = language;
            this.city = city;
            this.province = province;
            this.country = country;
            this.headimgurl = headimgurl;
            this.subscribe_time = subscribe_time;
            this.privilege = null;
            this.unionid = unionid;
        }

        /// <summary>
        /// 构造函数：用于获取用户信息
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <param name="sex"></param>
        /// <param name="language"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="country"></param>
        /// <param name="headimgurl"></param>
        /// <param name="subscribe_time"></param>
        /// <param name="unionid"></param>
        public UserInfo(int subscribe, string openid, string nickname, int sex,
            string language, string city, string province, string country,
            string headimgurl, int subscribe_time, string unionid)
        {
            this.subscribe = subscribe == 1;
            this.openid = openid;
            this.nickname = nickname;
            SexEnum _sex;
            if (!Enum.TryParse<SexEnum>(sex.ToString(), out _sex))
                _sex = SexEnum.Unknown;
            this.sex = _sex;
            LanguageEnum lang;
            if (!Enum.TryParse<LanguageEnum>(language, out lang))
                lang = LanguageEnum.zh_CN;
            this.language = lang;
            this.city = city;
            this.province = province;
            this.country = country;
            this.headimgurl = headimgurl;
            this.subscribe_time = Utility.ToDateTime(subscribe_time);
            this.privilege = null;
            this.unionid = unionid;
        }

        /// <summary>
        /// 构造函数：用于用户未关注时的情况
        /// </summary>
        /// <param name="openid">用户id</param>
        public UserInfo(string openid)
            : this(false, openid, string.Empty, SexEnum.Unknown,
            LanguageEnum.zh_CN, string.Empty, string.Empty, string.Empty,
            string.Empty, null, string.Empty)
        {
        }

        /// <summary>
        /// 构造函数：用于获取网页授权用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <param name="sex"></param>
        /// <param name="language"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="country"></param>
        /// <param name="headimgurl"></param>
        /// <param name="privilege"></param>
        /// <param name="unionid"></param>
        public UserInfo(string openid, string nickname, int sex,
            string language, string city, string province, string country,
            string headimgurl, string[] privilege, string unionid)
        {
            this.subscribe = null;
            this.openid = openid;
            this.nickname = nickname;
            SexEnum _sex;
            if (!Enum.TryParse<SexEnum>(sex.ToString(), out _sex))
                _sex = SexEnum.Unknown;
            this.sex = _sex;
            LanguageEnum lang;
            if (!Enum.TryParse<LanguageEnum>(language, out lang))
                lang = LanguageEnum.zh_CN;
            this.language = lang;
            this.city = city;
            this.province = province;
            this.country = country;
            this.headimgurl = headimgurl;
            this.subscribe_time = null;
            this.privilege = privilege;
            this.unionid = unionid;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (privilege != null && privilege.Length > 0)
            {
                foreach (string p in privilege)
                    sb.AppendFormat("{0},", p);
                sb.Remove(sb.Length - 1, 1);
            }
            return string.Format("是否已订阅：{0}\r\n用户id：{1}\r\n昵称：{2}\r\n性别：{3}\r\n" +
                "语言：{4}\r\n城市：{5}\r\n省份：{6}\r\n国家：{7}\r\n" +
                "头像地址：{8}\r\n关注时间：{9}\r\n用户特权信息：{10}\r\nUnionId:{11}",
                subscribe.HasValue && subscribe.Value ? "是" : "否", openid, nickname, sex.HasValue ? sex.Value.ToString("g") : "",
                language.HasValue ? language.Value.ToString("g") : "", city, province, country,
                headimgurl, subscribe_time.HasValue ? subscribe_time.ToString() : "", sb.ToString(), unionid);
        }
    }

    /// <summary>
    /// 性别
    /// </summary>

    public enum SexEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 男性
        /// </summary>
        Male = 1,
        /// <summary>
        /// 女性
        /// </summary>
        Female = 2
    }

    /// <summary>
    /// 国家地区语言版本
    /// </summary>
    public enum LanguageEnum
    {
        /// <summary>
        /// 简体中文
        /// </summary>
        zh_CN,
        /// <summary>
        /// 繁体中文
        /// </summary>
        zh_TW,
        /// <summary>
        /// 英语
        /// </summary>
        en
    }
}