using System.Collections;
using System.Collections.Generic;
using OnlineGameTest;
using TMPro;
using UnityEngine;

public class PlayerInfoHandlerHandler : MonoBehaviour {
    // Prepare
    private string PlayerId => SearchLocalInstance.GetPlayerID(gameObject);
    private PlayerManager LocalInstance => SearchLocalInstance.GetPlayerManager(PlayerId);

    
    // Set By Script
    Camera camera;
    
    // Reference
    private TMP_Text PlayerName => LocalInstance.PlayerNameText;
    private CharacterProperties CharacterProperties => LocalInstance.CharacterProperties;
    

    void Start() {
        camera = Camera.main;

        // _distance = Vector3.Distance(camera.transform.position, transform.position);

        PlayerName.text = CharacterProperties.PlayerName;
    }

    void Update() {
        //跟随镜头旋转，直接把主镜头的旋转值赋值给公告牌即可
        transform.rotation = camera.transform.rotation;
        // transform.eulerAngles += new Vector3(0, 180, 0);

        //跟随镜头缩放（缩放镜头设置的镜头的位置），根据公告牌到主镜头的距离来做等距离缩放即可
        // float distance = Vector3.Distance(camera.transform.position, transform.position); //不断变化的距离
        // var scale = distance / _distance * 0.1F;
        // transform.localScale = new Vector3(scale, scale, scale);
    }
}