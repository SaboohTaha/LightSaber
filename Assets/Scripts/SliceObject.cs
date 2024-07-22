using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
public class SliceObject : MonoBehaviour
{
    [SerializeField] bool isLeftWeapon = true;
    [SerializeField] LevelController levelController;
    [SerializeField] TutorialController tutorialController;
    [Range(0, 1)]
    public float intensity;
    public float duration;
    ActionBasedController controller;

    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public Material crossSectionMaterial;
    public LayerMask sliceableLayer;
    public float cutForce = 500;



    private bool hitBoxUpPosition = false;
    private GameObject touchedBoxParent = null;
    void Start()
    { 
        controller = GetComponentInParent<ActionBasedController>();
    }

    //private void OnTriggerEnter(Collision other)
    //{
    //    // Checking correct color hit
    //    if (isLeftWeapon && other.gameObject.tag == "LeftWeaponBox" || !isLeftWeapon && other.gameObject.tag == "RightWeaponBox")
    //    {
    //        bool correctSlice = false;
    //        if (other.GetContact(0).normal.y == 1)
    //        {
    //            Slice(other.gameObject);
    //            levelController.IncreaseScore();
    //            correctSlice = true;
    //        }
    //        if (!correctSlice)
    //        {
    //            Destroy(other.gameObject);
    //            controller.SendHapticImpulse(intensity, duration);
    //        }
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        // Hit up position Collider
        if (isLeftWeapon && other.CompareTag("BlueUp") || !isLeftWeapon && other.CompareTag("PurpleUp"))
        {
            hitBoxUpPosition = true;
            touchedBoxParent = other.transform.parent.gameObject;
            Destroy(other.gameObject);
            return;
        }
        // Checking for edge case if another box down collider collides after Up collider of different object
        if (touchedBoxParent != other.transform.parent.gameObject)
        {
            //score--;
            hitBoxUpPosition = false;
        }
        // Checking if Down is collided after Up
        if (isLeftWeapon && other.CompareTag("BlueDown") || !isLeftWeapon && other.CompareTag("PurpleDown"))
        {
            if (hitBoxUpPosition)
            {
                Slice(other.transform.parent.gameObject);
                if (levelController != null)
                    levelController.IncreaseScore();
                else if (tutorialController != null)
                    tutorialController.score++;
            }
            else
            {
                Destroy(other.gameObject);
                controller.SendHapticImpulse(intensity, duration);
            }
            Destroy(other.gameObject);
            hitBoxUpPosition = false;
        }
        else
        {
            controller.SendHapticImpulse(intensity, duration);
            Destroy(other.gameObject);
            hitBoxUpPosition = false;
        }
    }


    public void Slice(GameObject Target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 PlaneNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        PlaneNormal.Normalize();
        SlicedHull hull = Target.Slice(endSlicePoint.position, PlaneNormal);
        if (hull != null)
        {
            GameObject upperhull = hull.CreateUpperHull(Target, crossSectionMaterial);
            SetupSlicedComponent(upperhull);
            GameObject lowerhull = hull.CreateLowerHull(Target, crossSectionMaterial);
            SetupSlicedComponent(lowerhull);
            Destroy(Target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}
