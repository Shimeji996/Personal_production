using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // �Ǐ]����v���C���[
    public Rigidbody player;

    void Start()
    {
        
    }

    void Update()
    {
        var playerPosition = player.transform.position;
        var position = transform.position;
        position.x = playerPosition.x;
        position.z = playerPosition.z - 5;
        transform.position = position;
    }
}
