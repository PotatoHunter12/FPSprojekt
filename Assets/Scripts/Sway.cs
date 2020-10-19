using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float intensity;
    public float smooth;
    private Quaternion origin_rotation;

    // Start is called before the first frame update
    void Start()
    {
        origin_rotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSway();
    }

    void UpdateSway()
    {
        float t_mousex = Input.GetAxis("Mouse X");
        float t_mousey = Input.GetAxis("Mouse Y");

        Quaternion t_adjx = Quaternion.AngleAxis(intensity * t_mousex,Vector3.down);
        Quaternion t_adjy = Quaternion.AngleAxis(intensity * t_mousey, Vector3.right);
        Quaternion target_rotation = origin_rotation * t_adjx * t_adjy;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
    }
}
