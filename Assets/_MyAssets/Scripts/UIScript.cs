using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] BudScript budScript;

    //[SerializeField] Image fill;

    private void Update()
    {
        slider.value = budScript.GetEnergy();
    }
}