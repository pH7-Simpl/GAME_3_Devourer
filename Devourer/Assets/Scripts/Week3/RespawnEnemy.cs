using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEnemy : EnemySpawner
{
    GameObject player;
    private bool leftRoom;
    private bool spawnAgain;
    private float coolDown;
    private float maxCoolDown;
    private bool startCooldown;
    private void Awake()
    {
        leftRoom = false;
        player = GameObject.FindGameObjectWithTag("Player");
        spawnAgain = false;
        maxCoolDown = 30f;
        coolDown = maxCoolDown;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (leftRoom && spawnAgain)
            {
                SpawnEnemy();
                leftRoom = false;
                spawnAgain = false;
                if (coolDown <= 0)
                {
                    coolDown = maxCoolDown;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            leftRoom = true;
            startCooldown = true;
        }
    }
    private void FixedUpdate()
    {
        if (startCooldown)
        {
            if (coolDown >= 0f)
            {
                coolDown -= Time.deltaTime;
            }
            else
            {
                spawnAgain = true;
            }
        }
    }
}
