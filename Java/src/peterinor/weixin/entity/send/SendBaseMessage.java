package peterinor.weixin.entity.send;

/**
 * Created with IntelliJ IDEA.
 * User: Peterinor
 * Date: 13-10-11
 * Time: 上午9:39
 * To change this template use File | Settings | File Templates.
 */
/**
 *
 * 项目名称：wechatlib
 * 类名称：SendBaseMessage
 * 类描述：回复消息基类（公众帐号回复消息给普通用户）
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:11:46
 * @version
 */
public class SendBaseMessage {
    // 接收方帐号（OpenID）
    private String ToUserName;
    // 开发者微信号
    private String FromUserName;
    // 消息创建时间 （整型）
    private long CreateTime;
    // 消息类型（text/music/news）
    private String MsgType;

    public String getToUserName() {
        return ToUserName;
    }

    public void setToUserName(String toUserName) {
        ToUserName = toUserName;
    }

    public String getFromUserName() {
        return FromUserName;
    }

    public void setFromUserName(String fromUserName) {
        FromUserName = fromUserName;
    }

    public long getCreateTime() {
        return CreateTime;
    }

    public void setCreateTime(long createTime) {
        CreateTime = createTime;
    }

    public String getMsgType() {
        return MsgType;
    }

    public void setMsgType(String msgType) {
        MsgType = msgType;
    }
}