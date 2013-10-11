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
 * 类名称：ImageMessage
 * 类描述：图片消息
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:12:19
 * @version
 */
public class ImageMessage extends ReceiveBaseMessage{
    // 图片链接
    private String PicUrl;

    public String getPicUrl() {
        return PicUrl;
    }

    public void setPicUrl(String picUrl) {
        PicUrl = picUrl;
    }
}