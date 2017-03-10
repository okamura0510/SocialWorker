//----------------------------------------------
// SocialWorker
// © 2015 yedo-factory
// Version 1.0.4
//----------------------------------------------
SocialWorkerはTwitter、Facebook、Line、Instagram、メールへの連携を簡単に行うことが出来るAssetである!
※OAuth認証の仕組みは行っていない

▽Twitter
メッセージ、URL、画像の投稿をサポート。

▽Facebook
画像の投稿をサポート。Facebookは仕様で、画像の投稿のみを許可している。

▽Line
メッセージと画像の投稿をサポート。ただしメッセージと画像の同時投稿は行えない。

▽Instagram
画像の投稿をサポート。

▽メール
メッセージと画像の投稿をサポート。

▽アプリ選択式での投稿
メッセージと画像の投稿をサポート。

■使用方法
(1)[Prefabs/SocialWorker]をHierarchyに配置
(2)Scriptから[SocialWorker.PostTwitter][SocialWorker.PostFacebook][SocialWorker.PostLine][SocialWorker.PostInstagram][SocialWorker.PostMail][SocialWorker.CreateChooser]を呼ぶ

■バージョン履歴

1.0.4
- mod : SocialWorkerPostProcessBuildを「UnityEditor.iOS.Xcodeライブラリ」を使うように修正
- mod : SocialWorker.mmに「MessageUI.framework」「Social.framework」のプラットフォーム設定を追加(Plugin Inspector)
- mod : Unityのサポートバージョンを 5.5.0+ に修正
- add : metaデータ追加

1.0.3
- mod : AndroidのIntent取得の方法をパッケージ名で判定するように修正

1.0.2
- mod : FacebookのIntent投稿が失敗するバグを修正

1.0.1
- add : [Editor/SocialWorkerPostProcessBuild]追加。iOS9でカスタムURLスキームがinfo.plistに記載されていない場合にエラーになる問題の対応。

1.0.0
- 初期バージョン
