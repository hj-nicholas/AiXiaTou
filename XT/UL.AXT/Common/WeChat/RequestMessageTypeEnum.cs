using System;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 请求消息类型：用户发给公众号的消息类型
    /// </summary>
    public enum RequestMessageTypeEnum
    {
        /// <summary>
        /// 文本
        /// </summary>
        text,
        /// <summary>
        /// 图片
        /// </summary>
        image,
        /// <summary>
        /// 语音
        /// </summary>
        voice,
        /// <summary>
        /// 视频
        /// </summary>
        video,
        /// <summary>
        /// 地理位置
        /// </summary>
        location,
        /// <summary>
        /// 链接
        /// </summary>
        link,
        /// <summary>
        /// 事件，事件又分为多个子类型
        /// </summary>
        Event
    }

    /// <summary>
    /// 请求事件类型：用户发给公众号的事件类型
    /// </summary>
    public enum RequestEventTypeEnum
    {
        /// <summary>
        /// 订阅
        /// </summary>
        subscribe,
        /// <summary>
        /// 取消订阅
        /// </summary>
        unsubscribe,
        /// <summary>
        /// 扫描二维码
        /// </summary>
        SCAN,
        /// <summary>
        /// 上报地理位置
        /// </summary>
        LOCATION,
        /// <summary>
        /// 点击菜单拉取消息的事件推送
        /// </summary>
        CLICK,
        /// <summary>
        /// 点击菜单跳转链接的事件推送
        /// </summary>
        VIEW,
        /// <summary>
        /// 扫码推事件的事件推送
        /// </summary>
        scancode_push,
        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框的事件推送
        /// </summary>
        scancode_waitmsg,
        /// <summary>
        /// 弹出系统拍照发图的事件推送
        /// </summary>
        pic_sysphoto,
        /// <summary>
        /// 弹出拍照或者相册发图的事件推送
        /// </summary>
        pic_photo_or_album,
        /// <summary>
        /// 弹出微信相册发图器的事件推送
        /// </summary>
        pic_weixin,
        /// <summary>
        /// 弹出地理位置选择器的事件推送
        /// </summary>
        location_select,
        /// <summary>
        /// 推送群发消息结果
        /// </summary>
        MASSSENDJOBFINISH,
        /// <summary>
        /// 推送发送模板消息结果
        /// </summary>
        TEMPLATESENDJOBFINISH,
        /// <summary>
        /// 客服接入会话
        /// </summary>
        kf_create_session,
        /// <summary>
        /// 客服关闭会话
        /// </summary>
        kf_close_session,
        /// <summary>
        /// 客服转接会话
        /// </summary>
        kf_switch_session
    }
}