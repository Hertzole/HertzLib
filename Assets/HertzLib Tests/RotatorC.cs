using Hertzole.HertzLib;
using UnityEngine;

public class RotatorC : MonoBehaviour, IUpdate
{
    private void OnEnable()
    {
        UpdateManager.AddUpdate(this);
    }

    private void OnDisable()
    {
        UpdateManager.RemoveUpdate(this);
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        transform.Rotate(Vector3.up * 90 * Time.deltaTime);
    }
}
