using Godot;
using System;
using System.Collections.Generic;

public class Level : Node2D
{

    #region Nodes

    private Sprite node_backgroundSprite;

    private Node2D node_tiles;
    private Node2D node_floorTiles;
    private Node2D node_itemTiles;
    private Node2D node_wallTiles;
    private Node2D node_enemyTiles;
    private PlayerTile node_playerTile;

    private Grid node_grid;
    private Camera2D node_camera;
    private Position2D node_position2D;

    #endregion // Nodes



    #region Signals

    [Signal] public delegate void PlayerHitEnemySignal (PlayerTile playerTile, EnemyTile enemyTile, int damage);
    [Signal] public delegate void PlayerKilledEnemySignal (PlayerTile playerTile, EnemyTile enemyTile);
    [Signal] public delegate void EnemyHitPlayerSignal (EnemyTile enemyTile, PlayerTile playerTile, int damage);
    [Signal] public delegate void EnemyKilledPlayerSignal (EnemyTile enemyTile, PlayerTile playerTile);
    [Signal] public delegate void PlayerWalkedIntoWallSignal (PlayerTile playerTile, WallTile wallTile);
    [Signal] public delegate void EnemyCameIntoViewSignal (EnemyTile enemyTile);
    [Signal] public delegate void EnemyWentOutOfViewSignal (EnemyTile enemyTile);

    #endregion // Signals



    #region Properties

    [Export] public int TileWidth { get; private set; } = 20;
    [Export] public int TileHeight { get; private set; } = 20;

    public int LevelWidth { get; private set; } = 16;
    public int LevelHeight { get; private set; } = 16;

    public int LevelWidthInPixels => TileWidth * LevelWidth;
    public int LevelHeightInPixels => TileHeight * LevelHeight;

    #endregion // Properties



    #region Fields

    private readonly Random m_rng;

    private FloorTile[,] m_floorTileMap;
    private Tile[,] m_itemTileMap;
    private WallTile[,] m_wallTileMap;
    private EnemyTile[,] m_enemyTileMap;
    private PlayerTile[,] m_playerTileMap;

    private readonly List<FloorTile> m_floorTiles;
    private readonly List<Tile> m_itemTiles;
    private readonly List<WallTile> m_wallTiles;
    private readonly List<EnemyTile> m_enemyTiles;
    private readonly List<Tile> m_allTiles;

    private readonly List<FloorTile> m_floorTilesInPlayerSightRange;
    private readonly List<Tile> m_itemTilesInPlayerSightRange;
    private readonly List<WallTile> m_wallTilesInPlayerSightRange;
    private readonly List<EnemyTile> m_enemyTilesInPlayerSightRange;
    private readonly List<Tile> m_allTilesInPlayerSightRange;

    private readonly List<FloorTile> m_visibleFloorTiles;
    private readonly List<Tile> m_visibleItemTiles;
    private readonly List<WallTile> m_visibleWallTiles;
    private readonly List<EnemyTile> m_visibleEnemyTiles;
    private readonly List<Tile> m_allVisibleTiles;

    private readonly List<Tile> m_seenTiles;
    private readonly List<Tile> m_unseenTiles;

    #endregion // Fields



    #region Constructors

    public Level ()
    {
        m_rng = new Random();

        m_floorTileMap = new FloorTile[LevelHeight, LevelWidth];
        m_itemTileMap = new Tile[LevelHeight, LevelWidth];
        m_wallTileMap = new WallTile[LevelHeight, LevelWidth];
        m_enemyTileMap = new EnemyTile[LevelHeight, LevelWidth];
        m_playerTileMap = new PlayerTile[LevelHeight, LevelWidth];

        m_floorTiles = new List<FloorTile>();
        m_itemTiles = new List<Tile>();
        m_wallTiles = new List<WallTile>();
        m_enemyTiles = new List<EnemyTile>();
        m_allTiles = new List<Tile>();

        m_floorTilesInPlayerSightRange = new List<FloorTile>();
        m_itemTilesInPlayerSightRange = new List<Tile>();
        m_wallTilesInPlayerSightRange = new List<WallTile>();
        m_enemyTilesInPlayerSightRange = new List<EnemyTile>();
        m_allTilesInPlayerSightRange = new List<Tile>();

        m_visibleFloorTiles = new List<FloorTile>();
        m_visibleItemTiles = new List<Tile>();
        m_visibleWallTiles = new List<WallTile>();
        m_visibleEnemyTiles = new List<EnemyTile>();
        m_allVisibleTiles = new List<Tile>();

        m_seenTiles = new List<Tile>();
        m_unseenTiles = new List<Tile>();
    }

