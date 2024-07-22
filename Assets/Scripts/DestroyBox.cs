using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DestroyBox : MonoBehaviour
{
    [SerializeField] bool isLeftWeapon = true;
    [SerializeField] LevelController levelController;
    [Range(0, 1)]
    public float intensity;
    public float duration;
    ActionBasedController controller;

    private bool hitBoxUpPosition = false;
    private GameObject touchedBoxParent = null;
    private void OnTriggerEnter(Collider other)
    {
        if (isLeftWeapon && other.CompareTag("BlueUp") || !isLeftWeapon && other.CompareTag("PurpleUp"))
        {
            hitBoxUpPosition = true;
            touchedBoxParent = other.transform.parent.gameObject;
        }
        if (touchedBoxParent != other.transform.parent.gameObject)
        {
            //score--;
            hitBoxUpPosition = false;
        }
        if (isLeftWeapon && other.CompareTag("BlueDown") || !isLeftWeapon && other.CompareTag("PurpleDown"))
        {
            if (hitBoxUpPosition)
            {
            }
            else
            {
            }
            hitBoxUpPosition = false;
        }
        else
        {
            hitBoxUpPosition = false;
        }
    }
    void Start()
    {
        controller = GetComponentInParent<ActionBasedController>();
    }
}
