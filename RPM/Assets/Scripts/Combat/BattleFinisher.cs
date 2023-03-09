using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFinisher : MonoBehaviour
{
   [SerializeField] private Canvas winCanvas;
   [SerializeField] private Canvas loseCanvas;
    public void FinishBattle(bool isPlayerWon, Unit[] winUnits, Unit[] loserUnits)
    {
        if (isPlayerWon)
        {
            Debug.Log("����������� � �������");
            winCanvas.gameObject.SetActive(true);
           
        }
        else
        {
            Debug.Log("�������...");
            loseCanvas.gameObject.SetActive(true);
        }
        EXPAdder eXPAdder = new EXPAdder();
        eXPAdder.SetTotalEXP(winUnits, loserUnits);
    }
}
