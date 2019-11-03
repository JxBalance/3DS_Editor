namespace VRTK.Examples
{
    using UnityEngine;

    public class M_buttonBack: VRTK_InteractableObject
    {
        

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            Debug.Log("hello!");
            base.StartUsing(usingObject);
            transform.GetComponent<backcabin>().OnRayPointInSphere();
           

        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            
        }

        protected void Start()
        {
           
        }

        protected override void Update()
        {
            base.Update();
            
        }
    }
}

