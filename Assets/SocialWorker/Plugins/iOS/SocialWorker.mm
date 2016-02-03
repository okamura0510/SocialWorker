//----------------------------------------------
// SocialWorker
// © 2015 yedo-factory
//----------------------------------------------
#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <Social/Social.h>
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>

/** UnitySendMessage：GameObject名 */
static const char *kUnitySendGameObject = "SocialWorker";
/** UnitySendMessage：コールバック名 */
static const char *kUnitySendCallback = "OnSocialWorkerResult";

/** 結果：成功 */
static const char *kResultSuccess = "0";
/** 結果：利用できない */
static const char *kResultNotAvailable = "1";
/** 結果：予期せぬエラー */
static const char *kResultError = "2";

/**
 * SocialWorker
 * @author okamura
 */
@interface SocialWorker : NSObject<UIDocumentInteractionControllerDelegate, MFMailComposeViewControllerDelegate>
@property(nonatomic, retain) UIDocumentInteractionController *_dic;
@end

@implementation SocialWorker
@synthesize _dic;

/**
 * Twitter or Facebook 投稿。ただしFacebookは画像の投稿のみ許可しており、テキストの投稿は無視されることに注意。
 * @param isTwitter true：Twitter、false：Facebook
 * @param message メッセージ
 * @param url URL。空文字の場合は処理されない。
 * @param imagePath 画像パス(PNG/JPGのみ)。空文字の場合は処理されない。
 */
- (void)postTwitterOrFacebook:(BOOL)isTwitter message:(NSString *)message url:(NSString *)url imagePath:(NSString *)imagePath {
    NSString *type = (isTwitter) ? SLServiceTypeTwitter : SLServiceTypeFacebook;
    if ([SLComposeViewController isAvailableForServiceType:type]) {
    	SLComposeViewController *vc = [SLComposeViewController composeViewControllerForServiceType:type];
	    [vc setInitialText:message];
	    if([url length] != 0) {
	        [vc addURL:[NSURL URLWithString:url]];
	    }
	    if([imagePath length] != 0) {
	        [vc addImage:[UIImage imageWithContentsOfFile:imagePath]];
	    }
	    [vc setCompletionHandler:^(SLComposeViewControllerResult result) {}];
	    [UnityGetGLViewController() presentViewController:vc animated:YES completion:nil];
	    UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultSuccess);
    } else {
    	UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultNotAvailable);
    }
}

/**
 * Line投稿。Lineはメッセージと画像の同時投稿は行えないことに注意。
 * @param message メッセージ
 * @param imagePath 画像パス(PNG/JPGのみ)。空文字の場合は処理されない。
 */
- (void)postLine:(NSString *)message imagePath:(NSString *)imagePath {
    NSString *url;
    if([imagePath length] == 0) {
        // メッセージ投稿
        url = [NSString stringWithFormat:@"line://msg/text/%@", message];
    } else {
        // 画像投稿
        UIPasteboard *pasteboard;
        if ([[UIDevice currentDevice].systemVersion floatValue] >= 7.0f) { // Pasteboardの仕様がiOS7から変更された
            pasteboard = [UIPasteboard generalPasteboard];
        } else {
            pasteboard = [UIPasteboard pasteboardWithUniqueName];
        }

        NSString *extension = [[@"." stringByAppendingString:[imagePath pathExtension]] lowercaseString];
        if([extension rangeOfString:@".png"].location != NSNotFound) {
            [pasteboard setData:UIImagePNGRepresentation([UIImage imageWithContentsOfFile:imagePath]) forPasteboardType:@"public.png"];
        } else {
            [pasteboard setData:UIImageJPEGRepresentation([UIImage imageWithContentsOfFile:imagePath], 1.0f) forPasteboardType:@"public.jpeg"];
        }
        url = [NSString stringWithFormat:@"line://msg/image/%@", pasteboard.name];
    }

    NSURL *urlData = [NSURL URLWithString:url];
    if ([[UIApplication sharedApplication] canOpenURL:urlData]) {
        [[UIApplication sharedApplication] openURL:urlData];
        UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultSuccess);
    } else {
        UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultNotAvailable);
    }
}

/**
 * Instagram投稿。Instagramは画像の投稿のみ行える。
 * @param imagePath 画像パス(PNG/JPGのみ)
 */
- (void)postInstagram:(NSString *)imagePath {
    if ([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"instagram://app"]]) {
        // 拡張子「.igo」にすることでアプリを特定させるため、ファイル名を変更して新たに保存
        UIImage *image = [UIImage imageWithContentsOfFile:imagePath];
        NSString *extension = [[@"." stringByAppendingString:[imagePath pathExtension]] lowercaseString];
        imagePath = [imagePath stringByReplacingOccurrencesOfString:extension withString:@".igo"];
        if([extension rangeOfString:@".png"].location != NSNotFound) {
            [UIImagePNGRepresentation(image) writeToFile:imagePath atomically:YES];
        } else {
            [UIImageJPEGRepresentation(image, 1.0f) writeToFile:imagePath atomically:YES];
        }

        UIViewController *unityView = UnityGetGLViewController();
        _dic = [UIDocumentInteractionController interactionControllerWithURL:[NSURL fileURLWithPath:imagePath]];
        _dic.UTI = @"com.instagram.exclusivegram";
        _dic.delegate = self;
        if ([_dic presentOpenInMenuFromRect:unityView.view.frame inView:unityView.view animated:YES]) {
            UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultSuccess);
        } else {
            UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultError);
        }
    } else {
        UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultNotAvailable);
    }
}

