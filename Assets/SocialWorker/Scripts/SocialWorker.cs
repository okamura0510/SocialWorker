//----------------------------------------------
// SocialWorker
// © 2015 yedo-factory
//----------------------------------------------
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace SWorker
{
    /// <summary>
    /// SocialWorker
    /// </summary>
    public class SocialWorker : MonoBehaviour
	{
        /// <summary>
        /// ネイティブプラグイン定義
        /// </summary>
#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void postMail(string to, string cc, string bcc, string subject, string message, string imagePath);
        [DllImport("__Internal")]
        private static extern void postTwitterOrFacebook(bool isTwitter, string message, string url, string imagePath);
        [DllImport("__Internal")]
        private static extern void postLine(string message, string imagePath);
        [DllImport("__Internal")]
        private static extern void postInstagram(string imagePath);
        [DllImport("__Internal")]
        private static extern void createChooser(string message, string imagePath);
#elif UNITY_ANDROID
        private static AndroidJavaObject worker = null;
#endif

        /// <summary>
        /// 結果コールバック
        /// </summary>
        private static Action<SocialWorkerResult> onResult = null;

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
#if !UNITY_EDITOR && UNITY_ANDROID
            worker = new AndroidJavaObject("com.yedo.socialworker.SocialWorker");
#endif
        }

        /// <summary>
        /// Twitter投稿
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)。空文字の場合は処理されない。</param>
        /// <param name="onResult">結果コールバック</param>
        public static void PostTwitter(string message, string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            PostTwitter(message, null, imagePath, onResult);
        }

        /// <summary>
        /// Twitter投稿
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="url">URL。空文字の場合は処理されない。</param>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)。空文字の場合は処理されない。</param>
        /// <param name="onResult">結果コールバック</param>
        public static void PostTwitter(string message, string url, string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            if (message == null) { message = ""; }
            if (url == null) { url = ""; }
            if (imagePath == null) { imagePath = ""; }
            SocialWorker.onResult = onResult;
#if UNITY_IPHONE
            postTwitterOrFacebook(true, message, url, imagePath);
#elif UNITY_ANDROID
            worker.Call("postTwitterOrFacebook", true, message, url, imagePath);
#endif
        }

        /// <summary>
        /// Facebook投稿。ただしFacebookは画像の投稿のみ許可しており、テキストの投稿は無視されることに注意。
        /// </summary>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)。空文字の場合は処理されない。</param>
        /// <param name="onResult">結果コールバック</param>
        public static void PostFacebook(string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            if (imagePath == null) { imagePath = ""; }
            SocialWorker.onResult = onResult;
#if UNITY_IPHONE
			postTwitterOrFacebook(false, "", "", imagePath);
#elif UNITY_ANDROID
			worker.Call("postTwitterOrFacebook", false, "", "", imagePath);
#endif
        }

        /// <summary>
        /// Line投稿。Lineはメッセージと画像の同時投稿は行えないことに注意。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)。空文字の場合は処理されない。</param>
        /// <param name="onResult">結果コールバック</param>
        public static void PostLine(string message, string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            if (message == null) { message = ""; }
            if (imagePath == null) { imagePath = ""; }
			SocialWorker.onResult = onResult;
#if UNITY_IPHONE
			postLine(message, imagePath);
#elif UNITY_ANDROID
			worker.Call("postLine", message, imagePath);
#endif
        }

        /// <summary>
		/// Instagram投稿。Instagramは画像の投稿のみ行える。
        /// </summary>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)</param>
        /// <param name="onResult">結果コールバック</param>
        public static void PostInstagram(string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            if (imagePath == null) { imagePath = ""; }
            SocialWorker.onResult = onResult;
#if UNITY_IPHONE
            postInstagram(imagePath);
#elif UNITY_ANDROID
            worker.Call("postInstagram", imagePath);
#endif
        }

        /// <summary>
        /// メール投稿
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)。空文字の場合は処理されない。</param>
        /// <param name="onResult">結果コールバック</param>
        public static void PostMail(string message, string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            PostMail(null, null, null, null, message, imagePath, onResult);
        }

        /// <summary>
        /// メール投稿
        /// </summary>
        /// <param name="to">宛先。カンマ区切りの配列。</param>
        /// <param name="cc">CC。カンマ区切りの配列。</param>
        /// <param name="bcc">BCC。カンマ区切りの配列。</param>
        /// <param name="subject">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)。空文字の場合は処理されない。</param>
        /// <param name="onResult">結果コールバック</param>
        public static void PostMail(string[] to, string[] cc, string[] bcc, string subject, string message, string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            if (to == null) { to = new string[] { "" }; }
            if (cc == null) { cc = new string[] { "" }; }
            if (bcc == null) { bcc = new string[] { "" }; }
            if (subject == null) { subject = ""; }
            if (message == null) { message = ""; }
            if (imagePath == null) { imagePath = ""; }
            SocialWorker.onResult = onResult;
#if UNITY_IPHONE
            postMail(string.Join(",", to), string.Join(",", cc), string.Join(",", bcc), subject, message, imagePath);
#elif UNITY_ANDROID
            worker.Call("postMail", string.Join(",", to), string.Join(",", cc), string.Join(",", bcc), subject, message, imagePath);
#endif
        }

        /// <summary>
        /// アプリ選択式での投稿
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="imagePath">画像パス(PNG/JPGのみ)。空文字の場合は処理されない。</param>
        /// <param name="onResult">結果コールバック</param>
        public static void CreateChooser(string message, string imagePath, Action<SocialWorkerResult> onResult = null)
        {
            if (message == null) { message = ""; }
            if (imagePath == null) { imagePath = ""; }
            SocialWorker.onResult = onResult;
#if UNITY_IPHONE
            createChooser(message, imagePath);
#elif UNITY_ANDROID
            worker.Call("createChooser", message, imagePath);
#endif
        }

        /// <summary>
        /// 結果コールバック。ネイティブプラグイン側から呼ばれるコールバック。
        /// </summary>
        /// <param name="res">結果値</param>
        public void OnSocialWorkerResult(string res)
        {
            if (onResult != null) 
            {
                onResult((SocialWorkerResult)int.Parse(res));
                onResult = null;
            }
        }
	}

    /// <summary>
    /// 結果値
    /// </summary>
    public enum SocialWorkerResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 利用できない
        /// </summary>
        NotAvailable = 1,
        /// <summary>
        /// 予期せぬエラー
        /// </summary>
        Error = 2
    }
}