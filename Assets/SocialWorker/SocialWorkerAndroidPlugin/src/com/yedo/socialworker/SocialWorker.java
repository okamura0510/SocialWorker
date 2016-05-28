//----------------------------------------------
// SocialWorker
// © 2015 yedo-factory
//----------------------------------------------
package com.yedo.socialworker;

import java.io.File;
import java.net.URLEncoder;
import java.util.List;
import java.util.Locale;

import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.pm.ResolveInfo;
import android.net.Uri;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

/**
 * SocialWorker
 * @author okamura
 */
public class SocialWorker {
	/** タグ */
	public static final String TAG = SocialWorker.class.getSimpleName();
	/** 改行 */
	public static final String BR = System.getProperty("line.separator");
	
	/** UnitySendMessage：GameObject名 */
	public static final String UNITY_SEND_GAMEOBJECT = "SocialWorker";
	/** UnitySendMessage：コールバック名 */
	public static final String UNITY_SEND_CALLBACK = "OnSocialWorkerResult";
	
	/** 結果：成功 */
	public static final String RESULT_SUCCESS = "0";
	/** 結果：利用できない */
	public static final String RESULT_NOT_AVAILABLE = "1";
	/** 結果：予期せぬエラー */
	public static final String RESULT_ERROR = "2";
	
