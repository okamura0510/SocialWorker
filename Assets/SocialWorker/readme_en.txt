//----------------------------------------------
// SocialWorker
// © 2015 yedo-factory
// Version 1.0.2
//----------------------------------------------
SocialWorker is Asset which can post Twitter, Facebook, Line, Instagram and Mail easily!
※OAuth authentication isn't being used.

▽Twitter
Post of message, url and image is supported.

▽Facebook
Post of image is supported. Facebook permits only post of image by the specification.

▽Line
Post of message and image is supported. But, message and image can't be post at the same time.

▽Instagram
Post of image is supported.

▽Mail
Post of message and image is supported.

▽Chooser
Post of message and image is supported.

■How to use
(1)[Prefabs/SocialWorker] is set Hierarchy.
(2)[SocialWorker.PostTwitter][SocialWorker.PostFacebook][SocialWorker.PostLine][SocialWorker.PostInstagram][SocialWorker.PostMail][SocialWorker.CreateChooser] is called.

■Version History

1.0.2
- mod : Modify facebook intent post error. 

1.0.1
- add : [Editor/SocialWorkerPostProcessBuild]. Custom URL Scheme has to be written in info.plist in iOS9, modify of the bug URL doesn't open.

1.0.0
- First release