using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AISandbox {
    public class Grid : MonoBehaviour
    {
        public GridNode gridNodePrefab;

        private GridNode[ , ] _nodes;
        private float _node_width;
        private float _node_height;

        private int _dimension;

        public int Dimension
        {
            get
            {
                return _dimension;
            }
        }

        private GridNode CreateNode( int row, int col ) {
            GridNode node = Instantiate<GridNode>( gridNodePrefab );
            node.name = string.Format( "Node {0}{1}", (char)('A'+row), col );
            node.grid = this;
            node.row = row;
            node.column = col;
            node.transform.parent = transform;
            node.gameObject.SetActive( true );
            return node;
        }

        public void Create(int dimension) {

            _dimension = dimension;
            _node_width = gridNodePrefab.GetComponent<Renderer>().bounds.size.x;
            _node_height = gridNodePrefab.GetComponent<Renderer>().bounds.size.y;
            Vector2 node_position = new Vector2( _node_width * 0.5f, _node_height * -0.5f );
            _nodes = new GridNode[_dimension, _dimension];
            for( int row = 0; row < _dimension; ++row ) {
                for( int col = 0; col < _dimension; ++col ) {
                    GridNode node = CreateNode( row, col );
                    node.transform.localPosition = node_position;
                    _nodes[ row, col ] = node;

                    node_position.x += _node_width;
                }
                node_position.x = _node_width * 0.5f;
                node_position.y -= _node_height;
            }
        }

        public Vector2 size
        {
            get
            {
                return new Vector2(_node_width * _nodes.GetLength(1), _node_height * _nodes.GetLength(0));
            }
        }

        public GridNode GetNode( int row, int col ) {
            return _nodes[row, col];
        }

        public GridNode GetGridForPosition(Vector2 i_position)
        {
            // This trick makes a lot of assumptions that the nodes haven't been modified since initialization.
            Vector3 local_pos = transform.InverseTransformPoint(i_position);
            int column = Mathf.FloorToInt(local_pos.x / _node_width);
            int row = Mathf.FloorToInt(-local_pos.y / _node_height);
            if (row >= 0 && row < _nodes.GetLength(0)
                && column >= 0 && column < _nodes.GetLength(1))
            {
                return _nodes[row, column];
            }
            return null;
        }

        public Vector2 GetGridPositionFromViewport(Vector2 i_position)
        {
            Vector3 world_pos = Camera.main.ScreenToWorldPoint(i_position);
            Vector3 local_pos = transform.InverseTransformPoint(world_pos);
            return local_pos;
        }

        public GridNode GetGridFromViewport(Vector2 i_position)
        {
            Vector3 world_pos = Camera.main.ScreenToWorldPoint(i_position);
            return GetGridForPosition(world_pos);
        }

        public int[,] GetPlayerControlMatrix()
        {
            int[,] matrix = new int[_dimension + 2, _dimension + 2];
            for (int i = 1; i <= _dimension ; i ++)
            {
                for (int j = 1; j <= _dimension; j++)
                {
                    matrix[i, j] = _nodes[i - 1, j - 1].Player;
                }
            }

            for (int i = 0; i <= _dimension + 1; i++)
            {
                matrix[0, i] = -1;
            }

            for (int i = 0; i <= _dimension + 1; i++)
            {
                matrix[i, 0] = -1;
            }

            for (int i = 0; i <= _dimension + 1; i++)
            {
                matrix[_dimension + 1, i] = -1;
            }

            for (int i = 0; i <= _dimension + 1; i++)
            {
                matrix[i, _dimension + 1] = -1;
            }
            return matrix;
        }
    }
}