    #endregion // Constructors



    #region Godot methods

    public override void _EnterTree ()
    {
        node_backgroundSprite = GetNode<Sprite>("Background");

        node_tiles = GetNode<Node2D>("Tiles");
        node_floorTiles = node_tiles.GetNode<Node2D>("FloorTiles");
        node_itemTiles = node_tiles.GetNode<Node2D>("ItemTiles");
        node_wallTiles = node_tiles.GetNode<Node2D>("WallTiles");
        node_enemyTiles = node_tiles.GetNode<Node2D>("EnemyTiles");
        node_playerTile = node_tiles.GetNode<PlayerTile>("PlayerTile");

        node_grid = GetNode<Grid>("Grid");
        node_camera = GetNode<Camera2D>("Camera2D");
        node_position2D = GetNode<Position2D>("Center");
    }

    public override void _Ready ()
    {
        foreach (Node node in node_floorTiles.GetChildren())
        {
            if (node is FloorTile tile)
            {
                m_floorTiles.Add(tile);
                m_unseenTiles.Add(tile);
                m_allTiles.Add(tile);
                tile.SetMainSpriteVisible(false);
            }
        }
        foreach (Node node in node_itemTiles.GetChildren())
        {
            if (node is Tile tile)
            {
                m_itemTiles.Add(tile);
                m_unseenTiles.Add(tile);
                m_allTiles.Add(tile);
                tile.SetMainSpriteVisible(false);
            }
        }
        foreach (Node node in node_wallTiles.GetChildren())
        {
            if (node is WallTile tile)
            {
                m_wallTiles.Add(tile);
                m_unseenTiles.Add(tile);
                m_allTiles.Add(tile);
                tile.SetMainSpriteVisible(false);
            }
        }
        foreach (Node node in node_enemyTiles.GetChildren())
        {
            if (node is EnemyTile tile)
            {
                m_enemyTiles.Add(tile);
                m_unseenTiles.Add(tile);
                m_allTiles.Add(tile);
                tile.SetMainSpriteVisible(false);
                int health = m_rng.Next(1, 5);
                tile.SetHealth(health, health);
            }
        }

        int playerX = Mathf.FloorToInt(node_playerTile.Position.x / TileWidth);
        int playerY = Mathf.FloorToInt(node_playerTile.Position.y / TileHeight);

        int furthestTilePositionRight = 0;
        int furthestTilePositionDown = 0;

        foreach (Tile tile in m_allTiles)
        {
            int tileX = Mathf.FloorToInt(tile.Position.x / TileWidth);
            int tileY = Mathf.FloorToInt(tile.Position.y / TileHeight);

            if (tileX > furthestTilePositionRight)
                furthestTilePositionRight = tileX;
            if (tileY > furthestTilePositionDown)
                furthestTilePositionDown = tileY;
        }

        if (playerX > furthestTilePositionRight)
            furthestTilePositionRight = playerX;
        if (playerY > furthestTilePositionDown)
            furthestTilePositionDown = playerY;

        LevelWidth = furthestTilePositionRight + 1;
        LevelHeight = furthestTilePositionDown + 1;

        m_floorTileMap = new FloorTile[LevelHeight, LevelWidth];
        m_itemTileMap = new Tile[LevelHeight, LevelWidth];
        m_wallTileMap = new WallTile[LevelHeight, LevelWidth];
        m_enemyTileMap = new EnemyTile[LevelHeight, LevelWidth];
        m_playerTileMap = new PlayerTile[LevelHeight, LevelWidth];

        for (int i = 0; i < m_floorTiles.Count; i++)
        {
            FloorTile tile = m_floorTiles[i];
            int tileX = Mathf.FloorToInt(tile.Position.x / TileWidth);
            int tileY = Mathf.FloorToInt(tile.Position.y / TileHeight);
            m_floorTileMap[tileY, tileX] = tile;
        }
        for (int i = 0; i < m_itemTiles.Count; i++)
        {
            Tile tile = m_itemTiles[i];
            int tileX = Mathf.FloorToInt(tile.Position.x / TileWidth);
            int tileY = Mathf.FloorToInt(tile.Position.y / TileHeight);
            m_itemTileMap[tileY, tileX] = tile;
        }
        for (int i = 0; i < m_wallTiles.Count; i++)
        {
            WallTile tile = m_wallTiles[i];
            int tileX = Mathf.FloorToInt(tile.Position.x / TileWidth);
            int tileY = Mathf.FloorToInt(tile.Position.y / TileHeight);
            m_wallTileMap[tileY, tileX] = tile;
        }
        for (int i = 0; i < m_enemyTiles.Count; i++)
        {
            EnemyTile tile = m_enemyTiles[i];
            int tileX = Mathf.FloorToInt(tile.Position.x / TileWidth);
            int tileY = Mathf.FloorToInt(tile.Position.y / TileHeight);
            m_enemyTileMap[tileY, tileX] = tile;
        }

        m_playerTileMap[playerY, playerX] = node_playerTile;

        node_grid.SetGridSize(LevelWidth, LevelHeight);
        node_grid.SetGridCellSize(TileWidth, TileHeight);
        node_grid.CreateGrid();

        node_backgroundSprite.Scale = new Vector2(LevelWidthInPixels, LevelHeightInPixels);

        node_position2D.Position = new Vector2(LevelWidthInPixels / 2f, LevelHeightInPixels / 2f);

        node_camera.LimitLeft = 0;
        node_camera.LimitRight = LevelWidthInPixels;
        node_camera.LimitTop = 0;
        node_camera.LimitBottom = LevelHeightInPixels;
        node_camera.Position = node_playerTile.GlobalPosition;

        RayCasting();
    }

