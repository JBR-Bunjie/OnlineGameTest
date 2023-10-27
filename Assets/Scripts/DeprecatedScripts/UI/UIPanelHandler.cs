using System;
using System.Collections;
using System.Collections.Generic;
using OnlineGameTest.Bit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace OnlineGameTest {
    public class UIPanelHandler : MonoBehaviour {
        // public static PlayerManager MainTrackingTarget { get; set; } // this is the local player
        //
        // // Set By Inspector
        // // Right Panel UI GameObjects
        // [Header("Right Part")]
        // [SerializeField] private Slider _healthBar;
        // [SerializeField] private Slider _healthBarRedBackground;
        // [SerializeField] private TMP_Text _healthValueText;
        //
        // [SerializeField] private TMP_Text _currentWeaponMagLeftText;
        // [SerializeField] private TMP_Text _currentWeaponTotalAmmoLeftText;
        //
        // // Left Panel UI GameObjects
        // [Header("Left Part")]
        // [SerializeField] private GameObject _leftPanel; // get it, and use it as parent for creating gameobjects
        // [SerializeField] private GameObject _playerSelfPrefab;
        // [SerializeField] private GameObject _playerOtherPrefab;
        //
        // public static bool PlayerReady { get; set; }
        //
        // private CharacterProperties CharacterProperties => MainTrackingTarget.CharacterProperties;
        // private GunBitProperties GunBitProperties => MainTrackingTarget.GunBitProperties;
        // private BitFire BitFire => MainTrackingTarget.BitFire;
        //
        // private Color _currentWeaponMagLeftTextNormalColor;
        // private Color _currentWeaponTotalAmmoLeftTextNormalColor;
        //
        // private void Awake() {
        //     _currentWeaponMagLeftTextNormalColor = _currentWeaponMagLeftText.color;
        //     _currentWeaponTotalAmmoLeftTextNormalColor = _currentWeaponTotalAmmoLeftText.color;
        // }
        //
        // private Queue<bool> HealbarRedBackgroundCounter = new Queue<bool>();
        //
        // private float _preHealthBarValue;
        // private float _healthBarWaitTimeBeforeValueChange = 0.5f;
        // private float _healthBarValueChangeInterpolateTime = 0.5f;
        // private const float TOLERANCE = 0.0001f;
        //
        //
        // #region Right Panel
        //
        // private void UpdateRightPanelHealthBar() {
        //     // Set Text
        //     _currentWeaponMagLeftText.text = GunBitProperties.GunBitBulletCurrentMagazine.ToString();
        //     _currentWeaponTotalAmmoLeftText.text = GunBitProperties.GunBitBulletCurrentStoreNum.ToString();
        //
        //     // Set Color
        //     _currentWeaponMagLeftText.color = BitFire.MagAmmoAlert 
        //         ? Color.red 
        //         : _currentWeaponMagLeftTextNormalColor;
        //     _currentWeaponTotalAmmoLeftText.color = BitFire.StoreAmmoAlert 
        //         ? Color.red
        //         : _currentWeaponTotalAmmoLeftTextNormalColor;
        //     
        //     // Set Slider
        //     HealthChange(
        //         health: CharacterProperties.Health, 
        //         maxHealth: CharacterProperties.MaxHealth
        //     );
        // }
        //
        // private void HealthChange(int health, int maxHealth) {
        //     _preHealthBarValue = _healthBar.value;
        //     float healthPercentage = (float) health / maxHealth;
        //     
        //     _healthBar.value = healthPercentage;
        //
        //     StartCoroutine(
        //         routine:HealthBarRedBackground(
        //             healthChange: Math.Abs(_healthBar.value - _preHealthBarValue) > TOLERANCE,
        //             healthDecrease: _healthBar.value < _preHealthBarValue
        //         )
        //     );
        //
        //     _healthValueText.text = health.ToString();
        // }
        //
        //
        // IEnumerator HealthBarRedBackground(bool healthChange, bool healthDecrease) {
        //     if (!healthChange) yield return null;
        //     // then, healthChange === true, we will check healthDecrease only
        //     else if (!healthDecrease) {
        //         _healthBarRedBackground.value = _healthBar.value;
        //         yield return null;
        //     }
        //     else {
        //         HealbarRedBackgroundCounter.Enqueue(true);
        //         
        //         yield return new WaitForSeconds(_healthBarWaitTimeBeforeValueChange);
        //         
        //         if (HealbarRedBackgroundCounter.Count >= 2) {
        //             HealbarRedBackgroundCounter.Dequeue();
        //             yield break;
        //         }
        //         
        //         float elapsedTime = 0;
        //         float startValue = _healthBarRedBackground.value;
        //         float endValue = _healthBar.value;
        //
        //         while (elapsedTime < _healthBarValueChangeInterpolateTime) {
        //             _healthBarRedBackground.value = Mathf.Lerp(
        //                 startValue,
        //                 endValue,
        //                 elapsedTime / _healthBarValueChangeInterpolateTime
        //             );
        //
        //             elapsedTime += Time.deltaTime;
        //             yield return null;
        //         }
        //
        //         HealbarRedBackgroundCounter.Dequeue();
        //         
        //         yield return null;
        //     }
        // }
        //
        //
        // #endregion
        //
        // #region Left Panel
        //
        // private void UpdateLeftPanel() {
        //     foreach (var clientPlayerId in LocalGlobalValues.ClientPlayerIds) {
        //         PlayerManager currentInstance = LocalGlobalValues.PlayerLists[clientPlayerId];
        //
        //         LocalGlobalValues.PlayerScoreBoardDict.TryGetValue(clientPlayerId, out var targetScoreArea);
        //         
        //         if (targetScoreArea == null) {
        //             if (currentInstance.IsLocalPlayer) {
        //                 targetScoreArea = Instantiate(_playerSelfPrefab);
        //             }
        //             else {
        //                 targetScoreArea = Instantiate(_playerOtherPrefab);
        //             }
        //
        //             targetScoreArea.GetComponent<PlayerScoreApply>().targetPlayerId = clientPlayerId;
        //             targetScoreArea.GetComponent<PlayerScoreApply>().targetPlayerManager = currentInstance;
        //             targetScoreArea.transform.SetParent(_leftPanel.transform, false);
        //             LocalGlobalValues.PlayerScoreBoardDict[clientPlayerId] = targetScoreArea;
        //             ReAdjustTotalPanelPosition(targetScoreArea, currentInstance.IsLocalPlayer);
        //         }
        //         
        //         targetScoreArea.GetComponent<PlayerScoreApply>().Refresh();
        //         
        //     }
        // }
        //
        // private void ReAdjustTotalPanelPosition(GameObject go, bool localPlayer) {
        //     if (LocalGlobalValues.PlayerScoreBoardDict.Keys.Count <= 2) return;
        //     else {
        //         int index = 0;
        //         foreach (var clientPlayerId in LocalGlobalValues.ClientPlayerIds) {
        //             GameObject area = LocalGlobalValues.PlayerScoreBoardDict[clientPlayerId];
        //             SetNewPosition(go, localPlayer);
        //             if (!localPlayer) index++;
        //         }
        //     }
        // }
        //
        // private void SetNewPosition(GameObject go, bool localPlayer, int index = 0) {
        //     RectTransform rt = go.GetComponent<RectTransform>();
        //     if (localPlayer) {
        //         rt.position = new Vector3(rt.position.x, 200, rt.position.z);
        //     }
        //     else {
        //         rt.position = new Vector3(rt.position.x, 100 - 100 * index, rt.position.z);
        //     }
        // }
        //
        // #endregion
        //
        // // Update is called once per frame
        // void Update() {
        //     if (PlayerReady) {
        //         UpdateRightPanelHealthBar();
        //         UpdateLeftPanel();
        //     }
        // }
    }
}
