using UnityEngine;

namespace OnlineGameTest.LocalLogic.Bit {
    public class GunBitFire : MonoBehaviour {
        public static bool StoreAmmoAlert = false;
        public static bool MagAmmoAlert = false;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gunBitPointer">the direction which the bullet should go</param>
        /// <param name="worldInitialPosition"></param>
        /// <param name="gunBitProperties"></param>
        public static void FireBullet(
            Vector3 gunBitPointer,
            Vector3 worldInitialPosition,
            GunBitProperties gunBitProperties
        ) {
            if (GunBitProperties.GunBitBulletCurrentMagazine <= 10) {
                AmmoAlert(mag: true, store: StoreAmmoAlert);
                if (GunBitProperties.GunBitBulletCurrentMagazine <= 0) {
                    GunBitReload();
                    return;
                }
            }
            else {
                AmmoAlert(mag: false, store: StoreAmmoAlert);
            }
            
            GameObject bullet = Instantiate(gunBitProperties.BulletPrefab);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Transform tf = rb.transform; // Transform tf = bullet.GetComponent<Transform>();


            tf.position = worldInitialPosition;
            tf.forward = gunBitPointer;
            rb.velocity = tf.forward * gunBitProperties.GunBitBulletSpeed;
            rb.mass = gunBitProperties.GunBitBulletMass;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // bullet.transform.rotation = GunBitProperties.BulletPrefab.transform.rotation;
            // bullet.GetComponent<Rigidbody>().AddForce(pointer * GunBitProperties.GunBitSpeed);

            Destroy(bullet, gunBitProperties.GunBitBulletExistingTime);
            GunBitProperties.GunBitBulletCurrentMagazine -= 1;
            
            if (GunBitProperties.GunBitBulletCurrentMagazine <= 0) {
                // if the Current Magazine has no ammo, then try to reload
                GunBitReload();
            }
        }

        public static void GunBitReload() {
            int reloadBulletNeededNum = GunBitProperties.GunBitBulletMagazineCapacity -
                                        GunBitProperties.GunBitBulletCurrentMagazine;

            // if (GunBitProperties.GunBitBulletCurrentStoreNum == 0) {
            //     AmmoAlert(store:true);
            // }
            // else 
            if (GunBitProperties.GunBitBulletCurrentStoreNum < reloadBulletNeededNum) {
                GunBitProperties.GunBitBulletCurrentMagazine += GunBitProperties.GunBitBulletCurrentStoreNum;
                GunBitProperties.GunBitBulletCurrentStoreNum = 0;
            }
            else {
                GunBitProperties.GunBitBulletCurrentMagazine = GunBitProperties.GunBitBulletMagazineCapacity;
                GunBitProperties.GunBitBulletCurrentStoreNum -= reloadBulletNeededNum;
            }
            
            // ReCheck
            if (GunBitProperties.GunBitBulletCurrentStoreNum <= GunBitProperties.GunBitBulletMagazineCapacity / 2) {
                AmmoAlert(mag: MagAmmoAlert, store: true);
            }
            else {
                AmmoAlert(mag: MagAmmoAlert, store: false);
            }
        }

        private static void AmmoAlert(bool mag = false, bool store = false) {
            MagAmmoAlert = mag;
            StoreAmmoAlert = store;
        }

        public static void PickUpAmmo() { }
    }
}