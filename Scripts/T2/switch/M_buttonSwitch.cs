namespace VRTK.Examples
{
    using UnityEngine;

    public class M_buttonSwitch: VRTK_InteractableObject
    {
        

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            Debug.Log("hello!");
            base.StartUsing(usingObject);
            transform.GetComponent<switchcameraT2>().OnRayPointInSphere();
    
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            
        }

        protected void Start()
        {
            isUsable = true;
            holdButtonToUse = true;
            useOnlyIfGrabbed = false;
            pointerActivatesUseAction = true;
        }

        protected override void Update()
        {
            base.Update();
            
        }
    }
}

