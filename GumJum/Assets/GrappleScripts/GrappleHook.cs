﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    GrappleGun grapple_gun;
    LineRenderer lr;
    Rigidbody player_body, rb;
    int layer_mask;
    bool can_fire, fired, broken, dropped;
    float pull_force = 10f, max_distance = 20f, current_distance, speed = 10f, 
        return_force = 20f, last_crank = -1, crank_cooldown = 0.5f, 
        block_distance = 2f, break_distance = 10f, pick_up_distance = 2f;
    FixedJoint joint;
    Stickable stuck_target;
    Collider hook_collider;

    // Start is called before the first frame update
    void Awake()
    {
        can_fire = true;
        player_body = GetComponentInParent<Player>().GetComponent<Rigidbody>();
        hook_collider = GetComponentInChildren<Collider>();
        Physics.IgnoreCollision(hook_collider, player_body.GetComponent<Collider>());
        layer_mask = ~LayerMask.GetMask("Player");
        grapple_gun = GetComponentInParent<GrappleGun>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponentInParent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.enabled = false;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dropped)
        {
            float distance = Vector3.Distance(transform.position, grapple_gun.start_point.position);
            if (distance < pick_up_distance)
                grapple_gun.PickUp();
            else if (distance > max_distance + break_distance)
                Break();
        }
        // if broken, return
        if (broken)
        return;

        lr.SetPositions(new Vector3[] {grapple_gun.start_point.position, transform.position});
        // if stuck in wall, then pull player
        if (stuck_target && !stuck_target.is_monster)
        {
            Vector3 dir = (player_body.transform.position - transform.position).normalized;
            player_body.AddForce(dir * pull_force * Time.deltaTime, ForceMode.Impulse);
        }
        // if fired, then check distance, drop if too far
        else if (fired)
        {
            print("Going");
            if (current_distance < max_distance)
                current_distance += Time.deltaTime;
            else
                Drop();
        }
    }

    public void Fire(Vector3 dir) 
    {
        // if broken, return
        if (broken)
            return;
        // if can_fire go, go, go
        if (can_fire)
        {
            hook_collider.enabled = true;
            transform.parent = null;
            fired = true;
            lr.enabled = true;
            rb.isKinematic = false;
            rb.velocity = dir * speed;
        }
    }

    public void Stick(Collider collider) 
    {
        // if object !has Stickable return
        stuck_target = collider.GetComponent<Stickable>();
        if (!stuck_target)
        {
            Drop();
            return;
        }
        // set that joint, boy
        rb.velocity = Vector3.zero;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = collider.GetComponent<Rigidbody>();
        // if monster, then stick em
        stuck_target.OnBreak += Drop;
        // change bools n stuff
        fired = false;
    }

    public void Drop() 
    {
        // change the joint
        if (joint)
            Destroy(joint);
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        //if monster, drop em
        ReleaseTarget();
        // change bools n stuff
        fired = false;
        dropped = true;
    }

    public void Break()
    {
        Drop();
        // change bools n stuff
        lr.enabled = false;
        rb.velocity = Vector3.zero;
        broken = true;
    }

    public void Crank() 
    {
        // if broken, return
        if (broken)
            return;
        // if stuck in monster, monster.pump, if monster explodes, drop
        if (stuck_target) {
            if (stuck_target.is_monster)
                print("PUMP IT UPPPP");
            else
                Drop();
        }
        // if dropped, then pull pump
        if (dropped)
            if (last_crank < 0 || (Time.time - last_crank > crank_cooldown))
            {
                Vector3 dir = (grapple_gun.start_point.position - transform.position).normalized;
                rb.AddForce(dir * return_force, ForceMode.Impulse);
                last_crank = Time.time;
            }
    }

    public void ReleaseTarget()
    {
        if (stuck_target)
        {
            stuck_target.OnBreak -= Drop;
            stuck_target = null;
        }
    }

    public void ResetState()
    {
        can_fire = true;
        fired = broken = dropped = false;
        ReleaseTarget();
        last_crank = -1;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        current_distance = 0f;
        hook_collider.enabled = false;
        transform.rotation = grapple_gun.start_point.rotation;
        transform.position = grapple_gun.start_point.position;
        transform.parent = grapple_gun.start_point;
    }

    public void CheckRope() 
    {
        // if broken, return
        if (broken)
            return;
        // if !can_fire, raycast from gun to hook
        // if hit non-player something and distance from hook is significant, break
        if (!can_fire)
        {
            Vector3 dir = (grapple_gun.start_point.position - transform.position);
            if (Physics.Raycast(grapple_gun.start_point.position, dir.normalized, 
                out RaycastHit hit, dir.magnitude, layer_mask)) 
                if (Vector3.Distance(hit.point, transform.position) > block_distance)
                    Break();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if broken, return
        if (broken)
            return;
        // if fired and wall, stick in wall
        // if monster, then monster.Stick()
        if (fired)
            Stick(collision.collider);
    }
}