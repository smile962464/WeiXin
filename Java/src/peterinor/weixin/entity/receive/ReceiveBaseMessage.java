/**
 * Created with IntelliJ IDEA.
 * User: Peterinor
 * Date: 13-10-11
 * Time: 上午9:28
 * To change this template use File | Settings | File Templates.
 */
package peterinor.weixin.entity.receive;


/**
 *
 * 项目名称：wechatlib
 * 类名称：ReceiveBaseMessage
 * 类描述：接收消息基类（普通用户发送信息给公众帐号）
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:12:57
 * @version
 */
public class ReceiveBaseMessage {
    // 开发者微信号
    private String ToUserName;
    // 发送方帐号（OpenID）
    private String FromUserName;
    // 消息创建时间 （整型）
    private long CreateTime;
    // 消息类型（text/image/location/link）
    private String MsgType;
    // 消息id，64位整型
    private long MsgId;

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

    public long getMsgId() {
        return MsgId;
    }

    public void setMsgId(long msgId) {
        MsgId = msgId;
    }
}