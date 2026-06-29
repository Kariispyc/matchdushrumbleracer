using UnityEngine;

public class KendaraanMaju : MonoBehaviour
{
    void Start()
    {

    }

    public float speed = 30.0f;
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
