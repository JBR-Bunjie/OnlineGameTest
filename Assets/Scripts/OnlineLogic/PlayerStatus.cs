using UnityEngine;

namespace OnlineGameTest {
    public class PlayerStatus {
        
        // We Maintain these Built-in States, They are States Machine Related Variables
        // We will use this variable in `PlayerAnimatorController.cs`

        public class CharacterStates {
            /// <summary>
            /// Normal States, Like Bool, Float
            /// </summary>
            public bool Moving = false;
            public bool Running = false;

            /// <summary>
            /// For Triggers, we have two states, one is `WantXXX`, another is `IsXXX`
            /// Through this we can avoid the repeat trigger problem
            /// </summary>
            // Used For Pre state, process in `PlayerInputProcessing.cs`
            public bool WantToRoll = false;
            public bool WantToJump = false;
            public bool WantToAttack = false;
            
            // Runtime state, process in Animator, through `SetBoolBehaviour.cs` and `SetInsideVariables.cs`
            public string IsRollingString = "IsRolling";
            public string IsJumpingString = "IsJumping";
            public string IsAttackingString = "IsAttacking";
            // public bool IsRolling = false;
            // public bool IsJumping = false;
            // public bool IsAttacking = false;
        }
        
        public class GunBitStates {
            /// <summary>
            /// For Triggers, we have two states, one is `WantXXX`, another is `IsXXX`
            /// Through this we can avoid the repeat trigger problem
            /// </summary>
            // Used For Pre state, process in `PlayerInputProcessing.cs`
            public bool WantToAttack = false;
            public bool WantToReload = false;
        }
    }
}