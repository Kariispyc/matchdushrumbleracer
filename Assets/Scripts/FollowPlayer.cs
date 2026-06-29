using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public GameObject mobil;
    private Vector3 offset = new Vector3(0, 12, -15);
    void Update()
    {
        transform.position = mobil.transform.position + offset;
    }
}

