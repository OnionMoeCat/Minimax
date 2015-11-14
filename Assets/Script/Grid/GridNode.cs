using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AISandbox {

    public class GridNode : MonoBehaviour
    {
        private int m_player;
        public int Player
        {
            get
            {
                return m_player;
            }
        }

        public Grid grid;
        public int column;
        public int row;

        public Sprite spriteEmpty;
        public Sprite[] spritePlayers;

        private bool isPut;

        [SerializeField]
        private SpriteRenderer sprite_renderer;        

        void Awake()
        {
            sprite_renderer.sprite = spriteEmpty;
            m_player = 0;
            isPut = false;
        }

        public bool Put(int i_player)
        {
            if (!isPut)
            {
                Debug.Assert(i_player <= Constants.NUMPLAYERS);
                m_player = i_player;
                sprite_renderer.sprite = spritePlayers[i_player - 1];
                isPut = true;
                return true;
            }
            return false;
        }
    }
}