using System.Collections;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject lightningStrike;
    [SerializeField] private GameObject waterStrike;
    [SerializeField] private GameObject windStrike;
    [SerializeField] private GameObject fireStrike;
    private GameObject player;
    private Animator animator;
    private Animator plyrAnim;
    private gameManager gm;
    private Slashing s;
    private GeyserSeedSpawn gss;
    private LightningDash ld;
    private Breathing b;
    private GameObject mainCamera;
    private bool timeToAttack;
    private bool attacking;

    private void Awake() {
        timeToAttack = false;
        attacking = false;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        plyrAnim = player.GetComponent<Animator>();
        s = player.GetComponent<Slashing>();
        gss = player.GetComponent<GeyserSeedSpawn>();
        ld = player.GetComponent<LightningDash>();
        b = player.GetComponent<Breathing>();
        mainCamera = Camera.main.gameObject;
        StartCoroutine(EnterRoomAnimation());
    }
    void Update()
    {
        if(timeToAttack && !attacking) {
            StartCoroutine(AttackSequence());
            attacking = true;
        }
    }

    private IEnumerator AttackSequence() {
        animator.SetBool("blinking", true);
        //EDIT FOR EACH SKILL DEBUG HERE
        AttackStart(Random.Range(3, 4));
        yield return new WaitForSeconds(4f + Random.Range(0f, 1f));
        animator.SetBool("blinking", false);
        attacking = false;
    }
    private IEnumerator EnterRoomAnimation() {
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        plyrAnim.SetFloat("speed", 0);
        player.GetComponent<PlayerMovement>().enabled = false;
        setSkillEnabledIfAlreadyUnlocked(false);
        MainCameraPlaying mcp = mainCamera.GetComponent<MainCameraPlaying>();
        mcp.enabled = false;
        Vector3 oriPos = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 0, -5f);
        Vector3 bossPos = transform.position + new Vector3(0, 0, -5f);
        float elapsedTime = 0f;
        float duration = 0.5f;
        float t = 0f;
        while (elapsedTime <= duration)
        {
            t = elapsedTime / duration;
            mainCamera.transform.position = Vector3.Lerp(oriPos, bossPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = bossPos;
        animator.SetBool("blinking", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("blinking", false);
        elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            t = elapsedTime / duration;
            mainCamera.transform.position = Vector3.Lerp(bossPos, oriPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = oriPos;
        if(player != null) {
            player.GetComponent<PlayerMovement>().enabled = true;
        }
        setSkillEnabledIfAlreadyUnlocked(true);
        mcp.enabled = true;
        yield return new WaitForSeconds(2f);
        //bool to start attack set to true after playing animation
        timeToAttack = true;
    }
    
    private void AttackStart(int choice){
        switch (choice){
            case 0:
                LightningStrikeOnce();
            break;
            case 1:
                WindStrikeOnce();
            break;
            case 2:
                WaterStrikeOnce();
            break;
            case 3:
                FireStrikeOnce();
            break;
        }
    }
    //atack has to be under 4 seconds
    private void LightningStrikeOnce() {
        StartCoroutine(LightningStrike());
    }
    private IEnumerator LightningStrike() {
        yield return new WaitForSeconds(0.5f);
        GameObject l1 = Instantiate(lightningStrike, new Vector3(-6.5f,30.5f,0f), Quaternion.AngleAxis(90f, Vector3.forward), transform);
        l1.name = "lightningStrike";
        yield return new WaitForSeconds(0.5f);
        Destroy(l1);
        GameObject l2 = Instantiate(lightningStrike, new Vector3(0f,30.5f,0f), Quaternion.AngleAxis(90f, Vector3.forward), transform);
        l2.name = "lightningStrike";
        yield return new WaitForSeconds(0.5f);
        Destroy(l2);
        GameObject l3 = Instantiate(lightningStrike, new Vector3(5.5f,30.5f,0f), Quaternion.AngleAxis(90f, Vector3.forward), transform);
        l3.name = "lightningStrike";
        yield return new WaitForSeconds(0.5f);
        Destroy(l3);
        Debug.Log("Lightning Strike Finished");
    }
    private void WaterStrikeOnce() {
        StartCoroutine(WaterStrike());
    }
    private IEnumerator WaterStrike() {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Water Strike Finished");
    }
    private void WindStrikeOnce() {
        StartCoroutine(WindStrike());
    }
    private IEnumerator WindStrike() {
        yield return new WaitForSeconds(0.5f);
        GameObject ws = GameObject.Instantiate(windStrike, transform.position, Quaternion.identity, transform);
        ws.name = "windStrike";
        Debug.Log("Wind Strike Finished");
    }
    private void FireStrikeOnce() {
        StartCoroutine(FireStrike());
    }
    private IEnumerator FireStrike() {
        yield return new WaitForSeconds(0.5f);
        GameObject fs = GameObject.Instantiate(fireStrike, new Vector3(-6.66f,30.5f,0f), Quaternion.identity, transform);
        if(Random.Range(0,2) == 0) {
            fs.transform.position = fs.transform.position - new Vector3(0f, 3.5f, 0f);
        }
        fs.name = "fireStrike";
        Debug.Log("Fire Strike Finished");
    }
    private void setSkillEnabledIfAlreadyUnlocked(bool x)
    {
        s.SetCanSkill(x);
        gss.SetCanSkill(x);
        ld.SetCanSkill(x);
        b.SetCanSkill(x);
    }
}