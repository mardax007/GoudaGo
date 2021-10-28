using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class API : MonoBehaviour
{

    const string BundleFolder = "https://mardax.tech/all_projects/GoudaGO/Unity/AssetBundles/";

    public void GetBundleObject(string assetName, UnityAction<AssetBundle> callback, Transform bundleParent) {
        StartCoroutine(GetDisplayBundleRoutine(assetName, callback, bundleParent));
    }

    IEnumerator GetDisplayBundleRoutine(string assetName, UnityAction<AssetBundle> callback, Transform bundleParent) {

        string bundleURL = BundleFolder + assetName + "-";

        //append platform to asset bundle name
#if UNITY_ANDROID
        bundleURL += "Android";
#else
        bundleURL += "IOS";
#endif
        
        //Debug.Log("Requesting bundle at " + bundleURL);

        //request asset bundle
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        yield return www.SendWebRequest();

        if (!(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.DataProcessingError || www.result == UnityWebRequest.Result.ProtocolError))
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle != null)
            {
                callback(bundle);
            }
            else
            {
                Debug.LogError("Not a valid asset bundle");
            }
        }
        else
        {
            Debug.LogError("Network error");
        }
    }
}
