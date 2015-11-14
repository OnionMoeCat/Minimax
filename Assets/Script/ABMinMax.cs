using UnityEngine;
using System.Collections;

namespace AISandbox
{
    public class ABMinMax
    {
        private static int numNodes = 0;
        public static Vector2Int CalculateBestMove(ref int[,] matrix, int player, int depthLeft)
        {
            numNodes = 0;

            int alpha = int.MinValue;
            int beta = int.MaxValue;

            int dimensionWithWall = matrix.GetLength(0);
            int dimension = dimensionWithWall - 2;

            Vector2Int bestMove = new Vector2Int(-1, -1);
            for (int i = 1; i <= dimension; i ++)
            {
                for (int j = 1; j <= dimension; j ++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = player;

                        int opponent = Constants.NUMPLAYERS - player + 1;
                        int score = AlphaBetaMin(alpha, beta, ref matrix, opponent, player, depthLeft - 1);
                        if (score > alpha)
                        {
                            alpha = score;
                            bestMove.x = i - 1;
                            bestMove.y = j - 1;
                        }                        
                        matrix[i, j] = 0;
                    }
                }
            }

            Debug.Log(numNodes);

            return bestMove;
        }

        private static int AlphaBetaMax(int alpha, int beta, ref int[,] matrix, int player, int origPlayer, int depthLeft)
        {
            numNodes += 1;

            if (depthLeft == 0)
            {
                return ScoreBoard.CalculateTeamScore(ref matrix, origPlayer);
            }
            int dimensionWithWall = matrix.GetLength(0);
            int dimension = dimensionWithWall - 2;
            for (int i = 1; i <= dimension; i++)
            {
                for (int j = 1; j <= dimension; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = player;
                        int opponent = Constants.NUMPLAYERS - player + 1;
                        int score = AlphaBetaMin(alpha, beta, ref matrix, opponent, origPlayer, depthLeft - 1);
                        if (score >= beta)
                        {
                            matrix[i, j] = 0;
                            return beta;
                        }
                        if (score > alpha)
                        {
                            alpha = score;
                        }
                        matrix[i, j] = 0;
                    }
                }
            }
            return alpha;
        }

        private static int AlphaBetaMin(int alpha, int beta, ref int[,] matrix, int player, int origPlayer, int depthLeft)
        {
            numNodes += 1;

            if (depthLeft == 0)
            {
                return ScoreBoard.CalculateTeamScore(ref matrix, origPlayer);
            }
            int dimensionWithWall = matrix.GetLength(0);
            int dimension = dimensionWithWall - 2;
            for (int i = 1; i <= dimension; i++)
            {
                for (int j = 1; j <= dimension; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = player;
                        int opponent = Constants.NUMPLAYERS - player + 1;
                        int score = AlphaBetaMax(alpha, beta, ref matrix, opponent, origPlayer, depthLeft - 1);
                        if (score <= alpha)
                        {
                            matrix[i, j] = 0;
                            return alpha;
                        }
                        if (score < beta)
                        {
                            beta = score;
                        }
                        matrix[i, j] = 0;
                    }
                }
            }
            return beta;
        }
    }
}

