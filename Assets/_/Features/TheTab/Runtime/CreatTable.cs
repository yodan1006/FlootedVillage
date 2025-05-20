using System;
using UnityEngine;

namespace TheTab.Runtime
{
    public class CreatTable : TheTable
    {
        #region Public
        
        #endregion
        
        #region UnityApi

        private void Start()
        {
            InitialiserTerrain();
            ConvertListToGrid();
            CreateTerrain();
        }

        private void Update()
        {
            SpreadWater();
        }
        
        #endregion
        
        #region Utils

        public void ToggleSandEmpty(int x, int y, SpriteRenderer spriteRenderer)
        {
            if (terrainGrid[y, x] == TerrainType.Sand)
                terrainGrid[y, x] = TerrainType.Empty;
            else if (terrainGrid[y, x] == TerrainType.Empty)
                terrainGrid[y, x] = TerrainType.Sand;
            else
                return;

            UpdateCellSprite(spriteRenderer, terrainGrid[y, x]);
        }
        
        #endregion
        
        #region MainMethods

        protected override void CreateCell(int y, int x)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell {y} {x}";
            cell.transform.position = new Vector3(x * 1.5f, y * 1.5f, 0);

            var spriteRenderer = cell.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                UpdateCellSprite(spriteRenderer, terrainGrid[y, x]);

            // Store the SpriteRenderer
            if (_spriteRenderersGrid == null)
                _spriteRenderersGrid = new SpriteRenderer[terrainGrid.GetLength(0), terrainGrid.GetLength(1)];
            _spriteRenderersGrid[y, x] = spriteRenderer;
            
            var clickable = cell.AddComponent<CellClick>();
            clickable.x = x;
            clickable.y = y;
            clickable.parentTable = this;
        }

        private void SpreadWaterOld()
        {
            var newMovements = new System.Collections.Generic.List<(int, int, int, int)>();

            for (int y = 0; y < terrainGrid.GetLength(0); y++)
            {
                for (int x = 0; x < terrainGrid.GetLength(1); x++)
                {
                    if (terrainGrid[y, x] == TerrainType.Water)
                    {
                        var dirs = new (int dy, int dx)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
                        foreach (var (dy, dx) in dirs)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && nx < terrainGrid.GetLength(1) && ny >= 0 && ny < terrainGrid.GetLength(0))
                            {
                                if (terrainGrid[ny, nx] == TerrainType.Empty)
                                {
                                    newMovements.Add((y, x, ny, nx));
                                    break; 
                                }
                            }
                        }
                    }
                }
            }
           
            foreach (var move in newMovements)
            {
                terrainGrid[move.Item1, move.Item2] = TerrainType.Empty;
                terrainGrid[move.Item3, move.Item4] = TerrainType.Water;

                UpdateCellSprite(_spriteRenderersGrid[move.Item1, move.Item2], TerrainType.Empty);
                UpdateCellSprite(_spriteRenderersGrid[move.Item3, move.Item4], TerrainType.Water);
            }
        }

        private void SpreadWater()
        {
            var waterPositions = new System.Collections.Generic.List<(int y, int x)>();
            var villagerPositions = new System.Collections.Generic.List<(int y, int x)>();
            var seedPositions = new System.Collections.Generic.List<(int y, int x)>();

            for (int y = 0; y < terrainGrid.GetLength(0); y++)
            {
                for (int x = 0; x < terrainGrid.GetLength(1); x++)
                {
                    if (terrainGrid[y, x] == TerrainType.Water)
                    {
                        var dirs = new (int dy, int dx)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
                        foreach (var (dy, dx) in dirs)
                        {
                            int ny = y + dy;
                            int nx = x + dx;

                            if (
                                ny >= 0 && ny < terrainGrid.GetLength(0) &&
                                nx >= 0 && nx < terrainGrid.GetLength(1)
                            )
                            {
                                if (terrainGrid[ny, nx] == TerrainType.Empty)
                                {
                                    waterPositions.Add((ny, nx));
                                }
                                else if (terrainGrid[ny, nx] == TerrainType.Villager)
                                {
                                    villagerPositions.Add((ny, nx));
                                }
                                else if (terrainGrid[ny, nx] == TerrainType.Seed)
                                {
                                    seedPositions.Add((ny, nx));
                                }
                                else if (terrainGrid[ny, nx] == TerrainType.VillagerDrown)
                                {
                                    int nny = ny + dy;
                                    int nnx = nx + dx;
                                    if (
                                        nny >= 0 && nny < terrainGrid.GetLength(0) &&
                                        nnx >= 0 && nnx < terrainGrid.GetLength(1)
                                    )
                                    {
                                        if (terrainGrid[nny, nnx] == TerrainType.Empty)
                                            waterPositions.Add((nny, nnx));
                                        else if (terrainGrid[nny, nnx] == TerrainType.Seed)
                                            seedPositions.Add((nny, nnx));
                                        else if (terrainGrid[nny, nnx] == TerrainType.Villager)
                                            villagerPositions.Add((nny, nnx));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var (vy, vx) in villagerPositions)
            {
                terrainGrid[vy, vx] = TerrainType.VillagerDrown;
                UpdateCellSprite(_spriteRenderersGrid[vy, vx], TerrainType.VillagerDrown);
            }

            foreach (var (sy, sx) in seedPositions)
            {
                terrainGrid[sy, sx] = TerrainType.Crops;
                UpdateCellSprite(_spriteRenderersGrid[sy, sx], TerrainType.Crops);
            }

            foreach (var (ny, nx) in waterPositions)
            {
                terrainGrid[ny, nx] = TerrainType.Water;
                UpdateCellSprite(_spriteRenderersGrid[ny, nx], TerrainType.Water);
            }
        }

        #endregion
        
        #region Private

        private SpriteRenderer[,] _spriteRenderersGrid;
        
        #endregion
    }
}