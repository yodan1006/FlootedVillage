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
            Empty,
            Bridge,
            Crops,
            Sand,
            Villager,
            VillagerDrown,
            Water,
            Seed
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
            int desiredLength = gridWidth * gridHeight;
            if (serializedStates == null || serializedStates.Length != desiredLength)
                serializedStates = new TerrainType[desiredLength];
        }

        #endregion

        #region Main Methods

        protected void InitialiserTerrain()
        {
            gridCells.Clear();
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    int idx = y * gridWidth + x;
                    var t = (serializedStates != null && idx < serializedStates.Length)
                        ? serializedStates[idx]
                        : TerrainType.Empty;
                    gridCells.Add(new GridCell(x, y, t));
                }
            }
        }

        protected void ConvertListToGrid()
        {
            terrainGrid = new TerrainType[gridHeight, gridWidth];
            for (int y = 0; y < gridHeight; y++)
                for (int x = 0; x < gridWidth; x++)
                    terrainGrid[y, x] = TerrainType.Empty;

            foreach (var cell in gridCells)
            {
                if (cell.x >= 0 && cell.x < gridWidth && cell.y >= 0 && cell.y < gridHeight)
                    terrainGrid[cell.y, cell.x] = cell.type;
            }
        }

        protected virtual void CreateTerrain()
        {
            foreach (Transform child in transform)
                DestroyImmediate(child.gameObject);

            for (int y = 0; y < gridHeight; y++)
                for (int x = 0; x < gridWidth; x++)
                    CreateCell(y, x);
        }

        protected virtual void CreateCell(int y, int x)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell {y} {x}";
            cell.transform.position = new Vector3(x * 1.5f, y * 1.5f, 0);

            var spriteRenderer = cell.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                UpdateCellSprite(spriteRenderer, terrainGrid[y, x]);
        }

        protected void UpdateCellSprite(SpriteRenderer spriteRenderer, TerrainType type)
        {
            switch (type)
            {
                case TerrainType.Bridge:
                    spriteRenderer.sprite = bridgeSprite;
                    break;
                case TerrainType.Empty:
                    spriteRenderer.sprite = emptySprite;
                    break;
                case TerrainType.Seed:
                    spriteRenderer.sprite = seedSprite;
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
                case TerrainType.VillagerDrown:
                    spriteRenderer.sprite = villagerDrownSprite;

                    break;
                default:
                    spriteRenderer.sprite = null;
                    break;
            }
        }

        #endregion

        #region Privates

        //[SerializeField] private int gridSize = 10;
        [SerializeField] private int gridWidth = 10;   // pour x
        [SerializeField] private int gridHeight = 10;  // pour y
        [SerializeField] protected GameObject cellPrefab;

        [Header("terrain type sprite")]
        [SerializeField] private Sprite bridgeSprite;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite seedSprite;
        [SerializeField] private Sprite cropsSprite;
        [SerializeField] private Sprite sandSprite;
        [SerializeField] private Sprite villagerSprite;
        [SerializeField] private Sprite villagerDrownSprite;
        [SerializeField] private Sprite waterSprite;

        [Header("liste pour les definir la case")]
        [SerializeField] private TerrainType[] serializedStates;
        private List<GridCell> gridCells = new List<GridCell>();
        protected TerrainType[,] terrainGrid;

        #endregion
    }
}