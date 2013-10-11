package peterinor.weixin.entity.send;

/**
 * Created with IntelliJ IDEA.
 * User: Peterinor
 * Date: 13-10-11
 * Time: 上午9:41
 * To change this template use File | Settings | File Templates.
 */
/**
 *
 * 项目名称：wechatlib
 * 类名称：Article
 * 类描述：图文model
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:10:51
 * @version
 */
public class Article {
    // 图文消息名称
    private String Title;
    // 图文消息描述
    private String Description;
    // 图片链接，支持JPG、PNG格式，较好的效果为大图600*300，小图80*80，限制图片链接的域名需要与开发者填写的基本资料中的Url一致
    private String PicUrl;
    // 点击图文消息跳转链接
    private String Url;

    public String getTitle() {
        return Title;
    }

    public void setTitle(String title) {
        Title = title;
    }

    public String getDescription() {
        return null == Description ? "" : Description;
    }

    public void setDescription(String description) {
        Description = description;
    }

    public String getPicUrl() {
        return null == PicUrl ? "" : PicUrl;
    }

    public void setPicUrl(String picUrl) {
        PicUrl = picUrl;
    }

    public String getUrl() {
        return null == Url ? "" : Url;
    }

    public void setUrl(String url) {
        Url = url;
    }
}