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
 * 类名称：TextMessage
 * 类描述：文本消息
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:12:00
 * @version
 */
public class TextMessage extends SendBaseMessage{
    // 消息内容
    private String Content;

    public String getContent() {
        return Content;
    }

    public void setContent(String content) {
        Content = content;
    }
}