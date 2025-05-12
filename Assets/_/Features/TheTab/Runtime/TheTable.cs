using UnityEngine;

namespace TheTab.Runtime
{
    public class TheTable : MonoBehaviour
    {
        #region Publics

        

        #endregion


        #region Unity Api

        private void Start()
        {
            for (int i = 0; i < _tailleTab; i++)
            {
                
            }
        }

        private void Update()
        {
            
        }

        #endregion


        #region Utils

        

        #endregion


        #region Main Methode

        protected void CreateTable(GameObject objet,int tailleTab)
        {
            for (int i = 0; i < tailleTab; i++)
            {
                for (int j = 0; j < tailleTab; j++)
                {
                    Vector2 pos = new Vector2(j, i);
                    Instantiate(objet);
                    objet.transform.position = pos;
                    MapDisign(i,j, objet);
                }
            }
        }
        private void MapDisign(int i,int j, GameObject currentcell)
        {
            sbyte inter = m_ground[i,j];
            //sbyte inter = (sbyte)Random.Range(0,3);
            SpriteRenderer spriteRenderer = currentcell.GetComponent<SpriteRenderer>();
            switch (inter)
            {
                case 0: spriteRenderer.sprite = _spriteForSable; break;
                case 1: spriteRenderer.sprite = _spriteForGround; break;
                case 2: spriteRenderer.sprite = _spriteForWater; break;
            }
        }

        #endregion
        
        
        #region Privates
        
        
        private int _tailleTab;
        public sbyte[,] m_ground;
        [SerializeField] private Sprite _spriteForWater;
        [SerializeField] private Sprite _spriteForGround;
        [SerializeField] private Sprite _spriteForSable;
        [SerializeField] private sbyte[] _mapDisign;
        public enum Etat
        {
            bridge = 0,
            empty,
            crops,
            sand,
            villager,
            water
        }
        
        #endregion
    }
}
