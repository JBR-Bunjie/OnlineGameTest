using UnityEngine;

public class HotFixEntry {
    private static ResMgr _resMgr = new ResMgr();

    public static void Hello() {
        Debug.Log("Test");
        // _resMgr.LoadAssetsIEnumerator("")
    }

    public void InitBattleFieldScene() {
        
    }

    // var operationHandle = Addressables.LoadAssetAsync<TextAsset>("generatedData");
    // string tableDataPrefix = $"{Application.streamingAssetsPath}/HRProceduralData";
    // gameConfigTables = new cfg.Tables(file => JSON.Parse(File.ReadAllText($"{tableDataPrefix}/{file}.json")));
}