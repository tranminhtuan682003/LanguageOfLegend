using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject Kamehameha;
    public List<TextMeshProUGUI> timeCoolDowns;
    private List<int> seconds;
    private int ticks;
    private bool usedKamehameha;
    private bool usedUlti;

    void Start()
    {
        seconds = new List<int> { 0, 0, 0 };

        // Initialize cooldown text displays
        foreach (var textCooldown in timeCoolDowns)
        {
            textCooldown.text = "";
        }

        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        TimeCoolDown(ref usedKamehameha, 1, 7, Kamehameha);
        TimeCoolDown(ref usedUlti, 0, 10, null);
    }

    private void TimeCoolDown(ref bool useSkill, int index, int time, GameObject skillObject)
    {
        if (useSkill)
        {
            ticks++;

            if (ticks >= 60)
            {
                ticks = 0;
                seconds[index]++;
                timeCoolDowns[index].text = (time - seconds[index]).ToString();
            }

            if (seconds[index] >= time)
            {
                if (skillObject != null)
                {
                    skillObject.SetActive(false);
                }

                if (index == 0)
                {
                    playerController.transform.localScale = Vector3.one;
                    playerController.agent.speed = 10f;
                }

                seconds[index] = 0;
                useSkill = false;
                timeCoolDowns[index].text = "";
            }
        }
    }

    public void Skill2()
    {
        if (seconds[1] == 0 && !usedKamehameha)
        {
            playerController.GetComponentInChildren<Animator>().SetTrigger("Kamehameha");
            Kamehameha.SetActive(true);
            usedKamehameha = true;
            StartCoroutine(ManageSkillCollider());
        }
    }

    public void Ulti()
    {
        if (seconds[0] == 0 && !usedUlti)
        {
            playerController.damage = 45f;
            playerController.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            playerController.agent.speed = 20f;
            usedUlti = true;
        }
    }

    private IEnumerator ManageSkillCollider()
    {
        yield return new WaitForSeconds(2);

        Transform skillTransform = playerController.transform.Find("Skills");
        if (skillTransform != null)
        {
            Collider skillCollider = skillTransform.GetComponent<Collider>();
            if (skillCollider != null)
            {
                skillCollider.enabled = true;

                yield return new WaitForSeconds(1);
                skillCollider.enabled = false;
            }
        }
    }
}
