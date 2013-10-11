package peterinor.weixin.entity.receive;

/**
 * Created with IntelliJ IDEA.
 * User: Peterinor
 * Date: 13-10-11
 * Time: 上午9:33
 * To change this template use File | Settings | File Templates.
 */

/**
 *
 * 项目名称：wechatlib
 * 类名称：TextMessage
 * 类描述：文本消息
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:13:06
 * @version
 */
public class TextMessage extends ReceiveBaseMessage{
    // 消息内容
    private String Content;

    public String getContent() {
        return Content;
    }

    public void setContent(String content) {
        Content = content;
    }
}