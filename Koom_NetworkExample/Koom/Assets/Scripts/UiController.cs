using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public static UiController instance;

    private GameObject ui;
    private GameObject healthBar;
    private GameObject armorBar;
    private Text ammoText;
    private GameObject hitmarker;

    // Use this for initialization
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        //ui = transform.Find("UI").gameObject;
        ui = gameObject;
        healthBar = ui.transform.Find("LifePanel").Find("HealthBar").gameObject;
        armorBar = ui.transform.Find("LifePanel").Find("ArmorBar").gameObject;
        ammoText = ui.transform.Find("Ammunition").Find("AmmoText").gameObject.GetComponent<Text>();
        hitmarker = ui.transform.Find("Scope").Find("Hitmarker").gameObject;
    }

    public void ReScaleBar(string barname, float amount)
    {
        switch (barname)
        {
            case "health":
                healthBar.GetComponent<RectTransform>().localScale = new Vector3(amount / 100, healthBar.GetComponent<RectTransform>().localScale.y, healthBar.GetComponent<RectTransform>().localScale.z);
                break;

            case "armor":
                armorBar.GetComponent<RectTransform>().localScale = new Vector3(amount / 100, armorBar.GetComponent<RectTransform>().localScale.y, armorBar.GetComponent<RectTransform>().localScale.z);
                break;
        }
    }

    public void SetAmmoText(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    public void Hitmarker()
    {
        hitmarker.SetActive(true);
        StartCoroutine(DisableHitmarker(0.1f));
    }

    IEnumerator DisableHitmarker(float time)
    {
        yield return new WaitForSeconds(time);
        hitmarker.SetActive(false);
    }
}
