using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheTab.Runtime
{
    public class TheTable : MonoBehaviour
    {
        #region Publics
        
        public enum TerrainType
        {
            Bridge,
            Empty,
            Crops,
            Sand,
            Villager,
            Water
        }

        [Serializable]
        public class GridCell
        {
            public TerrainType type;
            public int x;
            public int y;
            
            public GridCell(int x, int y, TerrainType type = TerrainType.Empty)
            {
                this.x = x;
                this.y = y;
                this.type = type;
            }
        }
        #endregion
        
        

        #region Unity Api

        private void Start()
        {
           
        }

        #endregion
        

        #region Utils
        
        #endregion
        
        

        #region Main Methode
        
        private void InitialiserTerrain()
        {
            gridCells.Clear();
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    gridCells.Add(new GridCell(j, i,TerrainType.Empty));
                }
            }
        }
        
        protected void ConvertListToGrid()
        {
            terrainGrid = new TerrainType[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    terrainGrid[i, j] = TerrainType.Empty;
                }
            }

            foreach (GridCell cell in gridCells)
            {
                if (cell.x >= 0 && cell.x < gridSize && cell.y >= 0 && cell.y < gridSize)
                {
                    terrainGrid[cell.x, cell.y] = cell.type;
                }
            }
        }

        protected void CreateTerrain()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);;
            }

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    CreateCell(i, j);
                }
            }
        }

        private void CreateCell(int i, int i1)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell {i} {i1}";
            cell.transform.position = new Vector3(i1*1.5f, i*1.5f, 0);
            
            SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                UpdateCellSprite(spriteRenderer, terrainGrid[i, i1]);
            }
        }

        private void UpdateCellSprite(SpriteRenderer spriteRenderer, TerrainType valueTuple)
        {
            switch (valueTuple)
            {
                case TerrainType.Bridge:
                    spriteRenderer.sprite = bridgeSprite;
                    break;
                case TerrainType.Empty:
                    spriteRenderer.sprite = emptySprite;
                    break;
                case TerrainType.Crops:
                    spriteRenderer.sprite = cropsSprite;
                    break;
                case TerrainType.Sand:
                    spriteRenderer.sprite = sandSprite;
                    break;
                case TerrainType.Villager:
                    spriteRenderer.sprite = villagerSprite;
                    break;
                case TerrainType.Water:
                    spriteRenderer.sprite = waterSprite;
                    break;
            }
        }

        #endregion
        
        
        
        #region Privates
        
        [SerializeField] private int gridSize = 10;
        [SerializeField] private GameObject cellPrefab;
        
        [Header("terrain type sprite")]
        [SerializeField] private Sprite bridgeSprite;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite cropsSprite;
        [SerializeField] private Sprite sandSprite;
        [SerializeField] private Sprite villagerSprite;
        [SerializeField] private Sprite waterSprite;
        
        [Header("liste pour les definir la case")]
        [SerializeField] private List<GridCell> gridCells = new List<GridCell>();
        private TerrainType[,] terrainGrid;
        
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