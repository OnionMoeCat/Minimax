using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AISandbox
{
    public class PrintOutput : MonoBehaviour
    {
        public Text Score;
        public Text Strength;

        public void ChangeOutput(int[] playerScores, int[] playerStrength)
        {
            string scoreString = "Score:\n";
            for (int i = 0; i < playerScores.Length; i ++)
            {
                scoreString += "Player " + i + " : " + playerScores[i] + "\n";                           
            }

            string strengthString = "Player Strength:\n";
            for (int i = 0; i < playerStrength.Length; i++)
            {
                strengthString += "Player " + i + " : " + playerStrength[i] + "\n";
            }

            Score.text = scoreString;
            Strength.text = strengthString;
        }
    }
}

