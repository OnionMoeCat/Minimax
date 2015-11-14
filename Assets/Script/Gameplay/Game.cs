using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AISandbox {
    public enum PlayerType
    {
        HUMAN,
        AI
    }

    class AsyncCal
    {
        public async Task DoStuff(ref int[,] matrix, int currentPlayer, int depthLeft)
        {

        }
    }

    public class Game : MonoBehaviour {
        public int dimension = 6;

        private Grid m_grid;
        private PrintOutput m_printOutput;

        private int currentPlayer;

        private bool isMousePressed;

        public PlayerType[] players;

        private void Awake() {           	
            m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
            m_printOutput = GameObject.FindGameObjectWithTag("PrintOutput").GetComponent<PrintOutput>();
            currentPlayer = 1;
            isMousePressed = false;
            CreateGrid();
        }

        public void Launch()
        {
        }

        public void Reset()
        {
        }


        public void CreateGrid()
        {
            // Create and center the grid
            m_grid.Create(dimension);
            Vector2 gridSize = m_grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            m_grid.transform.position = gridPos;
        }

        void Update()
        {
            Debug.Assert(players.Length <= Constants.NUMPLAYERS);
            bool onMouseClicked = false;            
            if (Input.GetMouseButton(0))
            {
                if (!isMousePressed)
                {
                    onMouseClicked = true;
                    isMousePressed = true;
                }
            }
            else
            {
                if (isMousePressed)
                {
                    isMousePressed = false;
                }
            }

            if (players[currentPlayer - 1] == PlayerType.HUMAN)
            {
                if (onMouseClicked)
                {
                    GridNode node = m_grid.GetGridFromViewport(Input.mousePosition);
                    if (node != null)
                    {
                        if (node.Put(currentPlayer))
                        {
                            SwitchPlayer();
                        }
                    }
                }
            }
            else
            {
                int[,] matrix = m_grid.GetPlayerControlMatrix();
                int depthLeft = 5;
                AsyncCal asyncCal = new AsyncCal();
                asyncCal.DoStuff(ref matrix, currentPlayer, depthLeft);
                Vector2Int bestMove = ABMinMax.CalculateBestMove(ref matrix, currentPlayer, depthLeft);
                GridNode node = m_grid.GetNode(bestMove.x, bestMove.y);
                Debug.Assert(node != null);
                if (node.Put(currentPlayer))
                {
                    SwitchPlayer();
                }
            }
        }

        void SwitchPlayer()
        {
            currentPlayer += 1;
            if (currentPlayer > Constants.NUMPLAYERS)
            {
                currentPlayer = 1;
            }
            int[,] matrix = m_grid.GetPlayerControlMatrix();
            int[] strength;
            int[] scores = ScoreBoard.CalculateTeamScore(ref matrix, out strength);
            m_printOutput.ChangeOutput(scores, strength);
        }
    }


}