# SocialWorker

![SocialWorker](https://qiita-image-store.s3.amazonaws.com/0/98018/dc39c8a8-ae3b-5323-d953-3b97e5a8cfa4.png)

SocialWorkerは、iOS/AndroidでのTwitter、Facebook、Line、Instagram、メールへの連携を簡単に行うことが出来るUnityAssetです。  
詳しくは以下をご確認下さい。  
  
[Japanese](http://qiita.com/yedo/items/7e76dbf58bab34042bc1)/[English](http://qiita.com/yedo/items/c53beabcc6e75a64ed2a)

## Description

SocialWorkerは各種SNSへの簡単なデータの受け渡しをサポートしています。そのため、連携可能なデータはメッセージと画像のみです(SNSによっては微妙に追加で渡せるデータもあったりしますが)。  
連携方法は、iOSではURLSchemeを主に使用しており、AndroidではIntentによるデータの受け渡しの方法を取っています。  

## Requirement

Unity 5.0+  
iOS 6.0+  
Android 2.3+

## Usage

#### 1. SocialWorker Prefab の設置

[SocialWorker/Prefabs/SocialWorker]をHierarchyに設置。

#### 2. スクリプトから連携メソッドを呼ぶ

    SocialWorker.PostTwitter(string message, string url, string imagePath, Action<SocialWorkerResult> onResult = null)
    SocialWorker.PostFacebook(string imagePath, Action<SocialWorkerResult> onResult = null)
    SocialWorker.PostLine(string message, string imagePath, Action<SocialWorkerResult> onResult = null)
    SocialWorker.PostInstagram(string imagePath, Action<SocialWorkerResult> onResult = null)
    SocialWorker.PostMail(string[] to, string[] cc, string[] bcc, string subject, string message, string imagePath, Action<SocialWorkerResult> onResult = null)
    SocialWorker.CreateChooser(string message, string imagePath, Action<SocialWorkerResult> onResult = null)

![ss_0.png](https://qiita-image-store.s3.amazonaws.com/0/98018/c6f72970-9c06-44eb-e226-ba75075c5ff8.png)
![ss_1.png](https://qiita-image-store.s3.amazonaws.com/0/98018/2940c110-7763-529d-bea8-d05ec29bb8f2.png)
![ss_2.png](https://qiita-image-store.s3.amazonaws.com/0/98018/a4e8319d-e71c-2190-960a-e8241dbf72df.png)
![ss_3.png](https://qiita-image-store.s3.amazonaws.com/0/98018/90ee0e20-5497-b7c1-994b-f7b0cb97cc6f.png)
![ss_4.png](https://qiita-image-store.s3.amazonaws.com/0/98018/ac1e9b54-9bc5-df8e-903c-af383643f3af.png)
![ss_5.png](https://qiita-image-store.s3.amazonaws.com/0/98018/39c98e18-0e5f-01f7-9e99-d5ef71ebabcb.png)

## Install

本プロジェクトをUnity上にそのままインポートするか、トップ階層にある SocialWorker.unitypackage をご使用下さい。

## Licence

The MIT License (MIT)  
  
Copyright (c) 2015 yedo-factory  
  
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:  
  
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.  
  
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Author

[yedo-factory](http://yedo-factory.co.jp/)
