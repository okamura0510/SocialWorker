//----------------------------------------------
// SocialWorker
// © 2015 yedo-factory
//----------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace SWorker
{
    /// <summary>
    /// デモシーン
    /// </summary>
	public class Demo : MonoBehaviour
	{
		static readonly string ExtensionImage = ".png";
//		static readonly string ExtensionImage = ".jpeg";

		public Text Result;
		public RawImage Image;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // Post画像は端末から読み込むので、ない場合はあらかじめ保存しておくこと
			var imagePath = Application.persistentDataPath + "/image" + ExtensionImage;
            if (!File.Exists(imagePath)) 
            {
				var texture = (Texture2D)Image.texture;
				var data    = (ExtensionImage == ".png") ? texture.EncodeToPNG () : texture.EncodeToJPG ();
				File.WriteAllBytes(imagePath, data); 
            }
        }

        /// <summary>
        /// Twitter投稿
        /// </summary>
        public void OnPostTwitter()
        {
			var message   = "message";
			var url       = "http://yedo-factory.co.jp/";
			var imagePath = Application.persistentDataPath + "/image" + ExtensionImage;
			SocialWorker.PostTwitter(message, url, imagePath, OnResult);
//			SocialWorker.PostTwitter(message, "", OnResult);
//			SocialWorker.PostTwitter("", imagePath, OnResult);
        }

        /// <summary>
        /// Facebook投稿。ただしFacebookは画像の投稿のみ許可しており、テキストの投稿は無視されることに注意。
        /// </summary>
        public void OnPostFacebook()
        {
			var imagePath = Application.persistentDataPath + "/image" + ExtensionImage;
			SocialWorker.PostFacebook(imagePath, OnResult);
        }

        /// <summary>
        /// Line投稿。Lineはメッセージと画像の同時投稿は行えないことに注意。
        /// </summary>
        public void OnPostLine()
        {
			var message   = "message";
			var imagePath = Application.persistentDataPath + "/image" + ExtensionImage;
			SocialWorker.PostLine(message, imagePath, OnResult);
//			SocialWorker.PostLine(message, "", OnResult);
//			SocialWorker.PostLine("", imagePath, OnResult);
        }

        /// <summary>
        /// Instagram投稿。Instagramは画像の投稿のみ行える。
        /// </summary>
        public void OnPostInstagram()
        {
			var imagePath = Application.persistentDataPath + "/image" + ExtensionImage;
			SocialWorker.PostInstagram(imagePath, OnResult);
        }

        /// <summary>
        /// メール投稿
        /// </summary>
        public void OnPostMail()
        {
			var to        = new string[] { "to@hoge.com" };
			var cc        = new string[] { "cc@hoge.com" };
			var bcc       = new string[] { "bcc@hoge.com" };
			var subject   = "subject";
			var message   = "message";
			var imagePath = Application.persistentDataPath + "/image" + ExtensionImage;
			SocialWorker.PostMail(to, cc, bcc, subject, message, imagePath, OnResult);
//			SocialWorker.PostMail(message, "", OnResult);
//			SocialWorker.PostMail("", imagePath, OnResult);
        }

        /// <summary>
        /// アプリ選択式での投稿
        /// </summary>
        public void OnCreateChooser()
        {
			var message   = "message";
			var imagePath = Application.persistentDataPath + "/image" + ExtensionImage;
			SocialWorker.CreateChooser(message, imagePath, OnResult);
//			SocialWorker.CreateChooser(message, "", OnResult);
//			SocialWorker.CreateChooser("", imagePath, OnResult);
        }

        /// <summary>
        /// 結果コールバック
        /// </summary>
        /// <param name="res">結果値</param>
        public void OnResult(SocialWorkerResult res)
        {
            switch(res)
            {
				case SocialWorkerResult.Success:
					Result.text = "Result : Success";
                    break;
                case SocialWorkerResult.NotAvailable:
					Result.text = "Result : NotAvailable";
                    break;
                case SocialWorkerResult.Error:
					Result.text = "Result : Error";
                    break;
            }
        }
	}
}