    public override void _Input (InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey && inputEventKey.Pressed /* && !inputEventKey.IsEcho() */)
        {
            uint scancode = inputEventKey.Scancode;

            if (scancode == (int)KeyList.S)
            {
                foreach (Tile tile in m_allTiles)
                {
                    tile.SetMainSpriteVisible(true);
                    tile.SetForegroundSpriteVisible(false);
                    tile.SetForegroundSpriteColor(Colors.White);
                }
                m_seenTiles.Clear();
                m_seenTiles.AddRange(m_allTiles);
                m_unseenTiles.Clear();
                return;
            }
            else if (scancode == (int)KeyList.H)
            {
                foreach (Tile tile in m_allTiles)
                {
                    tile.SetMainSpriteVisible(false);
                    tile.SetForegroundSpriteVisible(false);
                    tile.SetForegroundSpriteColor(Colors.White);
                }
                m_seenTiles.Clear();
                m_unseenTiles.Clear();
                m_unseenTiles.AddRange(m_allTiles);
                m_visibleFloorTiles.Clear();
                m_visibleItemTiles.Clear();
                m_visibleWallTiles.Clear();
                m_visibleEnemyTiles.Clear();
                m_allVisibleTiles.Clear();
                return;
            }
            else if (scancode == (int)KeyList.Escape)
            {
                GetTree().Quit();
            }
            else if (scancode == (int)KeyList.R)
            {
                GetTree().ReloadCurrentScene();
            }
            else if (node_playerTile.Health > 0)
            {
                {
                    int dx = 0;
                    int dy = 0;

                    if (scancode == (int)KeyList.Kp4)
                        dx--;
                    else if (scancode == (int)KeyList.Kp6)
                        dx++;
                    else if (scancode == (int)KeyList.Kp8)
                        dy--;
                    else if (scancode == (int)KeyList.Kp2)
                        dy++;
                    else if (scancode == (int)KeyList.Kp7)
                    {
                        dx--;
                        dy--;
                    }
                    else if (scancode == (int)KeyList.Kp9)
                    {
                        dx++;
                        dy--;
                    }
                    else if (scancode == (int)KeyList.Kp1)
                    {
                        dx--;
                        dy++;
                    }
                    else if (scancode == (int)KeyList.Kp3)
                    {
                        dx++;
                        dy++;
                    }

                    if (dx != 0 || dy != 0)
                    {
                        int currentX = Mathf.FloorToInt(node_playerTile.Position.x / TileWidth);
                        int currentY = Mathf.FloorToInt(node_playerTile.Position.y / TileHeight);
                        int nextX = currentX + dx;
                        int nextY = currentY + dy;

                        if (nextX >= 0 && nextX < LevelWidth && nextY >= 0 && nextY < LevelHeight)
                        {
                            if (m_enemyTileMap[nextY, nextX] != null)
                            {
                                EnemyTile enemyTile = m_enemyTileMap[nextY, nextX];

                                enemyTile.DecreaseHealth(1);

                                EmitSignal(nameof(PlayerHitEnemySignal), node_playerTile, enemyTile, 1);

                                if (enemyTile.Health == 0)
                                {
                                    EmitSignal(nameof(PlayerKilledEnemySignal), node_playerTile, enemyTile);

                                    m_enemyTileMap[nextY, nextX] = null;
                                    m_enemyTiles.Remove(enemyTile);
                                    m_allTiles.Remove(enemyTile);
                                    m_enemyTilesInPlayerSightRange.Remove(enemyTile);
                                    m_allTilesInPlayerSightRange.Remove(enemyTile);
                                    m_visibleEnemyTiles.Remove(enemyTile);
                                    m_allVisibleTiles.Remove(enemyTile);
                                    m_seenTiles.Remove(enemyTile);
                                    m_unseenTiles.Remove(enemyTile);
                                    node_enemyTiles.RemoveChild(enemyTile);
                                    enemyTile.QueueFree();
                                }
                                else
                                {
                                    node_playerTile.DecreaseHealth(1);

                                    EmitSignal(nameof(EnemyHitPlayerSignal), enemyTile, node_playerTile, 1);

                                    if (node_playerTile.Health == 0)
                                    {
                                        EmitSignal(nameof(EnemyKilledPlayerSignal), enemyTile, node_playerTile);
                                        return;
                                    }
                                }
                            }
                            else if (m_floorTileMap[nextY, nextX] != null && m_wallTileMap[nextY, nextX] == null)
                            {
                                node_playerTile.Position = new Vector2(nextX * TileWidth, nextY * TileHeight);
                                m_playerTileMap[currentY, currentX] = null;
                                m_playerTileMap[nextY, nextX] = node_playerTile;
                                node_camera.Position = node_playerTile.Position;
                            }
                            else if (m_wallTileMap[nextY, nextX] != null)
                            {
                                WallTile wallTile = m_wallTileMap[nextY, nextX];
                                EmitSignal(nameof(PlayerWalkedIntoWallSignal), node_playerTile, wallTile);
                            }
                        }
                    }
                }

                for (int i = 0; i < m_enemyTiles.Count; i++)
                {
                    EnemyTile tile = m_enemyTiles[i];
                    int tileX = Mathf.FloorToInt(tile.Position.x / TileWidth);
                    int tileY = Mathf.FloorToInt(tile.Position.y / TileHeight);
                    int dx = m_rng.Next(-1, 2);
                    int dy = m_rng.Next(-1, 2);
                    int nextX = tileX + dx;
                    int nextY = tileY + dy;

                    if (nextX >= 0 && nextX < LevelWidth && nextY >= 0 && nextY < LevelHeight)
                    {
                        if (
                            m_floorTileMap[nextY, nextX] != null &&
                            m_wallTileMap[nextY, nextX] == null &&
                            m_enemyTileMap[nextY, nextX] == null &&
                            m_playerTileMap[nextY, nextX] == null)
                        {
                            tile.Position = new Vector2(nextX * TileWidth, nextY * TileHeight);
                            m_enemyTileMap[tileY, tileX] = null;
                            m_enemyTileMap[nextY, nextX] = tile;
                        }
                    }
                }

                RayCasting();
            }
        }
    }

    #endregion // Godot methods



    #region Private methods

    private void UpdateTilesInPlayerSightRange ()
    {
        m_floorTilesInPlayerSightRange.Clear();
        m_itemTilesInPlayerSightRange.Clear();
        m_wallTilesInPlayerSightRange.Clear();
        m_enemyTilesInPlayerSightRange.Clear();
        m_allTilesInPlayerSightRange.Clear();

        int playerX = Mathf.FloorToInt(node_playerTile.Position.x / TileWidth);
        int playerY = Mathf.FloorToInt(node_playerTile.Position.y / TileHeight);

        int xMin = playerX - node_playerTile.SightLength;
        int xMax = playerX + node_playerTile.SightLength;

        for (int y = 0; y < node_playerTile.SightLength; y++)
        {
            int nextPositiveY = playerY + y;
            int nextNegativeY = playerY - y;

            for (int x = xMin + y; x <= xMax - y; x++)
            {
                if (x >= 0 && x < LevelWidth)
                {
                    if (nextPositiveY >= 0 && nextPositiveY < LevelHeight)
                    {
                        if (m_floorTileMap[nextPositiveY, x] != null)
                        {
                            m_floorTilesInPlayerSightRange.Add(m_floorTileMap[nextPositiveY, x]);
                            m_allTilesInPlayerSightRange.Add(m_floorTileMap[nextPositiveY, x]);
                        }
                        if (m_itemTileMap[nextPositiveY, x] != null)
                        {
                            m_itemTilesInPlayerSightRange.Add(m_itemTileMap[nextPositiveY, x]);
                            m_allTilesInPlayerSightRange.Add(m_itemTileMap[nextPositiveY, x]);
                        }
                        if (m_wallTileMap[nextPositiveY, x] != null)
                        {
                            m_wallTilesInPlayerSightRange.Add(m_wallTileMap[nextPositiveY, x]);
                            m_allTilesInPlayerSightRange.Add(m_wallTileMap[nextPositiveY, x]);
                        }
                        if (m_enemyTileMap[nextPositiveY, x] != null)
                        {
                            m_enemyTilesInPlayerSightRange.Add(m_enemyTileMap[nextPositiveY, x]);
                            m_allTilesInPlayerSightRange.Add(m_enemyTileMap[nextPositiveY, x]);
                        }
                    }

                    if (nextNegativeY >= 0 && nextNegativeY < LevelHeight)
                    {
                        if (m_floorTileMap[nextNegativeY, x] != null)
                        {
                            m_floorTilesInPlayerSightRange.Add(m_floorTileMap[nextNegativeY, x]);
                            m_allTilesInPlayerSightRange.Add(m_floorTileMap[nextNegativeY, x]);
                        }
                        if (m_itemTileMap[nextNegativeY, x] != null)
                        {
                            m_itemTilesInPlayerSightRange.Add(m_itemTileMap[nextNegativeY, x]);
                            m_allTilesInPlayerSightRange.Add(m_itemTileMap[nextNegativeY, x]);
                        }
                        if (m_wallTileMap[nextNegativeY, x] != null)
                        {
                            m_wallTilesInPlayerSightRange.Add(m_wallTileMap[nextNegativeY, x]);
                            m_allTilesInPlayerSightRange.Add(m_wallTileMap[nextNegativeY, x]);
                        }
                        if (m_enemyTileMap[nextNegativeY, x] != null)
                        {
                            m_enemyTilesInPlayerSightRange.Add(m_enemyTileMap[nextNegativeY, x]);
                            m_allTilesInPlayerSightRange.Add(m_enemyTileMap[nextNegativeY, x]);
                        }
                    }
                }
            }
        }
    }

    private void RayCasting ()
    {
        List<Vector2> destinationPoints = new List<Vector2>();

        Vector2 origin = node_playerTile.GlobalCenter;

        for (int i = 0; i <= node_playerTile.SightLength; i++)
        {
            if (i == 0)
            {
                int xMargin = node_playerTile.SightLength * TileWidth;
                destinationPoints.Add(new Vector2(origin.x - xMargin, origin.y));
                destinationPoints.Add(new Vector2(origin.x + xMargin, origin.y));
            }
            else if (i < node_playerTile.SightLength)
            {
                int xMargin = (node_playerTile.SightLength - i) * TileWidth;
                int yMargin = i * TileHeight;
                destinationPoints.Add(new Vector2(origin.x - xMargin, origin.y - yMargin));
                destinationPoints.Add(new Vector2(origin.x - xMargin, origin.y + yMargin));
                destinationPoints.Add(new Vector2(origin.x + xMargin, origin.y - yMargin));
                destinationPoints.Add(new Vector2(origin.x + xMargin, origin.y + yMargin));
            }
            else
            {
                int yMargin = node_playerTile.SightLength * TileHeight;
                destinationPoints.Add(new Vector2(origin.x, origin.y - yMargin));
                destinationPoints.Add(new Vector2(origin.x, origin.y + yMargin));
            }
        }

        List<FloorTile> previousVisibleFloorTiles = new List<FloorTile>(m_visibleFloorTiles);
        List<Tile> previousVisibleItemTiles = new List<Tile>(m_visibleItemTiles);
        List<WallTile> previousVisibleWallTiles = new List<WallTile>(m_visibleWallTiles);
        List<EnemyTile> previousVisibleEnemyTiles = new List<EnemyTile>(m_visibleEnemyTiles);
        List<Tile> allPreviousVisibleTiles = new List<Tile>(m_allVisibleTiles);

        m_visibleFloorTiles.Clear();
        m_visibleItemTiles.Clear();
        m_visibleWallTiles.Clear();
        m_visibleEnemyTiles.Clear();
        m_allVisibleTiles.Clear();

        for (int i = 0; i < destinationPoints.Count; i++)
        {
            Vector2 destination = destinationPoints[i];
            float distance = origin.DistanceTo(destination) + 1;
            float step = distance / node_playerTile.SightLength;

            for (int j = 0; j <= node_playerTile.SightLength; j++)
            {
                Vector2 point = origin.MoveToward(destination, step * j);
                int tileX = Mathf.FloorToInt(point.x / TileWidth);
                int tileY = Mathf.FloorToInt(point.y / TileHeight);
                if (tileX >= 0 && tileX < LevelWidth && tileY >= 0 && tileY < LevelHeight)
                {
                    if (m_floorTileMap[tileY, tileX] != null)
                    {
                        FloorTile floorTile = m_floorTileMap[tileY, tileX];
                        if (!m_visibleFloorTiles.Contains(floorTile))
                            m_visibleFloorTiles.Add(floorTile);
                        if (!m_allVisibleTiles.Contains(floorTile))
                            m_allVisibleTiles.Add(floorTile);
                    }
                    if (m_itemTileMap[tileY, tileX] != null)
                    {
                        Tile itemTile = m_itemTileMap[tileY, tileX];
                        if (!m_visibleItemTiles.Contains(itemTile))
                            m_visibleItemTiles.Add(itemTile);
                        if (!m_allVisibleTiles.Contains(itemTile))
                            m_allVisibleTiles.Add(itemTile);
                    }
                    if (m_enemyTileMap[tileY, tileX] != null)
                    {
                        EnemyTile enemyTile = m_enemyTileMap[tileY, tileX];
                        if (!m_visibleEnemyTiles.Contains(enemyTile))
                            m_visibleEnemyTiles.Add(enemyTile);
                        if (!m_allVisibleTiles.Contains(enemyTile))
                            m_allVisibleTiles.Add(enemyTile);
                    }
                    if (m_wallTileMap[tileY, tileX] != null)
                    {
                        WallTile wallTile = m_wallTileMap[tileY, tileX];
                        if (!m_visibleWallTiles.Contains(wallTile))
                            m_visibleWallTiles.Add(wallTile);
                        if (!m_allVisibleTiles.Contains(wallTile))
                            m_allVisibleTiles.Add(wallTile);

                        break;
                    }
                }
            }
        }

        List<Tile> previousSeenTiles = new List<Tile>(m_seenTiles);
        List<Tile> previousUnseenTiles = new List<Tile>(m_unseenTiles);

        foreach (Tile tile in m_allVisibleTiles)
        {
            if (!allPreviousVisibleTiles.Contains(tile))
            {
                if (!previousSeenTiles.Contains(tile))
                {
                    m_seenTiles.Add(tile);
                    m_unseenTiles.Remove(tile);
                    tile.SetMainSpriteVisible(true);
                }
            }
        }

        foreach (Tile tile in allPreviousVisibleTiles)
        {
            if (!m_allVisibleTiles.Contains(tile))
            {

            }
        }

        foreach (FloorTile floorTile in m_visibleFloorTiles)
        {
            if (!previousVisibleFloorTiles.Contains(floorTile))
            {
                floorTile.SetForegroundSpriteVisible(false);
                floorTile.SetForegroundSpriteColor(Colors.White);
            }
        }

        foreach (FloorTile floorTile in previousVisibleFloorTiles)
        {
            if (!m_visibleFloorTiles.Contains(floorTile))
            {
                floorTile.SetForegroundSpriteVisible(true);
                floorTile.SetForegroundSpriteColor(0, 0, 0, 0.75f);
            }
        }

        foreach (Tile itemTile in m_visibleItemTiles)
        {
            if (!previousVisibleItemTiles.Contains(itemTile))
            {
                itemTile.SetForegroundSpriteVisible(false);
                itemTile.SetForegroundSpriteColor(Colors.White);
            }
        }

        foreach (Tile itemTile in previousVisibleItemTiles)
        {
            if (!m_visibleItemTiles.Contains(itemTile))
            {
                itemTile.SetForegroundSpriteVisible(true);
                itemTile.SetForegroundSpriteColor(0, 0, 0, 0.75f);
            }
        }

        foreach (WallTile wallTile in m_visibleWallTiles)
        {
            if (!previousVisibleWallTiles.Contains(wallTile))
            {
                wallTile.SetForegroundSpriteVisible(false);
                wallTile.SetForegroundSpriteColor(Colors.White);
            }
        }

        foreach (WallTile wallTile in previousVisibleWallTiles)
        {
            if (!m_visibleWallTiles.Contains(wallTile))
            {
                wallTile.SetForegroundSpriteVisible(true);
                wallTile.SetForegroundSpriteColor(0, 0, 0, 0.75f);
            }
        }

        foreach (EnemyTile enemyTile in m_visibleEnemyTiles)
        {
            if (!previousVisibleEnemyTiles.Contains(enemyTile))
            {
                enemyTile.SetMainSpriteVisible(true);
                EmitSignal(nameof(EnemyCameIntoViewSignal), enemyTile);
            }
        }

        foreach (EnemyTile enemyTile in previousVisibleEnemyTiles)
        {
            if (!m_visibleEnemyTiles.Contains(enemyTile))
            {
                enemyTile.SetMainSpriteVisible(false);
                EmitSignal(nameof(EnemyWentOutOfViewSignal), enemyTile);
            }
        }
    }

    #endregion // Private methods

}
