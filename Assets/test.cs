using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Players
{
    public string name;
    public string url;
}

public class test : MonoBehaviour
{
    public List<Players> players = new List<Players>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadAsset());
    }
    IEnumerator DownloadAsset()
    {
        foreach(var i in players)
        {
            using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(i.url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                    var prefab = bundle.LoadAsset(i.name);
                    GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    obj.name = i.name;
                }
            }
        }
    }
}
