package peterinor.weixin.entity.receive;

/**
 * Created with IntelliJ IDEA.
 * User: Peterinor
 * Date: 13-10-11
 * Time: 上午9:34
 * To change this template use File | Settings | File Templates.
 */

/**
 *
 * 项目名称：wechatlib
 * 类名称：LinkMessage
 * 类描述：链接消息
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:12:33
 * @version
 */
public class LinkMessage {
    // 消息标题
    private String Title;
    // 消息描述
    private String Description;
    // 消息链接
    private String Url;

    public String getTitle() {
        return Title;
    }

    public void setTitle(String title) {
        Title = title;
    }

    public String getDescription() {
        return Description;
    }

    public void setDescription(String description) {
        Description = description;
    }

    public String getUrl() {
        return Url;
    }

    public void setUrl(String url) {
        Url = url;
    }
}