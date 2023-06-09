using UnityEngine;

namespace OnlineGameTest {
    public static class GameSystemGlobalVariables {
        #region Scene Name

        // For Load Scene
        internal static readonly string WelcomeSceneName = "00WelcomeScene";
        internal static readonly string GameplaySceneName = "02GamePlayScene";
        internal static readonly string CharacterSelectSceneName = "01CharacterSelectScene";
        internal static readonly string NetworkErrorSceneName = "03NetworkErrorScene";
        internal static readonly string GameSettlementSceneName = "04GameSettlementScene";
        internal static readonly string LoadingScene1Name = "99LoadingScene1";
        internal static readonly string LoadingScene2Name = "98LoadingScene2";
        internal static readonly string LoadingScene3Name = "97LoadingScene3";

        // Game Title
        internal static readonly string GameTitle = "Sour Night";

        #endregion

        #region Welcome Page

        // Welcome Page Button Name
        internal static readonly string Host = "Host";
        internal static readonly string Server = "Server";
        internal static readonly string Client = "Client";
        internal static readonly string Quit = "Quit";

        // Welcome Page Options:
        internal static readonly string ServerAddressInput = "ServerAddressInput"; // Server IP
        internal static readonly string ServerPortInput = "ServerPortInput"; // Server Port
        internal static readonly string GamePasswordInput = "GamePasswordInput"; // Game Password
        
        // Welcome Page Client Confirm:
        internal static readonly string Connect = "Connect"; // Connect Button

        // Welcome Page Server Confirm:
        internal static readonly string StartServer = "StartServer"; // Start Server Button

        // Welcome Page Host Confirm:
        internal static readonly string StartHost = "StartHost"; // Start Host Button
        
        internal static readonly string BackToWelcomeMenu = "BackToWelcomeMenu";

        #endregion

        #region System Environment
        
        internal static float _screenWidth = Screen.width;
        internal static float _screenHeight = Screen.height;

        #endregion
    }
}