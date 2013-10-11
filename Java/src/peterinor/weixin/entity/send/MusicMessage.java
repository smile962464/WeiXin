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
 * 类名称：MusicMessage
 * 类描述：音乐消息
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:11:19
 * @version
 */
public class MusicMessage extends SendBaseMessage{
    // 音乐
    private Music Music;

    public Music getMusic() {
        return Music;
    }

    public void setMusic(Music music) {
        Music = music;
    }
}