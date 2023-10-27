using UnityEngine;


namespace OnlineGameTest.Animation.StatusMachine {
    public class SetBoolBehaviour : StateMachineBehaviour{
        public bool updateOnStateMachine, updateOnState;
        public bool valueOnEnter, valueOnExit;
        public string[] boolNames;

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (updateOnState)
                foreach (var boolName in boolNames) {
                    animator.SetBool(boolName, valueOnEnter);
                    Debug.Log(boolName + " " + valueOnEnter);
                }
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (updateOnState)
                foreach (var boolName in boolNames) {
                    animator.SetBool(boolName, valueOnExit);
                }
        }

        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
            if (updateOnStateMachine)
                foreach (var boolName in boolNames) {
                    animator.SetBool(boolName, valueOnEnter);
                }
        }

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
            if (updateOnStateMachine)
                foreach (var boolName in boolNames) {
                    animator.SetBool(boolName, valueOnExit);
                }
        }
    }
}