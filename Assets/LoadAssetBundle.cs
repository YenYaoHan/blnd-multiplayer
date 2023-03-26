using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetBundle : MonoBehaviour {
    
    GameObject temp;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        string path = @"C:\Users\EricYen\Desktop\player";
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(path);
        yield return bundleRequest;
        if (bundleRequest.assetBundle == null)
            yield break;

        temp = Instantiate(bundleRequest.assetBundle.LoadAllAssetsAsync<GameObject>().asset) as GameObject;
        temp.name = bundleRequest.assetBundle.name;
        bundleRequest.assetBundle.Unload(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
