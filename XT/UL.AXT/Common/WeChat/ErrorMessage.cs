﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 错误信息
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// 成功对应的错误码
        /// </summary>
        public const int SuccessCode = 0;
        /// <summary>
        /// 成功
        /// </summary>
        public static readonly ErrorMessage Success = new ErrorMessage(SuccessCode, "请求成功");
        /// <summary>
        /// 异常
        /// </summary>
        public static readonly ErrorMessage Exception = new ErrorMessage(ExceptionCode, "请求异常");
        /// <summary>
        /// 程序异常对应的错误代码
        /// </summary>
        public const int ExceptionCode = -99999;

        /// <summary>
        /// 错误字典
        /// </summary>
        public static Dictionary<int, string> ErrorDictionary = new Dictionary<int, string>()
        {
            {-1     ,	"系统繁忙"},
            {0	    ,	"请求成功"},
            {40001	,	"获取access_token时AppSecret错误，或者access_token无效"},
            {40002	,	"不合法的凭证类型"},
            {40003	,	"不合法的OpenID"},
            {40004	,	"不合法的媒体文件类型"},
            {40005	,	"不合法的文件类型"},
            {40006	,	"不合法的文件大小"},
            {40007	,	"不合法的媒体文件id"},
            {40008	,	"不合法的消息类型"},
            {40009	,	"不合法的图片文件大小"},
            {40010	,	"不合法的语音文件大小"},
            {40011	,	"不合法的视频文件大小"},
            {40012	,	"不合法的缩略图文件大小"},
            {40013	,	"不合法的APPID"},
            {40014	,	"不合法的access_token"},
            {40015	,	"不合法的菜单类型"},
            {40016	,	"不合法的按钮个数"},
            {40017	,	"不合法的按钮个数"},
            {40018	,	"不合法的按钮名字长度"},
            {40019	,	"不合法的按钮KEY长度"},
            {40020	,	"不合法的按钮URL长度"},
            {40021	,	"不合法的菜单版本号"},
            {40022	,	"不合法的子菜单级数"},
            {40023	,	"不合法的子菜单按钮个数"},
            {40024	,	"不合法的子菜单按钮类型"},
            {40025	,	"不合法的子菜单按钮名字长度"},
            {40026	,	"不合法的子菜单按钮KEY长度"},
            {40027	,	"不合法的子菜单按钮URL长度"},
            {40028	,	"不合法的自定义菜单使用用户"},
            {40029	,	"不合法的oauth_code"},
            {40030	,	"不合法的refresh_token"},
            {40031	,	"不合法的openid列表"},
            {40032	,	"不合法的openid列表长度"},
            {40033	,	"不合法的请求字符，不能包含\\uxxxx格式的字符"},
            {40035	,	"不合法的参数"},
            {40038	,	"不合法的请求格式"},
            {40039	,	"不合法的URL长度"},
            {40050	,	"不合法的分组id"},
            {40051	,	"分组名字不合法"},

            {41001	,	"缺少access_token参数"},
            {41002	,	"缺少appid参数"},
            {41003	,	"缺少refresh_token参数"},
            {41004	,	"缺少secret参数"},
            {41005	,	"缺少多媒体文件数据"},
            {41006	,	"缺少media_id参数"},
            {41007	,	"缺少子菜单数据"},
            {41008	,	"缺少oauthcode"},
            {41009	,	"缺少openid"},

            {42001	,	"access_token超时"},
            {42002	,	"refresh_token超时"},
            {42003	,	"oauth_code超时"},

            {43001	,	"需要GET请求"},
            {43002	,	"需要POST请求"},
            {43003	,	"需要HTTPS请求"},
            {43004	,	"需要接收者关注"},
            {43005	,	"需要好友关系"},

            {44001	,	"多媒体文件为空"},
            {44002	,	"POST的数据包为空"},
            {44003	,	"图文消息内容为空"},
            {44004	,	"文本消息内容为空"},

            {45001	,	"多媒体文件大小超过限制"},
            {45002	,	"消息内容超过限制"},
            {45003	,	"标题字段超过限制"},
            {45004	,	"描述字段超过限制"},
            {45005	,	"链接字段超过限制"},
            {45006	,	"图片链接字段超过限制"},
            {45007	,	"语音播放时间超过限制"},
            {45008	,	"图文消息超过限制"},
            {45009	,	"接口调用超过限制"},
            {45010	,	"创建菜单个数超过限制"},
            {45015	,	"回复时间超过限制"},
            {45016	,	"系统分组，不允许修改"},
            {45017	,	"分组名字过长"},
            {45018	,	"分组数量超过上限"},

            {46001	,	"不存在媒体数据"},
            {46002	,	"不存在的菜单版本"},
            {46003	,	"不存在的菜单数据"},
            {46004	,	"不存在的用户"},

            {47001	,	"解析JSON/XML内容错误"},

            {48001	,	"api功能未授权"},

            {50001	,	"用户未授权该api"},
            //客服管理接口错误码
            {61450  ,	"系统错误(system error)"},
            {61451  ,	"参数错误(invalid parameter)"},
            {61452  ,	"无效客服账号(invalid kf_account)"},
            {61453  ,	"账号已存在(kf_account exsited)"},
            {61454  ,	"账号名长度超过限制(前缀10个英文字符)(invalid kf_acount length)"},
            {61455  ,	"账号名包含非法字符(英文+数字)(illegal character in kf_account)"},
            {61456  ,	"账号个数超过限制(10个客服账号)(kf_account count exceeded)"},
            {61457  ,	"无效头像文件类型(invalid file type)"},
            {61458  ,   "客户正在被其他客服接待(customer accepted by xxx@xxxx)"},
            {61459  ,   "客服不在线(kf offline)"},

            {61500  ,	"日期格式错误"},
            {61501  ,	"日期范围错误"},
            //语义理解接口错误码
            {7000000    ,   "请求正常，无语义结果"},
            {7000001    ,   "缺失请求参数"},
            {7000002    ,   "signature 参数无效"},
            {7000003    ,   "地理位置相关配置 1 无效"},
            {7000004    ,   "地理位置相关配置 2 无效"},
            {7000005    ,   "请求地理位置信息失败"},
            {7000006    ,   "地理位置结果解析失败"},
            {7000007    ,   "内部初始化失败"},
            {7000008    ,   "非法 appid（获取密钥失败）"},
            {7000009    ,   "请求语义服务失败"},
            {7000010    ,   "非法 post 请求"},
            {7000011    ,   "post 请求 json 字段无效 "},
            {7000030    ,   "查询 query 太短"},
            {7000031    ,   "查询 query 太长"},
            {7000032    ,   "城市、经纬度信息缺失"},
            {7000033    ,   "query 请求语义处理失败"},
            {7000034    ,   "获取天气信息失败"},
            {7000035    ,   "获取股票信息失败"},
            {7000036    ,   "utf8 编码转换失败"},

            {9001001    ,	"POST数据参数不合法"},
            {9001002    ,	"远端服务不可用"},
            {9001003    ,	"Ticket不合法"},
            {9001004    ,	"获取摇周边用户信息失败"},
            {9001005    ,	"获取商户信息失败"},
            {9001006    ,	"获取OpenID失败"},
            {9001007    ,	"上传文件缺失"},
            {9001008    ,	"上传素材的文件类型不合法"},
            {9001009    ,	"上传素材的文件尺寸不合法"},
            {9001010    ,	"上传失败"},
            {9001020    ,	"帐号不合法"},
            {9001021    ,	"已有设备激活率低于50%，不能新增设备"},
            {9001022    ,	"设备申请数不合法，必须为大于0的数字"},
            {9001023    ,	"已存在审核中的设备ID申请"},
            {9001024    ,	"一次查询设备ID数量不能超过50"},
            {9001025    ,	"设备ID不合法"},
            {9001026    ,	"页面ID不合法"},
            {9001027    ,	"页面参数不合法"},
            {9001028    ,	"一次删除页面ID数量不能超过10"},
            {9001029    ,	"页面已应用在设备中，请先解除应用关系再删除"},
            {9001030    ,	"一次查询页面ID数量不能超过50"},
            {9001031    ,	"时间区间不合法"},
            {9001032    ,	"保存设备与页面的绑定关系参数错误"},
            {9001033    ,	"门店ID不合法"},
            {9001034    ,	"设备备注信息过长"},
            {9001035    ,	"设备申请参数不合法"},
            {9001036    ,	"查询起始值begin不合法"}
        };

        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误提示
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 获取是否为成功消息
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess
        {
            get
            {
                return errcode == SuccessCode;
            }
        }

        /// <summary>
        /// 获取汉字错误提示
        /// </summary>
        /// <returns></returns>
        public string ChineseMessage
        {
            get
            {
                return GetChineseMessage(errcode, errmsg);
            }
        }

        /// <summary>
        /// 获取汉字错误提示
        /// </summary>
        /// <param name="errcode">错误码</param>
        /// <param name="errmsg">错误提示</param>
        /// <returns>返回汉字错误提示</returns>
        internal static string GetChineseMessage(int errcode, string errmsg = null)
        {
            string msg = errmsg;
            if (ErrorDictionary.ContainsKey(errcode))
                msg = ErrorDictionary[errcode];
            return msg;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_errcode">错误码</param>
        /// <param name="_errmsg">错误提示</param>
        public ErrorMessage(int _errcode, string _errmsg)
        {
            errcode = _errcode;
            errmsg = _errmsg;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="e">异常</param>
        public ErrorMessage(Exception e)
        {
            errcode = ExceptionCode;
            errmsg = string.Format("异常源：{0}\r\n错误描述：{1}\r\n堆栈：{2}", e.Source, e.Message, e.StackTrace);
        }

        /// <summary>
        /// 返回错误消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("编码：{0}\r\n提示：{1}", errcode, ChineseMessage ?? "");
        }

        /// <summary>
        /// 返回JSON字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 从JSON字符串解析错误消息
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>返回错误消息</returns>
        internal static ErrorMessage Parse(string json)
        {
            var em = JsonConvert.DeserializeAnonymousType(json, new { errcode = 0, errmsg = "ok" });
            return new ErrorMessage(em.errcode, em.errmsg);
        }

        /// <summary>
        /// 尝试从JSON字符串解析错误消息
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="msg">如果解析成功，返回错误消息；否则，返回null。</param>
        /// <returns></returns>
        internal static bool TryParse(string json, out ErrorMessage msg)
        {
            bool success = false;
            msg = null;
            try
            {
                msg = Parse(json);
                success = true;
            }
            catch { }
            return success;
        }

        /// <summary>
        /// 判断JSON字符串是否为错误消息
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>如果是，返回true；否则，返回false。</returns>
        internal static bool IsErrorMessage(string json)
        {
            JObject jo = JObject.Parse(json);
            JToken jt;
            return jo.TryGetValue("errcode", out jt) && jo.TryGetValue("errmsg", out jt);
        }
    }
}
