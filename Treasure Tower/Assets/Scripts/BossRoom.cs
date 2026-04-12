using System.Collections.Generic;
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

        // controls the tile attacks on the grid
        // lifetime dictates how long the tile attack will last
        private TileAttack? tileAttack;
        private int tileAttackLifetime = 0;

        // WIP
        // tile visual highlighting so we don't inflict damage on the player yet
        private bool highlighted;

        // WIP - may want to store original tile here, to make updating easier

        public int GetLifetime()
        {
            return this.tileAttackLifetime;
        }

        public void SetPassable(bool isPassable)
        {
            this.Passable = isPassable;
        }

        public bool IsPassable()
        {
            return this.Passable;
        }

        public bool IsHighlighted()
        {
            return this.highlighted;
        }

        public void SetHighlighted(bool highlighted)
        {
            this.highlighted = highlighted;
        }

        // checks if the tile has a perma status (eg: lava, impassable barrier, etc)
        public bool HasStatus()
        {
            if (this.tileStatus is null)
            {
                return false;
            }
            return true;
        }

        // sets the tile's status
        // only run on game start
        public void SetStatus(Status? newStatus)
        {
            this.tileStatus = newStatus;
        }

        // TILE ATTACK HANDLING
        // this sets the tile attack
        // non nullable
        public void SetTileAttack(TileAttack newTileAttack, int lifetime, bool highlighted)
        {
            if (tileAttackLifetime > 0)
            {
                return;
            }

            this.tileAttack = newTileAttack;
            this.tileAttackLifetime = lifetime;
            this.highlighted = highlighted;
        }

        // checks if there is a tile attack
        public bool HasTileAttack()
        {
            if (this.tileAttack is null)
            {
                return false;
            }
            return true;
        }

        public TileAttack GetTileAttack()
        {
            return this.tileAttack;
        }

        public void ReduceTileAttackLifetime()
        {
            this.tileAttackLifetime -= 1;
            if (this.tileAttackLifetime <= 0)
            {
                RemoveTileAttack();
            }
        }

        // removes the tile attack when it's no longer a factor or lifetime expires
        public void RemoveTileAttack()
        {
            this.tileAttack = null;
            this.tileAttackLifetime = 0;
            this.highlighted = false;
        }

        // status needs some more specific functions, WIP
        // func for changing the tile sprite
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

    // to control if the game should look for tile attacks on grid to do calculations
    // also allows the boss to make one if there isn't an active one yet
    private bool TileAttackOnGrid = false;

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
            if (NumPlayableXTiles[y] == 0)
            {
                impassableRow = true;
            }

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

    // handles all of the aspects of updating the room
    // does calculations for tile attack changes
    // changes the room sprites accordingly
    public void RefreshRoom(TileAttack? newTileAttack)
    {
        // collects all playable tile locations
        // they must both be passable and not affected by a tileattack
        List<Vector2Int> allowableLocations = new List<Vector2Int>();
        // and the locations to update
        List<Vector2Int> updateLocations = new List<Vector2Int>();

        int updatedLifetime = 0;

        bool isNewTileAttack = true;
        if (newTileAttack is null)
        {
            isNewTileAttack = false;
        }

        // if we're processing tile spread or not
        bool doesSpread = false;

        // grid doesn't need to be updated
        if (!this.TileAttackOnGrid && !isNewTileAttack)
        {
            return;
        }

        // start updating the grid by looping through/checking all the grid data
        for (int y = 0; y < RoomHeight; ++y)
        {
            // stop the loop early if it's an impassable row, nothing to change
            if (NumPlayableXTiles[y] == 0)
            {
                continue;
            }

            for (int x = 0; x < RoomWidth; ++x)
            {
                // so we can access the tile's information
                TileData tileData = roomData[x, y];

                // if it's an impassable tile, there is nothing to update
                if (!tileData.IsPassable())
                {
                    continue;
                }

                // swap the highlighted tiles to one that has an attack
                if (tileData.IsHighlighted())
                {
                    tileData.SetHighlighted(false);
                    updateLocations.Add(new Vector2Int(x, y));
                    continue;
                }

                // active tile attack updating
                if (tileData.HasTileAttack())
                {
                    updateLocations.Add(new Vector2Int(x, y));
                    // reduce the lifetime
                    // then, if the tile attack is over, stop this loop
                    // yes it's going to set the tileattackongrid multiple times idc
                    tileData.ReduceTileAttackLifetime();
                    if (!tileData.HasTileAttack())
                    {
                        this.TileAttackOnGrid = false;
                        continue;
                    }
                    // this tile will be free to add a new attack to in a moment
                    if (tileData.GetTileAttack().GetDoesSpread())
                    {
                        doesSpread = true;
                        updatedLifetime = tileData.GetLifetime();
                        allowableLocations.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    // this tile will be free to add an attack to
                    allowableLocations.Add(new Vector2Int(x, y));
                }
            }
        }

        // process attack spread
        // or new attacks on the grid
        if (doesSpread)
        {
            List<Vector2Int> newAffectedTiles = new List<Vector2Int>();

            // pass the updatable (spreadable) tiles to the tile attack handler
            // and the allowable tiles

            // loop through the newly affected tiles
            // set sprites and their TileAttack variables
            // lifetimes have already been handled previously
            //
            // if there are no tiles (all attacks spread off board or no possible tiles)
            // then skip this and fix vars
            if (newAffectedTiles.Count > 0)
            {
                foreach (Vector2Int location in newAffectedTiles)
                {
                    int x = location.x;
                    int y = location.y;

                    if (roomData[x, y] is null)
                    {
                        continue;
                    }

                    roomData[x, y].SetTileAttack(newTileAttack, updatedLifetime, false);
                    // WIP - sprite processing here
                    // note - make a method for this, it's called 3 times
                }
            }
            else
            {
                this.TileAttackOnGrid = false;
            }
        }
        else if (isNewTileAttack)
        {
            List<Vector2Int> newAffectedTiles = new List<Vector2Int>();
            this.TileAttackOnGrid = true;

            // pass the allowable tiles to the tile attack handler

            // loop through the affected tiles
            // set up the tileattack on them, as long as the tile exists
            foreach (Vector2Int location in newAffectedTiles)
            {
                int x = location.x;
                int y = location.y;

                if (roomData[x, y] is null)
                {
                    continue;
                }
                roomData[x, y]
                    .SetTileAttack(newTileAttack, newTileAttack.GetTileAttackDuration(), true);
                // WIP - sprite processing here
                // these would be highlighted tiles!
            }
        }

        // process updating the tiles to match their sprites
        // WIP - need to know how to do this first

        // loop through all updatable tiles
        if (updateLocations.Count > 0)
        {
            foreach (Vector2Int location in updateLocations)
            {
                int x = location.x;
                int y = location.y;

                if (roomData[x, y] is null)
                {
                    continue;
                }
                // WIP - sprite processing here
            }
        }
    }
}
