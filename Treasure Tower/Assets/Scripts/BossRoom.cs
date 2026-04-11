using UnityEngine;
using UnityEngine.Tilemaps;

// designs each boss room
// uses our tiles and randomizes them for the playable area
// adds impassable 'border' tiles for decoration reasons
// randomly affects a certain number of valid playable tiles with status effects
public class BossRoom : MonoBehaviour
{
    // contains information about that specific tile
    // declared here to make it easier
    public class TileData
    {
        private bool Passable;
        private Status? tileStatus;

        // WIP
        // tile visual highlighting, updated similarly to status in BossRoom
        private bool isHighlighted;

        public void SetPassable(bool isPassable)
        {
            this.Passable = isPassable;
        }

        public bool IsPassable()
        {
            return this.Passable;
        }

        public void SetHighlighted(bool highlighted)
        {
            this.isHighlighted = highlighted;
        }

        public bool HasStatus()
        {
            if (this.tileStatus is null)
            {
                return false;
            }
            return true;
        }

        public void SetStatus(Status? newStatus)
        {
            this.tileStatus = newStatus;
        }

        // status needs some more specific functions, WIP
        // func for getting the tile sprite
        // func for reducing lifetime (if applicable)
        //
        // func for BossRoom to redraw tiles
        public Status GetStatus()
        {
            return this.tileStatus;
        }
    }

    // roomData = each tile's information
    // roomTilemap = each tile on the grid, which will have an assigned sprite
    private TileData[,] roomData;
    private Tilemap roomTilemap;

    // size of the board
    // static because all rooms have to fit the screen
    private int RoomHeight = 26;
    private int RoomWidth = 26;

    // impassable y coordinates
    // top is for the boss placement
    // bottom is for fitting the UI
    private int TopImpass = 2;
    private int BottomImpass = 22;

    // # of playable x coordinates by row (26 rows)
    [SerializeField]
    private int[] NumPlayableXTiles = new int[26];

    // status effect for this room
    [SerializeField]
    private int NumStatusTiles;

    // the sprites
    // these are added to in the editor!
    // wip, not set up yet.
    [SerializeField]
    private Tile[] PlayableTiles;

    [SerializeField]
    private Tile[] ImpassableTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // commented out until testing is ready
        // GenerateFixedX();
        // GenerateMap();
    }

    // Update is called once per frame
    void Update() { }

    // failsafe in case the X values are set incorrectly
    // feel free to ignore this
    void GenerateFixedX()
    {
        for (int y = 0; y < RoomHeight; ++y)
        {
            if (y <= TopImpass || y >= BottomImpass)
            {
                NumPlayableXTiles[y] = 0;
            }
            else if ((RoomWidth - NumPlayableXTiles[y]) % 2 != 0)
            {
                // this means impassable tiles can't be divided by 2
                // important later when we're sectioning those out
                // adding one to the value is an easy fix
                NumPlayableXTiles[y] += 1;
            }
        }
    }

    // creation of the tilemap
    void GenerateMap()
    {
        roomTilemap = GetComponentInChildren<Tilemap>();
        // the data to store per the x, y of that tile
        roomData = new TileData[RoomWidth, RoomHeight];

        for (int y = 0; y < RoomHeight; ++y)
        {
            // controls if the entire row should be impassable
            bool impassableRow = false;

            // maps where the impassable X values are
            // left is where they END for that side
            // right is where they START for that side
            int NumSideImpassable;
            int leftX;
            int rightX;

            // if the row is impassable, don't do calculations
            if (impassableRow)
            {
                leftX = RoomWidth;
                rightX = 0;
            }
            else
            {
                NumSideImpassable = (RoomWidth - NumPlayableXTiles[y]) / 2;
                leftX = NumSideImpassable;
                rightX = RoomWidth - NumSideImpassable;
            }

            // now loop through each tile in the row
            for (int x = 0; x < RoomWidth; ++x)
            {
                // set up a tile
                Tile tile;

                if (x <= leftX || x >= rightX)
                {
                    tile = ImpassableTiles[Random.Range(0, ImpassableTiles.Length)];
                    roomData[x, y].SetPassable(false);
                }
                else
                {
                    tile = PlayableTiles[Random.Range(0, PlayableTiles.Length)];
                    roomData[x, y].SetPassable(true);
                }
                // WIP - status will be set at random in a sec
                // init these under the playable tiles
                roomData[x, y].SetStatus(null);
                // tiles always start as not highlighted!
                roomData[x, y].SetHighlighted(false);

                roomTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
