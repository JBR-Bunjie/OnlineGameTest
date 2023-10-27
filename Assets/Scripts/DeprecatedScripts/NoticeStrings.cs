using UnityEngine;

namespace OnlineGameTest {
    public static class NoticeStrings {
        // Game Title
        internal static readonly string GameTitle = "Sour Night";

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
    }
}