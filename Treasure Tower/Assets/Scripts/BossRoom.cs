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

        // not an object, the room holds its own script
        private bool tileStatus = false;

        // controls the tile attacks on the grid
        // lifetime dictates how long the tile attack will last on this specific tile
        private TileAttack? tileAttack;
        private int tileAttackLifetime = 0;

        // WIP
        // tile visual highlighting so we don't inflict damage on the player yet
        private bool highlighted;

        // original tile sprite, wow
        private Tile groundTile;

        public Tile GetDefaultTile()
        {
            return groundTile;
        }

        public void SetDefaultTile(Tile newTile)
        {
            groundTile = newTile;
        }

        public int GetLifetime()
        {
            return this.tileAttackLifetime;
        }

        public int GetCopyLifetime()
        {
            return this.tileAttack.GetLifetime();
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
            return tileStatus;
        }

        // sets the tile's status
        // only run on game start
        public void SetStatus(bool newStatus)
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

        public bool isRowOrWave()
        {
            string shape = tileAttack.GetShape();
            if (
                shape == "row"
                || shape == "double row"
                || shape == "wave"
                || shape == "double wave"
            )
            {
                return true;
            }
            return false;
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
    }

    // roomData = each tile's information
    // roomTilemap = each tile on the grid, which will have an assigned sprite
    private TileData[,] roomData;
    private Tilemap roomTilemap;

    // size of the board
    // static because all rooms have to fit the screen
    private int RoomHeight = 26;
    private int RoomWidth = 27;

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

    // holds the tile sprite for the perma status tiles
    [SerializeField]
    private Tile statusTile;

    // the tile attack's total lifetime
    // allows boss to make a tile attack next turn if it's 0
    private int totalTileAttackLifetime = 0;

    // the sprites
    // these are added to in the editor!
    // wip, not set up yet.
    [SerializeField]
    private Tile[] PlayableTiles;

    [SerializeField]
    private Tile[] ImpassableTiles;

    [SerializeField]
    private Tile bossPlatformTile;

    // holds all tiles the player can walk on
    private List<Vector2Int> WalkableTiles = new List<Vector2Int>();

    // and all tiles that tile attacks can currently affect. updated through the game
    private List<Vector2Int> usableTiles = new List<Vector2Int>();

    // where the boss is standing
    // MUST BE INITIALIZED IN THE EDITOR OR IT'LL BREAK
    [SerializeField]
    private List<Vector2Int> BossTiles = new List<Vector2Int>();

    [SerializeField]
    public Vector2Int playerLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateFixedX();
        GenerateMap();
    }

    // Update is called once per frame
    void Update() { }

    // methods called by other scripts that aren't related to updating the grid

    // player's location
    public Vector2Int GetPlayerLocation()
    {
        return playerLocation;
    }

    public void UpdatePlayerLocation(int x, int y)
    {
        playerLocation = new Vector2Int(x, y);
    }

    // movement needs a list of acceptable locations
    public List<Vector2Int> GetWalkableTiles()
    {
        return WalkableTiles;
    }

    // tile attack stuff
    public bool TileAttackOnGrid()
    {
        if (totalTileAttackLifetime <= 0)
        {
            return false;
        }
        return true;
    }

    // tile attack damage handling
    // returns an int to be handled by the reducing health method on character
    public int GetDamage(Vector2Int location)
    {
        int x = location.x;
        int y = location.y;
        int damage = 0;
        TileData tileData = roomData[x, y];

        if (tileData.HasTileAttack())
        {
            damage += tileData.GetTileAttack().GetDamage();
        }

        if (tileData.HasStatus())
        {
            // have to retrieve the script from the object first
            Status status = GetComponent<Status>();
            damage += status.GetDamage();
        }

        return damage;
    }

    public bool TileHasStatus(Vector2Int location)
    {
        int x = location.x;
        int y = location.y;

        if (roomData[x, y].HasStatus())
        {
            return true;
        }
        return false;
    }

    public Status GetRoomStatus()
    {
        return GetComponent<Status>();
    }

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

            // debugging ignore
            Debug.Log(
                $"Row {y}: leftX={leftX}, rightX={rightX}, NumPlayableXTiles={NumPlayableXTiles[y]}"
            );

            // now loop through each tile in the row
            for (int x = 0; x < RoomWidth; ++x)
            {
                roomData[x, y] = new TileData();
                // set up a tile
                Tile tile;

                if (x <= leftX || x >= rightX)
                {
                    tile = ImpassableTiles[Random.Range(0, ImpassableTiles.Length)];
                    roomData[x, y].SetPassable(false);
                }
                else
                {
                    Vector2Int location = new Vector2Int(x, y);

                    tile = PlayableTiles[Random.Range(0, PlayableTiles.Length)];

                    // check if it's a boss tile, since these are impassable
                    if (BossTiles.contains(location))
                    {
                        roomData[x, y].SetPassable(false);
                    }
                    else
                    {
                        roomData[x, y].SetPassable(true);
                        WalkableTiles.Add(location);
                    }
                }

                // tiles always start as not highlighted!
                roomData[x, y].SetHighlighted(false);

                roomData[x, y].SetDefaultTile(tile);
                roomTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        // create copy of our walkable tile list to delete things from
        List<Vector2Int> allowedStatus = new List<Vector2Int>(WalkableTiles);

        // set a bunch of random status tiles
        for (int placedStatus = 0; placedStatus < NumStatusTiles; placedStatus++)
        {
            int index = Random.Range(0, allowedStatus.Count);
            int x = allowedStatus[index].x;
            int y = allowedStatus[index].y;

            Vector3Int location = new Vector3Int(x, y, 0);

            roomData[x, y].SetStatus(true);
            roomData[x, y].SetDefaultTile(statusTile);
            roomTilemap.SetTile(location, statusTile);

            allowedStatus.RemoveAt(index);
        }
    }

    // handles all of the aspects of updating the room
    // does calculations for tile attack changes
    // changes the room sprites accordingly
    public void RefreshRoom(TileAttack newTileAttack = null)
    {
        roomTilemap = GetComponentInChildren<Tilemap>();

        // collects all playable tile locations
        // they must both be passable and not affected by a tileattack or status
        List<Vector2Int> allowableLocations = new List<Vector2Int>();
        // and the locations to update
        List<Vector2Int> updateLocations = new List<Vector2Int>();

        int localLifetime = 0;

        bool isNewTileAttack = true;
        if (newTileAttack is null)
        {
            isNewTileAttack = false;
        }

        // if we're processing tile spread or not
        bool doesSpread = false;

        // grid doesn't need to be updated
        if (!this.TileAttackOnGrid() && !isNewTileAttack)
        {
            return;
        }

        // update the title attack on the grid's total lifetime
        if (TileAttackOnGrid())
        {
            totalTileAttackLifetime -= 1;
        }

        // start updating the grid by looping through/checking all the grid data
        // only for the walkable tiles
        foreach (Vector2Int location in WalkableTiles)
        {
            int x = location.x;
            int y = location.y;
            // so we can access the tile's information
            TileData tileData = roomData[x, y];
            // and for sprite updating
            Vector3Int spriteLocation = new Vector3Int(x, y, 0);

            // swap the highlighted tiles to one that has an attack
            if (tileData.IsHighlighted())
            {
                tileData.SetHighlighted(false);
                updateLocations.Add(new Vector2Int(x, y));
                // sprite processing
                roomTilemap.SetTile(spriteLocation, newTileAttack.GetTileSprite());
                continue;
            }

            // active tile attack updating
            if (TileAttackOnGrid())
            {
                if (tileData.HasTileAttack())
                {
                    updateLocations.Add(new Vector2Int(x, y));
                    // reduce the lifetime
                    // if the tile attack is no longer active, this location is free again
                    tileData.ReduceTileAttackLifetime();
                    if (tileData.HasTileAttack())
                    {
                        if (tileData.GetTileAttack().GetDoesSpread())
                        {
                            doesSpread = true;
                            localLifetime = tileData.GetCopyLifetime();
                        }
                    }
                    else
                    {
                        if (tileData.GetTileAttack().GetDoesSpread())
                        {
                            doesSpread = true;
                        }
                        roomTilemap.SetTile(spriteLocation, tileData.GetDefaultTile());
                        allowableLocations.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    // this tile will be free to add an attack to
                    allowableLocations.Add(new Vector2Int(x, y));
                }
            }
            else
            {
                if (tileData.HasTileAttack())
                {
                    // remove the tile attack
                    updateLocations.Add(new Vector2Int(x, y));
                    tileData.RemoveTileAttack();
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

                    roomData[x, y].SetTileAttack(newTileAttack, localLifetime, false);
                    // WIP - sprite processing here
                    // note - make a method for this, it's called 3 times
                }
            }
            else
            {
                totalTileAttackLifetime = 0;
            }
        }
        else if (isNewTileAttack)
        {
            List<Vector2Int> newAffectedTiles = new List<Vector2Int>();
            totalTileAttackLifetime = newTileAttack.GetTotalLifetime();
            int LocalTileAttackLifetime = newTileAttack.GetLifetime();
            // WIP - replace with the highlight tiles
            Tile newSprite = newTileAttack.GetTileSprite();

            // pass the allowed tiles to the tile attack handler
            // collect the affected tiles
            newAffectedTiles = newTileAttack.PlaceAttack(
                allowableLocations,
                TopImpass + 1,
                RoomWidth
            );

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
                roomData[x, y].SetTileAttack(newTileAttack, LocalTileAttackLifetime, true);
                // WIP - sprite processing
                // these would be highlighted tiles!
                Vector3Int spriteLocation = new Vector3Int(x, y, 0);
                roomTilemap.SetTile(spriteLocation, newSprite);
            }
        }
    }
}
