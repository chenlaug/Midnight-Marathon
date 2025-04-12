using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrintEndMessage : MonoBehaviour
{
    [SerializeField] private int playerScore; // A modifier par le score général
    private bool isGetCaught = false;
    [SerializeField] private TextMeshProUGUI winMessage;
    [SerializeField] private TextMeshProUGUI defeatMessage;

    private void Start()
    {
        EndMessage();
    }

    private void EndMessage()
    {
        if (isGetCaught == true)
        {
            defeatMessage.text = "You got caught by your mom sneaking in late-night gaming... Game Over!";
        }
        else if (playerScore < 200)
        {
            defeatMessage.text = "You've scored enough points to call it a night... Game Over!";
        }
        else 
        {
            defeatMessage.text = "Congratulations! You've scored enough points to claim victory!";
        }
    }
}
