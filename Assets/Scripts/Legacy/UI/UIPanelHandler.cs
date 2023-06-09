using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OnlineGameTest.LocalLogic.Bit;


namespace OnlineGameTest.LocalLogic {
    public class UIPanelHandler : MonoBehaviour {
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider healthBarRedBackground;
        [SerializeField] private TMP_Text healthValueText;
        
        [SerializeField] private TMP_Text _currentWeaponMagLeftText;
        [SerializeField] private TMP_Text _currentWeaponTotalAmmoLeftText;

        [SerializeField] private TMP_Text _thisPlayerNameText;
        [SerializeField] private TMP_Text _thisPlayerScore;
        [SerializeField] private List<TMP_Text> _otherPlayerNameText;
        [SerializeField] private List<TMP_Text> _otherPlayerScore;
        // [SerializeField] private Dictionary<int, GameObject> asdfljplayer;

        [SerializeField] private GameObject _player;
        private PlayerProperties _playerProperties => _player.GetComponent<PlayerProperties>();

        private Color _currentWeaponMagLeftTextNormalColor;
        private Color _currentWeaponTotalAmmoLeftTextNormalColor;

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
            _currentWeaponMagLeftText.color = GunBitFire.MagAmmoAlert ? Color.red : _currentWeaponMagLeftTextNormalColor;
            _currentWeaponTotalAmmoLeftText.color =
                GunBitFire.StoreAmmoAlert ? Color.red : _currentWeaponTotalAmmoLeftTextNormalColor;
            
            // Set Slider
            HealthChange(health: _playerProperties.Health, maxHealth: _playerProperties.MaxHealth);
        }
        
        private void HealthChange(int health, int maxHealth) {
            _preHealthBarValue = healthBar.value;
            float healthPercentage = (float) health / maxHealth;
            
            healthBar.value = healthPercentage;

            StartCoroutine(
                routine:HealthBarRedBackground(
                    healthChange: Math.Abs(healthBar.value - _preHealthBarValue) > TOLERANCE,
                    healthDecrease: healthBar.value < _preHealthBarValue
                )
            );

            healthValueText.text = health.ToString();
            
            _thisPlayerNameText.text = _playerProperties.PlayerName;
            _thisPlayerScore.text = _playerProperties.Health.ToString();
        }


        IEnumerator HealthBarRedBackground(bool healthChange, bool healthDecrease) {
            if (!healthChange) yield return null;
            // then, healthChange === true, we will check healthDecrease only
            else if (!healthDecrease) {
                healthBarRedBackground.value = healthBar.value;
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
                float startValue = healthBarRedBackground.value;
                float endValue = healthBar.value;

                while (elapsedTime < _healthBarValueChangeInterpolateTime) {
                    healthBarRedBackground.value = Mathf.Lerp(
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

        private void Start() {
            _currentWeaponMagLeftTextNormalColor = _currentWeaponMagLeftText.color;
            _currentWeaponTotalAmmoLeftTextNormalColor = _currentWeaponTotalAmmoLeftText.color;
        }
        
        // Update is called once per frame
        void Update() {
            UpdateBarPanel();
        }
    }
}
