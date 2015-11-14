using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AISandbox
{

    public enum LinkType
    {
        OPEN,       
        HALFOPEN,
        CLOSED
    }

    public class ScoreBoard : MonoBehaviour
    {
        private static Dictionary<KeyValuePair<LinkType, int>, int> m_scoreBoard = new Dictionary<KeyValuePair<LinkType, int>, int>
        {
            {new KeyValuePair<LinkType, int>(LinkType.OPEN, 2), 10},
            {new KeyValuePair<LinkType, int>(LinkType.OPEN, 3), 100},
            {new KeyValuePair<LinkType, int>(LinkType.OPEN, 4), 100 + 2 * 100},
            {new KeyValuePair<LinkType, int>(LinkType.HALFOPEN, 2), 1},
            {new KeyValuePair<LinkType, int>(LinkType.HALFOPEN, 3), 20},
            {new KeyValuePair<LinkType, int>(LinkType.HALFOPEN, 4), 100 + 100},
            {new KeyValuePair<LinkType, int>(LinkType.HALFOPEN, 5), 200 + 100},
            {new KeyValuePair<LinkType, int>(LinkType.CLOSED, 4), 100},
            {new KeyValuePair<LinkType, int>(LinkType.CLOSED, 5), 200},
            {new KeyValuePair<LinkType, int>(LinkType.CLOSED, 6), 300}
        };

        private static void CollectionLineWithoutEndings(ref int[,] matrix, Vector2Int start, Vector2Int step, int length, ref Dictionary<KeyValuePair<LinkType, int>, int>[] dictionary)
        {
            int len = 1;
            int currentPlayer = matrix[start.x , start.y];
            int previousPlayer = currentPlayer;
            for (int j = 1; j <= length; j++)
            {
                Vector2Int temp = start + step * j;
                int nextPlayer = matrix[temp.x, temp.y];
                if (nextPlayer != currentPlayer)
                {
                    if (currentPlayer > 0 && len > 1)
                    {
                        LinkType linkType = GetLinkType(previousPlayer, nextPlayer, currentPlayer);
                        KeyValuePair<LinkType, int> keyValuePair = new KeyValuePair<LinkType, int>(linkType, len);
                        if (dictionary[currentPlayer - 1].ContainsKey(keyValuePair))
                        {
                            dictionary[currentPlayer - 1][keyValuePair] += 1;
                        }
                        else
                        {
                            dictionary[currentPlayer - 1][keyValuePair] = 1;
                        }
                    }
                    len = 1;
                    previousPlayer = currentPlayer;
                    currentPlayer = nextPlayer;
                }
                else
                {
                    len += 1;
                }
            }              
        }

        public static int[] CalculateTeamScore(ref int [,] matrix, out int[] strengths)
        {
            int dimensionWithWall = matrix.GetLength(0);
            int dimension = dimensionWithWall - 2;

            Dictionary<KeyValuePair<LinkType, int>, int>[] dictionary = new Dictionary<KeyValuePair<LinkType, int>, int>[Constants.NUMPLAYERS];

            for (int i = 0; i < dictionary.Length; i ++)
            {
                dictionary[i] = new Dictionary<KeyValuePair<LinkType, int>, int>();
            }

            for (int i = 1; i <= dimension; i++)
            {
                CollectionLineWithoutEndings(ref matrix, new Vector2Int(i, 0), new Vector2Int(0, 1), dimension + 1, ref dictionary);
            }

            for (int i = 1; i <= dimension; i++)
            {
                CollectionLineWithoutEndings(ref matrix, new Vector2Int(0, i), new Vector2Int(1, 0), dimension + 1, ref dictionary);
            }

            for (int i = 1; i <= dimension; i++)
            {
                CollectionLineWithoutEndings(ref matrix, new Vector2Int(0, dimension - i), new Vector2Int(1, 1), i + 1, ref dictionary);
            }

            for (int i = 1; i < dimension; i++)
            {
                CollectionLineWithoutEndings(ref matrix, new Vector2Int(dimension - i, 0), new Vector2Int(1, 1), i + 1, ref dictionary);
            }

            for (int i = 1; i <= dimension; i++)
            {
                CollectionLineWithoutEndings(ref matrix, new Vector2Int(i + 1, 0), new Vector2Int(-1, 1), i + 1, ref dictionary);
            }

            for (int i = 1; i < dimension; i++)
            {
                CollectionLineWithoutEndings(ref matrix, new Vector2Int(dimension + 1, dimension - i), new Vector2Int(-1, 1), i + 1, ref dictionary);
            }

            int[] scoreTeams = new int[Constants.NUMPLAYERS];
            strengths = new int[Constants.NUMPLAYERS];

            for (int i = 0; i < Constants.NUMPLAYERS; i ++)
            {
                foreach (KeyValuePair<KeyValuePair<LinkType, int>, int> entry in dictionary[i])
                {
                    if (m_scoreBoard.ContainsKey(entry.Key))
                    {
                        strengths[i] += entry.Value * m_scoreBoard[entry.Key];
                    }
                    scoreTeams[i] += entry.Value * GetPointsFromLength(entry.Key.Value);
                }
            }

            return scoreTeams;
        }

        public static int CalculateTeamScore(ref int[,] matrix, int currentPlayer)
        {
            int[] strengths;
            CalculateTeamScore(ref matrix, out strengths);
            int opponent = Constants.NUMPLAYERS - currentPlayer + 1;
            return strengths[currentPlayer - 1] - strengths[opponent - 1];
        }

        private static LinkType GetLinkType(int prev, int next, int current)
        {
            Debug.Assert(prev != current);
            Debug.Assert(next != current);

            if (prev != 0 && next != 0)
            {
                return LinkType.CLOSED;
            }

            if (prev != 0 || next != 0)
            {
                return LinkType.HALFOPEN;
            }
                      
            return LinkType.OPEN;                        
        }

        private static int GetPointsFromLength(int length)
        {
            return Math.Max(length - 3, 0);
        }
    }
}
