using System;
using System.Collections;
using System.Collections.Generic;
using OnlineGameTest.Bit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace OnlineGameTest {
    public class UIPanelHandler : MonoBehaviour {
        public static PlayerManager TrackingTarget { get; set; }
        
        // Set By Inspector
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Slider _healthBarRedBackground;
        [SerializeField] private TMP_Text _healthValueText;
        
        [SerializeField] private TMP_Text _currentWeaponMagLeftText;
        [SerializeField] private TMP_Text _currentWeaponTotalAmmoLeftText;

        [SerializeField] private TMP_Text _thisPlayerNameText;
        [SerializeField] private TMP_Text _thisPlayerScore;
        [SerializeField] private List<TMP_Text> _otherPlayerNameText;
        [SerializeField] private List<TMP_Text> _otherPlayerScore;
        
        public static bool PlayerReady { get; set; }
        
        private CharacterProperties CharacterProperties => TrackingTarget.CharacterProperties;
        private GunBitProperties GunBitProperties => TrackingTarget.GunBitProperties;
        private BitFire BitFire => TrackingTarget.BitFire;

        private Color _currentWeaponMagLeftTextNormalColor;
        private Color _currentWeaponTotalAmmoLeftTextNormalColor;

        private void Awake() {
            _currentWeaponMagLeftTextNormalColor = _currentWeaponMagLeftText.color;
            _currentWeaponTotalAmmoLeftTextNormalColor = _currentWeaponTotalAmmoLeftText.color;
        }
        
        private Queue<bool> HealbarRedBackgroundCounter = new Queue<bool>();

        private float _preHealthBarValue;
        private float _healthBarWaitTimeBeforeValueChange = 0.5f;
        private float _healthBarValueChangeInterpolateTime = 0.5f;
        private const float TOLERANCE = 0.0001f;


        private void UpdateBarPanel() {
            // Set Text
            _currentWeaponMagLeftText.text = GunBitProperties.GunBitBulletCurrentMagazine.ToString();
            _currentWeaponTotalAmmoLeftText.text = GunBitProperties.GunBitBulletCurrentStoreNum.ToString();

            // Set Color
            _currentWeaponMagLeftText.color = BitFire.MagAmmoAlert 
                ? Color.red 
                : _currentWeaponMagLeftTextNormalColor;
            _currentWeaponTotalAmmoLeftText.color = BitFire.StoreAmmoAlert 
                ? Color.red
                : _currentWeaponTotalAmmoLeftTextNormalColor;
            
            // Set Slider
            HealthChange(
                health: CharacterProperties.Health, 
                maxHealth: CharacterProperties.MaxHealth
            );
        }
        
        private void HealthChange(int health, int maxHealth) {
            _preHealthBarValue = _healthBar.value;
            float healthPercentage = (float) health / maxHealth;
            
            _healthBar.value = healthPercentage;

            StartCoroutine(
                routine:HealthBarRedBackground(
                    healthChange: Math.Abs(_healthBar.value - _preHealthBarValue) > TOLERANCE,
                    healthDecrease: _healthBar.value < _preHealthBarValue
                )
            );

            _healthValueText.text = health.ToString();
            
            _thisPlayerNameText.text = CharacterProperties.PlayerName;
            _thisPlayerScore.text = CharacterProperties.Health.ToString();
        }


        IEnumerator HealthBarRedBackground(bool healthChange, bool healthDecrease) {
            if (!healthChange) yield return null;
            // then, healthChange === true, we will check healthDecrease only
            else if (!healthDecrease) {
                _healthBarRedBackground.value = _healthBar.value;
                yield return null;
            }
            else {
                HealbarRedBackgroundCounter.Enqueue(true);
                
                yield return new WaitForSeconds(_healthBarWaitTimeBeforeValueChange);
                
                if (HealbarRedBackgroundCounter.Count >= 2) {
                    HealbarRedBackgroundCounter.Dequeue();
                    yield break;
                }
                
                float elapsedTime = 0;
                float startValue = _healthBarRedBackground.value;
                float endValue = _healthBar.value;

                while (elapsedTime < _healthBarValueChangeInterpolateTime) {
                    _healthBarRedBackground.value = Mathf.Lerp(
                        startValue,
                        endValue,
                        elapsedTime / _healthBarValueChangeInterpolateTime
                    );

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                HealbarRedBackgroundCounter.Dequeue();
                
                yield return null;
            }
        }

        
        // Update is called once per frame
        void Update() {
            if (PlayerReady) {
                UpdateBarPanel();
            }
        }
    }
}
