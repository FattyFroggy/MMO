using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideController : MonoBehaviour
{
    public Transform mountPoint;
    public EntityController rider;
    private Vector3 offset;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        this.anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.mountPoint == null || this.rider == null) return;
        this.rider.SetRidePosition(this.mountPoint.position + this.mountPoint.TransformDirection(this.offset));
    }

    internal void SetRide(EntityController rider)
    {
        this.rider = rider;
    }

    internal void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch (entityEvent)
        {

            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;

        }
    }
}
