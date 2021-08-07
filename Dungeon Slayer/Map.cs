using System;

namespace Dungeon_Slayer
{
    class Map
    {
        //Number of goblins on a map.
        const int NUM_OF_GOBLINS = 4;
        //Minimal distance form the edge where objects can spawn.
        const int OBJECT_MARGIN = 5;
        //Defines rectangle that will be cleared around certain objects.
        const int OBJECT_CLEARANCE = 3;
        //CAUTION: OBJECT_CLREANCE must be smaller than OBJECT_MARGIN. Otherwise OutOfBounds excemption may occur.


        //Iterations of map smoothing algorithm.
        const int MAX_ITERATIONS = 3;

        //Random Generator.
        readonly Random psrg = new Random();

        //Density of initial map fill (0-100).
        private int _fillDensity;

        //Map dimensions.
        private int _mapHeight;
        private int _mapWidth;
        
        //The Map.
        private int[,] _map;

        //Positions of player's starting point and the portal.
        private Vector2DInt _portalPosition;
        private Vector2DInt _playerStart;

        //Number of goblins on a map.
        private int _numberOfGoblins;

        //Constructor.
        public Map(int width, int height, int fillDensity)
        {
            _mapWidth = width;
            _mapHeight = height;
            _fillDensity = fillDensity;
            _map = new int[_mapWidth, _mapHeight];
            _portalPosition.x = 0;
            _portalPosition.y = 0;
            _playerStart.x = 0;
            _playerStart.y = 0;
            _numberOfGoblins = NUM_OF_GOBLINS;
        }

        //Random block 0 or 1 based on fill density.
        private int RandomBlock()
        {
            return psrg.Next(0, 100) > _fillDensity ? 0 : 1;
        }

