using System;
using UnityEngine;

namespace TheTab.Runtime
{
    public class CreatTable : TheTable
    {
        #region Publics
        
        #endregion
        
        

        #region Unity Api

        private void Start()
        {
            InitialiserTerrain();
            ConvertListToGrid();
            CreateTerrain();
        }

        private void Update()
        {
            
        }

        #endregion
        
        

        #region Utils
        
        #endregion
        
        

        #region Main Methode

        public void ToggleSandEmpty(int x, int y, SpriteRenderer renderer)
        {
            if (terrainGrid[y, x] == TerrainType.Sand)
                terrainGrid[y, x] = TerrainType.Empty;
            else if (terrainGrid[y, x] == TerrainType.Empty)
                terrainGrid[y, x] = TerrainType.Sand;
            else
                return;

            UpdateCellSprite(renderer, terrainGrid[y, x]);
        }

        protected override void CreateCell(int y, int x)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell {y} {x}";
            cell.transform.position = new Vector3(x * 1.5f, y * 1.5f, 0);

            var spriteRenderer = cell.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                UpdateCellSprite(spriteRenderer, terrainGrid[y, x]);
            
            var clickable = cell.AddComponent<CellClick>();
            clickable.x = x;
            clickable.y = y;
            clickable.parentTable = this;
        }

        #endregion
        
        
        
        #region Privates
        
        #endregion
    }
}