/**
 * UIDocumentInteractionController起動
 */
- (void)documentInteractionController:(UIDocumentInteractionController *)controller willBeginSendingToApplication:(NSString *)application {}

/**
 * UIDocumentInteractionController送信完了
 */
- (void)documentInteractionController:(UIDocumentInteractionController *)controller didEndSendingToApplication:(NSString *)application {
    self._dic = nil;
}

/**
 * UIDocumentInteractionControllerキャンセル
 */
- (void)documentInteractionControllerDidDismissOpenInMenu: (UIDocumentInteractionController *) controller {
    self._dic = nil;
}

/**
 * メール投稿
 * @param to 宛先。カンマ区切りの配列。
 * @param cc CC。カンマ区切りの配列。
 * @param bcc BCC。カンマ区切りの配列。
 * @param subject タイトル
 * @param message メッセージ
 * @param imagePath 画像パス(PNG/JPGのみ)。空文字の場合は処理されない。
 */
- (void)postMail:(NSString *)to cc:(NSString *)cc bcc:(NSString *)bcc subject:(NSString *)subject message:(NSString *)message imagePath:(NSString *)imagePath {
    if([MFMailComposeViewController canSendMail]) {
        MFMailComposeViewController *vc = [[MFMailComposeViewController alloc] init];
        vc.mailComposeDelegate = self;
        [vc setToRecipients:[to componentsSeparatedByString:@","]];
        [vc setCcRecipients:[cc componentsSeparatedByString:@","]];
        [vc setBccRecipients:[bcc componentsSeparatedByString:@","]];
        [vc setSubject:subject];
        [vc setMessageBody:message isHTML:NO];
        if([imagePath length] != 0) {
			NSString *extension = [[@"." stringByAppendingString:[imagePath pathExtension]] lowercaseString];
            if([extension rangeOfString:@".png"].location != NSNotFound) {
                [vc addAttachmentData:UIImagePNGRepresentation([UIImage imageWithContentsOfFile:imagePath]) mimeType:@"image/png" fileName:@"image.png"];
            } else {
                [vc addAttachmentData:UIImageJPEGRepresentation([UIImage imageWithContentsOfFile:imagePath], 1.0f) mimeType:@"image/jpeg" fileName:@"image.jpeg"];
            }
        }
        [UnityGetGLViewController() presentViewController:vc animated:YES completion:nil];
        UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultSuccess);
    } else {
        UnitySendMessage(kUnitySendGameObject, kUnitySendCallback, kResultNotAvailable);
    }
}

/** 
 * メール結果
 */
- (void)mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError *)error {
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
}

/**
 * アプリ選択式での投稿
 * @param message メッセージ
 * @param imagePath 画像パス(PNG/JPGのみ)。空文字の場合は処理されない。
 */
- (void)createChooser:(NSString *)message imagePath:(NSString *)imagePath {
    NSArray *activities = [NSArray arrayWithObjects:message, nil];
	if([imagePath length] != 0) {
		activities = [activities arrayByAddingObject:[UIImage imageWithContentsOfFile:imagePath]];
	}
    
    UIActivityViewController *vc = [[UIActivityViewController alloc] initWithActivityItems:activities applicationActivities:nil];
	if ([[UIDevice currentDevice].systemVersion floatValue] > 7.1f) {
        vc.popoverPresentationController.sourceView = UnityGetGLViewController().view;
	}
    [UnityGetGLViewController() presentViewController:vc animated:YES completion:nil];
}
@end

/**
 * Unityから呼び出されるネイティブコード
 */
extern "C" {
    static SocialWorker *worker =[[SocialWorker alloc] init];
    UIViewController *UnityGetGLViewController();
    void UnitySendMessage(const char *, const char *, const char *);
    
    static NSString *getStr(char *str){
        if (str) {
            return [NSString stringWithCString: str encoding:NSUTF8StringEncoding];
        } else {
            return [NSString stringWithUTF8String: ""];
        }
    }
    
	void postTwitterOrFacebook(BOOL isTwitter, char *message, char *url, char *imagePath){
        [worker postTwitterOrFacebook:isTwitter message:getStr(message) url:getStr(url) imagePath:getStr(imagePath)];
    }

	void postLine(char *message, char *imagePath){
        [worker postLine:getStr(message) imagePath:getStr(imagePath)];
    }

	void postInstagram(char *imagePath){
        [worker postInstagram:getStr(imagePath)];
    }

    void postMail(char *to, char *cc, char *bcc, char *subject, char *message, char *imagePath){
        [worker postMail:getStr(to) cc:getStr(cc) bcc:getStr(bcc) subject:getStr(subject) message:getStr(message) imagePath:getStr(imagePath)];
    }

	void createChooser(char *message, char *imagePath){
        [worker createChooser:getStr(message) imagePath:getStr(imagePath)];
    }
}