        //Fill the map randomly.
        private void RandomFill()
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    //Ensure that the edge is wall.
                    if (x == 0 || y == 0 || x == _mapWidth - 1 || y == _mapHeight - 1)
                    {
                        _map[x, y] = 1;
                    }
                    //Horizontal band in the middle to ensure continuity of empty space.
                    else if (y == (_mapHeight / 2))
                    {
                        _map[x, y] = 0;
                    }
                    //Ignore other objects.
                    else
                    {
                        _map[x, y] = RandomBlock();
                    }
                }
            }
        }

        //Place portal in random cell in lower right octant of the map.
        private void PlacePortal()
        {
            int maxX = _mapWidth / 4;
            int maxY = _mapHeight / 4;

            int x = psrg.Next(_mapWidth - maxX, _mapWidth - OBJECT_MARGIN);
            int y = psrg.Next(_mapHeight - maxY, _mapHeight - OBJECT_MARGIN);

            _portalPosition.x = x;
            _portalPosition.y = y;

            _map[x, y] = 3;
        }

        //Place random starting point for player in upper left octant of the map.
        private void ChoosePlayerStartingPoint()
        {
            int maxX = _mapWidth / 4;
            int maxY = _mapHeight / 4;

            int x = psrg.Next(OBJECT_MARGIN, maxX);
            int y = psrg.Next(OBJECT_MARGIN, maxY);

            _playerStart.x = x;
            _playerStart.y = y;
        }

        //Clear rectangular portion of the map around a given point.
        private void ClearAroundPoint(Vector2DInt position)
        {
            for (int i = position.x - OBJECT_CLEARANCE; i <= position.x + OBJECT_CLEARANCE; i++)
            {
                for (int j = position.y - OBJECT_CLEARANCE; j <= position.y + OBJECT_CLEARANCE; j++)
                {
                    if (!(i == position.x && j == position.y))
                        _map[i, j] = 0;
                }
            }
        }

        //Count number of walls in adjacent cells.
        private int CountAdjacentWalls(int x, int y)
        {
            int wallCount = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if(!(i == x && j == y))
                    {
                        if (IsWall(new Vector2DInt (i, j)))
                            wallCount++;
                    }
                }
            }

            return wallCount;
        }

        //Place wall logic.
        private int PlaceWall(int x, int y)
        {
            int adjWalls = CountAdjacentWalls(x, y);

            if (_map[x, y] == 1)
            {
                if (adjWalls >= 4)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (_map[x, y] == 0)
            {
                if (adjWalls >= 5)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                //Leave other object than walls and empty tiles alone.
                return _map[x, y];
            }
        }
        
        //Carve caves by cellural automata algorithm.
        private void CaveMap(int numberOfIterations)
        {
            int[,] temp = new int[_mapWidth, _mapHeight];
            temp = _map;

            for (int i = 0; i < numberOfIterations; i++)
            {
                for (int x = 1; x < _mapWidth - 1; x++)
                {
                    for (int y = 1; y < _mapHeight - 1; y++)
                    {
                        temp[x, y] = PlaceWall(x, y);
                    }
                }
            }

            _map = temp;
        }

        //Choose random spot in free space.
        private Vector2DInt RandomPointInFreeSpace()
        {
            bool freeSpotFound = false;

            Vector2DInt spot = new Vector2DInt(0, 0);

            while (!freeSpotFound)
            {
                spot = new Vector2DInt(psrg.Next(1, _mapWidth - 1), psrg.Next(1, _mapHeight - 1));

                if (IsFreeSpot(spot))
                    freeSpotFound = true;
            }

            return spot;
        }

        //Spawn goblins.
        private void SpawnGoblins(int numberOfGoblins)
        {
            Vector2DInt newGoblinPos;

            for(int i = 0; i < numberOfGoblins; i++)
            {
                newGoblinPos = RandomPointInFreeSpace();
                _map[newGoblinPos.x, newGoblinPos.y] = 5;
            }
        }

        //Turn number codes into tiles.
        private string DecodeSymbol(int code)
        {
            string symbol = "!"; //error symbol

            switch(code)
            {
                case 0:
                    symbol = " "; //blank space
                    break;
                case 1:
                    symbol = "#"; //wall tile
                    break;
                case 3:
                    symbol = "0"; //inactive portal
                    break;
                case 4:
                    symbol = "O"; //portal
                    break;
                case 5:
                    symbol = "G"; //goblin
                    break;
            }

            return symbol;
        }

        //Check object code at given position.
        private int CheckObj(Vector2DInt pos)
        {
            return _map[pos.x, pos.y];
        }

        //Check if given cell is wall.
        public bool IsWall(Vector2DInt position)
        {
            return _map[position.x, position.y] == 1;
        }

        //Check if given cell is walkable.
        private bool IsFreeSpot(Vector2DInt pos)
        {
            //other walkable tiles can be coded with negative ints
            return _map[pos.x, pos.y] < 1; 
        }

        //Initialising map.
        public void StartMap()
        {
            RandomFill();
            PlacePortal();
            ClearAroundPoint(_portalPosition);
            ChoosePlayerStartingPoint();
            ClearAroundPoint(_playerStart);
            CaveMap(MAX_ITERATIONS);
            SpawnGoblins(NUM_OF_GOBLINS);
            _numberOfGoblins = NUM_OF_GOBLINS;
        }

        //Draw map on screen.
        public void DrawMap()
        {
            Console.Clear();
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    WriteAt(new Vector2DInt(x, y), DecodeSymbol(_map[x, y]));
                }
            }
        }

        //Remove any object from given cell.
        public void BlankCell(Vector2DInt coords)
        {
            _map[coords.x, coords.y] = 0;
        }

        //Access to chosen starting point.
        public Vector2DInt GetPlayerStart()
        {
            return _playerStart;
        }

        //Check if move to target is permitted.
        public bool IsMovePermitted(Vector2DInt targetPosition)
        {
            return CheckObj(targetPosition) == 0 || CheckObj(targetPosition) > 3;
        }
        

        //Check what kind of object is at given location.
        public int GetObjType(Vector2DInt pos)
        {
            return _map[pos.x, pos.y];
        }

        //Write symbol at given location.
        public void WriteAt(Vector2DInt pos, string obj)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write(obj);
        }

        //Decrement number of goblins.
        public void RemoveGoblin()
        {
            _numberOfGoblins--;
        }

        //Goblin count getter.
        public int GoblinCount
        {
            get
            {
                return _numberOfGoblins;

            }
        }

        //Portal activation.
        public void ActivatePortal()
        {
            _map[_portalPosition.x, _portalPosition.y] = 4;
        }
    }
}
