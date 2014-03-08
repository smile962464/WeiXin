using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Xml;

using WeiXin;
using WeiXin.Models;

namespace WeiXin.Test
{
    [TestClass]
    public class UtitiesTest
    {
        [TestMethod]
        public void SendServiceMessageTest()
        {
            var accessToken = WeiXin.Utities.AccessToken();

            string toUser = "o3bikjlRrS1M3DHnYhFY7JnJvlQo";//ty
            //toUser = "o3bikjoWRHyYS8ZrKB7foI3On2-M";
            //Text
            RTextMessage tmsg = new RTextMessage("avt7", toUser, "TextMessage Test...");
            Utities.SendServiceMessage(accessToken.accessToken.Access_Token, tmsg);

            //Image
            RImageMessage imsg = new RImageMessage("avt7", toUser);
            imsg.Image = new RImageMessage.ImageMeta("vkErhevHyJCw0EJGPDb-uUewB9LoGMoP1rB4Bf6Bsa06AN2YPRAmHYxHljaOiLc6");
            Utities.SendServiceMessage(accessToken.accessToken.Access_Token, imsg);

            //Music
            RMusicMessage music = new RMusicMessage("avt7", toUser);
            music.Music.Description = "做我老婆好不好";
            music.Music.MusicUrl = "http://zhangmenshiting.baidu.com/data2/music/13124777/13124778151200128.mp3?xcode=360e4a81afd2f2974d74257b0854e7ec5c14833120133308";
            music.Music.HQMusicUrl = music.Music.MusicUrl;
            music.Music.ThumbMediaId = "";
            Utities.SendServiceMessage(accessToken.accessToken.Access_Token, music);

        }

        [TestMethod]
        public void Upload_DownloadMediaTest()
        {
            var accessToken = WeiXin.Utities.AccessToken();

            var r = Utities.UploadMedia(accessToken.accessToken.Access_Token, "image", "D:\\Server\\XiaMenAir\\Data\\upload.jpg");

            var mediaId = r.mediaUploadStatus.MediaId;
            Utities.DownloadMedia(accessToken.accessToken.Access_Token, mediaId, "D:\\Server\\XiaMenAir\\Data\\", mediaId);
        }

        [TestMethod]
        public void GetUserInfoTest()
        {
            var accessToken = WeiXin.Utities.AccessToken();

            var list = WeiXin.Utities.GetSubscriberList(accessToken.accessToken.Access_Token);

            WeiXin.Models.API.UserInfo ty = WeiXin.Utities.GetUserInfo(accessToken.accessToken.Access_Token, "o3bikjlRrS1M3DHnYhFY7JnJvlQo").userInfo;

            Assert.AreEqual(ty.NickName, "Peterinor");
            Assert.AreEqual(ty.Province, "福建");

        }

        [TestMethod]
        public void GroupTest()
        {
            var accessToken = WeiXin.Utities.AccessToken();

            var r = WeiXin.Utities.AddGroup(accessToken.accessToken.Access_Token, "测试组");
            var d = r.addGroupResult.Group;
            string groupId = Convert.ToString(d.id);

            var groups = WeiXin.Utities.GetAllGroups(accessToken.accessToken.Access_Token);

            var userGroup = WeiXin.Utities.GetUserGroup(accessToken.accessToken.Access_Token, "o3bikjlRrS1M3DHnYhFY7JnJvlQo");

            var status = WeiXin.Utities.MoveUserToGroup(accessToken.accessToken.Access_Token, "o3bikjlRrS1M3DHnYhFY7JnJvlQo", groupId);

            var mod = WeiXin.Utities.ModeifyGroup(accessToken.accessToken.Access_Token, groupId, "测试2");
        }
    }
}
