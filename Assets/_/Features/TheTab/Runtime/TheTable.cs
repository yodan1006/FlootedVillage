using System;
using System.Collections.Generic;
using UnityEngine;

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

        #region Unity API

        private void Start()
        {
        }

        private void OnValidate()
        {
            int desiredLength = gridSize * gridSize;
            if (serializedStates == null || serializedStates.Length != desiredLength)
                serializedStates = new TerrainType[desiredLength];
        }

        #endregion

        #region Main Methods

        protected void InitialiserTerrain()
        {
            gridCells.Clear();
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int idx = y * gridSize + x;
                    var t = (serializedStates != null && idx < serializedStates.Length)
                        ? serializedStates[idx]
                        : TerrainType.Empty;
                    gridCells.Add(new GridCell(x, y, t));
                }
            }
        }

        protected void ConvertListToGrid()
        {
            terrainGrid = new TerrainType[gridSize, gridSize];
            for (int y = 0; y < gridSize; y++)
                for (int x = 0; x < gridSize; x++)
                    terrainGrid[y, x] = TerrainType.Empty;

            foreach (var cell in gridCells)
            {
                if (cell.x >= 0 && cell.x < gridSize && cell.y >= 0 && cell.y < gridSize)
                    terrainGrid[cell.y, cell.x] = cell.type;
            }
        }

        protected void CreateTerrain()
        {
            foreach (Transform child in transform)
                DestroyImmediate(child.gameObject);

            for (int y = 0; y < gridSize; y++)
                for (int x = 0; x < gridSize; x++)
                    CreateCell(y, x);
        }

        private void CreateCell(int y, int x)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell {y} {x}";
            cell.transform.position = new Vector3(x * 1.5f, y * 1.5f, 0);

            var spriteRenderer = cell.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                UpdateCellSprite(spriteRenderer, terrainGrid[y, x]);
        }

        private void UpdateCellSprite(SpriteRenderer spriteRenderer, TerrainType type)
        {
            switch (type)
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
                default:
                    spriteRenderer.sprite = null;
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
        [SerializeField] private TerrainType[] serializedStates;

        #endregion
    }
}