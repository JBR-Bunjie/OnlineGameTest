using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace OnlineGameTest {
    public class ResourcesHandler {
        private List<IResourceLocator> cataLogResult;
        
        
        // private void DeleteAllCache() {
        //     Addressables.ClearDependencyCacheAsync(true);
        // }

        #region Download Assets

        private IEnumerator AddressablesSystemInit() {
            // System Init
            var initializeAsync = Addressables.InitializeAsync(false);
            yield return initializeAsync;

            if (initializeAsync.Status == AsyncOperationStatus.Failed) {
                Debug.Log("Failed At Initializing Addressable" + initializeAsync.OperationException);
                Addressables.Release(initializeAsync);
                yield break;
            }

            Debug.Log("Successfully Initialize Addressable" + initializeAsync.OperationException);
            Addressables.Release(initializeAsync);
        }
        
        private IEnumerator DeleteAllCatalog() {
            // Addressables.ClearResourceLocators();

            string pathPrefix = Application.persistentDataPath + "/com.unity.addressables";
            string[] tbdFiles = Directory.GetFiles(pathPrefix, "catalog_*", SearchOption.TopDirectoryOnly);
            // string[] tbdFiles = Directory.GetFiles(pathPrefix, "catalog_*.json", SearchOption.TopDirectoryOnly);
            foreach (var file in tbdFiles) {
                File.Delete(file);
                yield return null;
            }
        }
        
        public IEnumerator CheckGameABResources(
            bool checkPrevCatalog = true,
            Action<object> progressCallback = null,
            Action<string, bool, bool> suffixCallback = null
        ) {
            AddressablesSystemInit();

            // // TODO: This will be a problem through, but we'll just through this resource check
            // // When we confirm there is a newer game path, there is no need to check preLog.
            // if (!checkPrevCatalog) {
            //     // Addressables.ClearResourceLocators();
            //     var deleteCatalogCoroutine = MonoSystem.Instance.StartCoroutine(DeleteAllCatalog());
            //     yield return deleteCatalogCoroutine;
            // }
            
            
            // Check Catalog Updates
            var catalogCheckAsync = Addressables.CheckForCatalogUpdates(false);
            yield return catalogCheckAsync;

            if (catalogCheckAsync.Status == AsyncOperationStatus.Failed) {
                // DeleteAllCatalog();
                suffixCallback?.Invoke("Failed At Catalog Checking", true, true);
                Addressables.Release(catalogCheckAsync);
                yield break;
            }

            if (catalogCheckAsync.Result.Count == 0) {
                // Normally, 'CheckGameResource' with 'usingPrevCatalog == true' will end here,
                // because we dont need to do any download in that case
                suffixCallback?.Invoke("No Updates Exist", false, true);
                Addressables.Release(catalogCheckAsync);
                yield break; 
            }

            
            // Updating Catalog
            var catalogUpdateAsync = Addressables.UpdateCatalogs(catalogCheckAsync.Result, false);
            yield return catalogUpdateAsync;

            if (catalogUpdateAsync.Status != AsyncOperationStatus.Succeeded) {
                suffixCallback?.Invoke("Failed At Catalog Updating", true, true);
                Addressables.Release(catalogCheckAsync);
                Addressables.Release(catalogUpdateAsync);
                yield break;
            }
            
            cataLogResult = catalogUpdateAsync.Result;
            Addressables.Release(catalogCheckAsync);
            Addressables.Release(catalogUpdateAsync);
            

            foreach (var result in cataLogResult) {
                var downloadSizeHandle = Addressables.GetDownloadSizeAsync(result.Keys);
                yield return downloadSizeHandle;

                Debug.Log((float)downloadSizeHandle.Result / 1024 / 1024 + " MB Data Need to be downloaded");

                if (downloadSizeHandle.Result == 0) {
                    // This case means that we lost local catalog only, we just need to re-download it.
                    progressCallback?.Invoke(1f);
                    suffixCallback?.Invoke("No Updates Exist", false, true);
                    Addressables.Release(downloadSizeHandle);
                    yield break;
                }

                suffixCallback?.Invoke("Exists New Content", false, false);
                Addressables.Release(downloadSizeHandle);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressCallback"></param>
        /// <param name="suffixCallback"></param>
        /// <returns></returns>
        public IEnumerator GetGameABResources(
            // bool checkPrevCatalog = true,
            Action<object> progressCallback = null,
            Action<string, bool> suffixCallback = null
        ) {
            foreach (var result in cataLogResult) {
                var downloadSizeHandle = Addressables.GetDownloadSizeAsync(result.Keys);
                yield return downloadSizeHandle;

                Debug.Log((float)downloadSizeHandle.Result / 1024 / 1024 + " MB Data Need to be downloaded");

                // Don't Update Game Version
                var downloadDependenciesHandle = Addressables.DownloadDependenciesAsync(result.Keys, Addressables.MergeMode.Union, false);
                while (downloadDependenciesHandle.Status == AsyncOperationStatus.None) {
                    progressCallback?.Invoke(downloadDependenciesHandle.PercentComplete);
                    yield return null;
                }

                progressCallback?.Invoke(downloadSizeHandle.PercentComplete);
                if (downloadDependenciesHandle.Status == AsyncOperationStatus.Succeeded) {
                    suffixCallback?.Invoke("Resource Ready", true);
                }
                else {
                    suffixCallback?.Invoke("Error Occurred At Resources Downloading: " + downloadDependenciesHandle.OperationException, false);
                }

                Addressables.Release(downloadDependenciesHandle);
            }
        }
        

        #endregion

        #region Load Assets


        public void LoadAsset(string resourcesPath, Action suffix = null) {
            MonoSystem.Instance.StartCoroutine(LoadAssetsIEnumerator(resourcesPath, suffix));
        }

        private IEnumerator LoadAssetsIEnumerator(string resourcesPath, Action suffix = null) {
            var prefab = Addressables.LoadAssetAsync<GameObject>(resourcesPath);
            yield return prefab;
            UnityEngine.Object.Instantiate(prefab.Result);
            suffix?.Invoke();
        }

        #endregion
    }
}
