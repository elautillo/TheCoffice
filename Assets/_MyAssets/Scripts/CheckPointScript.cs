using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    [SerializeField] int scoreBonus = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Bud")
        {
            BudScript budScript =
                other.gameObject.GetComponent<BudScript>();

            //budScript.FillEnergy();
            budScript.AddScore(scoreBonus);

            GameController.StoreAll(
                transform.position,
                budScript.GetScore());
            
            GameController.SetPlay(false);
        }
    }
}
