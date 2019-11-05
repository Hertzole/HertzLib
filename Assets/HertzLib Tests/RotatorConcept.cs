using Hertzole.HertzLib;
using UnityEngine;

public class RotatorConcept : MonoBehaviour
{
    private class RotatorConceptUpdater : IUpdate
    {
        private RotatorConcept parent;

        public RotatorConceptUpdater(RotatorConcept parent)
        {
            this.parent = parent;
        }

        public void OnUpdate()
        {
            parent.OnUpdate();
        }
    }

    private RotatorConceptUpdater updater;

    private void OnUpdate()
    {
        transform.Rotate(Vector3.up * 90 * Time.deltaTime);
    }

    private void OnEnable()
    {
        if (updater == null)
        {
            updater = new RotatorConceptUpdater(this);
        }

        UpdateManager.AddUpdate(updater);
    }

    private void OnDisable()
    {
        UpdateManager.RemoveUpdate(updater);
    }


}
