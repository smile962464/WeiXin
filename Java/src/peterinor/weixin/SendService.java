package peterinor.weixin;

/**
 * Created with IntelliJ IDEA.
 * User: Peterinor
 * Date: 13-10-11
 * Time: 上午9:45
 * To change this template use File | Settings | File Templates.
 */

import peterinor.weixin.entity.send.Article;
import peterinor.weixin.entity.send.NewsMessage;
import peterinor.weixin.entity.send.TextMessage;
import peterinor.weixin.utils.MessageUtil;

import java.util.Date;
import java.util.List;



/**
 *
 * 项目名称：wechatlib
 * 类名称：SendService
 * 类描述： 回复信息类型的封装
 * 创建人：WQ
 * 创建时间：2013-10-3 下午4:07:59
 * @version
 */
public class SendService {
    /**
     * 回复文本消息
     * @param fromusername 粉丝openid
     * @param tousername 微信公众号
     * @param respContent 回复信息
     * @return
     */
    public static String sendTextmessage(String fromusername,String tousername,String respContent){
        //初始化回复信息
        String respmessage;
        //回复文本消息
        TextMessage textMessage = new TextMessage();
        //发送方帐号（一个OpenID）
        textMessage.setToUserName(fromusername);
        //开发者微信号
        textMessage.setFromUserName(tousername);
        //消息创建时间 （整型）
        textMessage.setCreateTime(new Date().getTime());
        //消息类型text
        textMessage.setMsgType(MessageUtil.SEND_TEXT);
        //回复的消息内容,长度不超过2048字节
        textMessage.setContent(respContent);
        //转为xml格式
        respmessage = MessageUtil.textMessageToXml(textMessage);
        //返回回复信息
        return respmessage;
    }

    /**
     * 图文消息设置
     * @param fromusername 粉丝openid
     * @param tousername   开发者微信公众账号
     * @param newslist     图文消息list
     * @return
     */
    public static String sendNewsmessage(String fromusername,String tousername,List<Article> newslist){
        //初始化回复信息
        String respmessage;
        //创建图文消息
        NewsMessage newsMessage=new NewsMessage();
        //发送方帐号（一个OpenID）
        newsMessage.setToUserName(fromusername);
        //开发者微信号
        newsMessage.setFromUserName(tousername);
        //消息创建时间 （整型）
        newsMessage.setCreateTime(new Date().getTime());
        //消息类型news
        newsMessage.setMsgType(MessageUtil.SEND_NEWS);
        //图文消息个数，限制为10条以内
        newsMessage.setArticleCount(newslist.size());
        //多条图文消息信息，默认第一个item为大图
        newsMessage.setArticles(newslist);
        //转成xml形式
        respmessage = MessageUtil.newsMessageToXml(newsMessage);
        //回复信息
        return respmessage;
    }
}