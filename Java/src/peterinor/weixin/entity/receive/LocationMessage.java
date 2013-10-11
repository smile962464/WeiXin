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
 * 类名称：LocationMessage
 * 类描述：地理位置消息
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:12:45
 * @version
 */
public class LocationMessage extends ReceiveBaseMessage{
    // 地理位置维度
    private String Location_X;
    // 地理位置经度
    private String Location_Y;
    // 地图缩放大小
    private String Scale;
    // 地理位置信息
    private String Label;

    public String getLocation_X() {
        return Location_X;
    }

    public void setLocation_X(String location_X) {
        Location_X = location_X;
    }

    public String getLocation_Y() {
        return Location_Y;
    }

    public void setLocation_Y(String location_Y) {
        Location_Y = location_Y;
    }

    public String getScale() {
        return Scale;
    }

    public void setScale(String scale) {
        Scale = scale;
    }

    public String getLabel() {
        return Label;
    }

    public void setLabel(String label) {
        Label = label;
    }
}