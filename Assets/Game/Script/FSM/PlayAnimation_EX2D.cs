using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Animation)]
    [Tooltip("Plays an ex2D Animation on a Game Object.")]
    public class PlayAnimation_EX2D : FsmStateAction
    {
        [RequiredField]
		public FsmOwnerDefault gameObject;
        private GameObject TargetGameObject;
        private exSprite exsprite;
        private exSpriteAnimation exanimation;
        [Tooltip("The animation name you want to play.")]
        public FsmString AnimationName;
        [Tooltip("If we have to trim the texture.")]
        public FsmBool trimTexture;
        [Tooltip("If we have to use the TextureOffset.")]
        public FsmBool useTextureOffset;
        [Tooltip("If we have to change the sprite color.")]
        public FsmColor color;
        [Tooltip("If we have to stop the animation on the exit.")]
        public bool stopOnExit;
        [Tooltip("If we have to hide the sprite on the exit.")]
        public bool hideOnExit;
        [ActionSection("Events")]
        [Tooltip("Send an event when finished.")]
        public FsmEvent finishEvent;


        public override void Reset()
        {
            TargetGameObject = null;
            AnimationName = null;
            trimTexture  = true;
            useTextureOffset = true;
            color = Color.white;
            finishEvent = null;
            stopOnExit = false;
            hideOnExit = false;
        }
        public override void OnEnter()
        {
            // Get the target of the animation
            TargetGameObject = Fsm.GetOwnerDefaultTarget(gameObject);

            // If there is no target
            if (TargetGameObject == null && AnimationName != null)
            {
                Finish();
                return;
            }
            exanimation = TargetGameObject.GetComponent<exSpriteAnimation>();
            exsprite = TargetGameObject.GetComponent<exSprite>();
            exsprite.trimTexture = trimTexture.Value;
            exsprite.useTextureOffset = useTextureOffset.Value;
            if (color.Value != Color.white)
            {
                exsprite.color = color.Value;
            }else
            {
                exsprite.color = exsprite.color;
            }
            exsprite.enabled = true;
            exanimation.Stop();
            exanimation.Play(AnimationName.Value);
        }
        public override void OnExit()
        {
            //If we have to hide the sprite on the exit.
            if (hideOnExit)
            {
                exsprite.enabled = false;
            }
            //If we have to stop the animation on the exit
            if (stopOnExit)
            {
                //Debug.Log("stop!");
                StopAnimation();
            }
        }
        //Stop the current animation
        protected void StopAnimation()
        {
            if (exanimation != null)
            {
                exanimation.Stop();
            }
        }
        protected void TerminateAction()
        {
            //Fire finish event
            Fsm.Event(finishEvent);

            Finish();
        }
        public override void OnUpdate()
        {
            //Terminate the action when the animation is stopped
            if (exanimation.IsPlaying() == false)
            {
                TerminateAction();
            }
        }
    }
}