	/**
	 * Twitter or Facebook 投稿。ただしFacebookは画像の投稿のみ許可しており、テキストの投稿は無視されることに注意。
	 * @param isTwitter true：Twitter、false：Facebook
	 * @param message メッセージ
	 * @param url URL。空文字の場合は処理されない。
	 * @param imagePath 画像パス(PNG/JPGのみ)。空文字の場合は処理されない。
	 */
	public void postTwitterOrFacebook(boolean isTwitter, String message, String url, String imagePath) {
		try {
			String name = (isTwitter) ? "com.twitter" : "com.facebook";
			String type = (imagePath.equals("")) ? "text/plain" : getIntentTypeForImage(imagePath);
			Intent intent = createAppIntent(name, Intent.ACTION_SEND, type);
    		if(intent != null) {
    			intent.putExtra(Intent.EXTRA_TEXT, message + BR + url);
     			if(!imagePath.equals("")) {
    				intent.putExtra(Intent.EXTRA_STREAM, Uri.fromFile(new File(imagePath)));
    			}
    			UnityPlayer.currentActivity.startActivity(intent);
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_SUCCESS);
    		} else {
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_NOT_AVAILABLE);
    		}
    	} catch(Exception e) {
    		Log.e(TAG, "postTwitterOrFacebook", e);
    		UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_ERROR);
    	}
	}
	
	/**
	 * Line投稿。Lineはメッセージと画像の同時投稿は行えないことに注意。
	 * @param message メッセージ
	 * @param imagePath 画像パス(PNG/JPGのみ)。空文字の場合は処理されない。
	 */
	public void postLine(String message, String imagePath) {
		try {
			Intent intent = createAppIntent("jp.naver.line", Intent.ACTION_SEND, "text/plain");
			if(intent != null) {
    			if(imagePath.equals("")) {
    				intent = new Intent(Intent.ACTION_VIEW, Uri.parse("line://msg/text/" + URLEncoder.encode(message, "UTF-8")));
    			} else {
    				intent = new Intent(Intent.ACTION_VIEW, Uri.parse("line://msg/image/" + imagePath));
    			}
    			UnityPlayer.currentActivity.startActivity(intent);
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_SUCCESS);
    		} else {
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_NOT_AVAILABLE);
    		}
    	} catch(Exception e) {
    		Log.e(TAG, "postLine", e);
    		UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_ERROR);
    	}
	}
	
    /**
     * Instagram投稿。Instagramは画像の投稿のみ行える。
     * @param imagePath 画像パス(PNG/JPGのみ)
     */
    public void postInstagram(String imagePath) {
    	try {
    		Intent intent = createAppIntent("com.instagram", Intent.ACTION_SEND, getIntentTypeForImage(imagePath));
			if(intent != null) {
				intent.putExtra(Intent.EXTRA_STREAM, Uri.fromFile(new File(imagePath)));
				UnityPlayer.currentActivity.startActivity(intent);
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_SUCCESS);
    		} else {
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_NOT_AVAILABLE);
    		}
    	} catch(Exception e) {
    		Log.e(TAG, "postInstagram", e);
    		UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_ERROR);
    	}
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
    public void postMail(String to, String cc, String bcc, String subject, String message, String imagePath) {
    	try {
    		Intent intent = createAppIntent(null, Intent.ACTION_SEND, "message/rfc822");
    		if(intent != null) {
    			intent.putExtra(Intent.EXTRA_EMAIL, to.split(","));
    			intent.putExtra(Intent.EXTRA_CC, cc.split(","));
    			intent.putExtra(Intent.EXTRA_BCC, bcc.split(","));
    			intent.putExtra(Intent.EXTRA_SUBJECT, subject);
    			intent.putExtra(Intent.EXTRA_TEXT, message);
    			if(!imagePath.equals("")) {
    				intent.putExtra(Intent.EXTRA_STREAM, Uri.fromFile(new File(imagePath)));
    			}
    			UnityPlayer.currentActivity.startActivity(intent);
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_SUCCESS);
    		} else {
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_NOT_AVAILABLE);
    		}
    	} catch(Exception e) {
    		Log.e(TAG, "postMail", e);
    		UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_ERROR);
    	}
    }
    
    /**
     * アプリ選択式での投稿
     * @param message メッセージ
     * @param imagePath 画像パス(PNG/JPGのみ)。空文字の場合は処理されない。
     */
    public void createChooser(String message, String imagePath) {
    	try {
    		String type = (imagePath.equals("")) ? "text/plain" : getIntentTypeForImage(imagePath);
    		Intent intent = createAppIntent(null, Intent.ACTION_SEND, type);
    		if(intent != null) {
    			intent.putExtra(Intent.EXTRA_TEXT, message);
    			if(!imagePath.equals("")) {
    				intent.putExtra(Intent.EXTRA_STREAM, Uri.fromFile(new File(imagePath)));
    			}
    			UnityPlayer.currentActivity.startActivity(Intent.createChooser(intent, "Share"));
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_SUCCESS);
    		} else {
    			UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_NOT_AVAILABLE);
    		}
    	} catch(Exception e) {
    		Log.e(TAG, "createChooser", e);
    		UnityPlayer.UnitySendMessage(UNITY_SEND_GAMEOBJECT, UNITY_SEND_CALLBACK, RESULT_ERROR);
    	}
    }
    
	/**
	 * 画像のIntentタイプを取得
	 * @param imagePath 画像パス(PNG/JPGのみ)
	 * @return Intentタイプ
	 */
	private String getIntentTypeForImage(String imagePath) {
		String extension = imagePath.substring(imagePath.lastIndexOf(".") + 1).toLowerCase(Locale.getDefault()) ;
		return (extension == ".png") ? "image/png" : "image/jpg";
	}
	
	/**
	 * 特定のアプリを起動させるためのIntentを生成
	 * @param name アプリパッケージ名。null or 空文字 で無視する。
	 * @param action Intentアクション
	 * @param type Intentタイプ
	 * @return Intent。アプリがない場合は null
	 */
	private Intent createAppIntent(String name, String action, String type) throws Exception {
		try {
			Intent intent = new Intent(action);
	        intent.setType(type);
	        
	        List<ResolveInfo> ris = UnityPlayer.currentActivity.getPackageManager().queryIntentActivities(intent, PackageManager.MATCH_DEFAULT_ONLY);
	        if(name == "" || name == null) {
	        	return (!ris.isEmpty()) ? intent : null;
	        } else {
	        	for (ResolveInfo ri : ris) {
		            if (ri.activityInfo.name.contains(name)) {
		            	intent.setClassName(ri.activityInfo.packageName, ri.activityInfo.name);
		                return intent;
		            }
		        }
	        }
	        return null;
	    } catch (Exception e) {
	    	throw e;
	    }
	}
}
