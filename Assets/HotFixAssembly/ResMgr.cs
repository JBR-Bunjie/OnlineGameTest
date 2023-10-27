using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResMgr {
    #region Load Assets

    public IEnumerator LoadAssetsIEnumerator(string resourcesPath) {
        // var prefab = Addressables.LoadAssetAsync<GameObject>(resourcesPath);
        var prefab = Addressables.LoadAssetAsync<GameObject>(resourcesPath);
        yield return prefab;
        UnityEngine.Object.Instantiate(prefab.Result).SetActive(true);
        // suffix?.Invoke();
    }

    #endregion
}