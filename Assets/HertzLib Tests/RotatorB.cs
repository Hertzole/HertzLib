using UnityEngine;

public class RotatorB : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Rotator B enable");
    }

    private void OnDisable()
    {
        Debug.Log("Rotator B disable");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 90 * Time.deltaTime);
    }
}
