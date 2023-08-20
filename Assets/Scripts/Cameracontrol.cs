using UnityEngine;

public class Cameracontrol : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x,player.position.y,transform.position.z);
        
    }
}
