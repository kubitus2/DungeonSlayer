using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{
    //Class handling all map related stuff: rendering, generating, etc.
    class Map
    {
        //minimal distance form the edge where objects can spawn
        const int OBJECT_MARGIN = 5;
        //defines rectangle that will be cleared around certain object
        const int OBJECT_CLEARANCE = 3;
        //iterations of map smoothing algorithm
        const int MAX_ITERATIONS = 4;

        readonly Random psrg = new Random();

        //density of initial map fill (0-100).
        private int _fillDensity;

        //map dimensions
        private int _mapHeight;
        private int _mapWidth;
        
        //actual map
        private int[,] _map;

        //Positions of player's starting point and the portal.
        private Vector2DInt _portalPosition;
        private Vector2DInt _playerStart;

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
        }

        //random block 0 or 1 based on denssity
        private int RandomBlock()
        {
            int rand = psrg.Next(0, 100);
            return rand < _fillDensity ? 0 : 1;
        }

        //fill the map randomly
        private void RandomFill()
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    //ensure that the edge is wall
                    if (x == 0 || y == 0 || x == _mapWidth - 1 || y == _mapHeight - 1)
                    {
                        _map[x, y] = 1;
                    }
                    //horizontal band in the middle to ensure continuity of empty space
                    else if (y == (_mapHeight / 2))
                    {
                        _map[x, y] = 0;
                    }
                    //ignore other objects
                    else
                    {
                        _map[x, y] = RandomBlock();
                    }
                }
            }
        }

        //place portal in random cell in lower right octant of the map.
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

        //place random starting point for player in upper left octant of the map.
        private void ChoosePlayerStartingPoint()
        {
            int maxX = _mapWidth / 4;
            int maxY = _mapHeight / 4;

            int x = psrg.Next(OBJECT_MARGIN, maxX);
            int y = psrg.Next(OBJECT_MARGIN, maxY);

            _playerStart.x = x;
            _playerStart.y = y;

        }

        //clear rectangular portion of the map around given point.
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

        //Check if cell contains wall
        public bool IsWall(Vector2DInt position)
        {
            return _map[position.x, position.y] == 1;
        }

        public bool IsWall(int x, int y)
        {
            return _map[x, y] == 1;
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
                        if (IsWall(i, j))
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
                return _map[x, y];
            }
        }
        //
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


        //Turn number codes into tiles.
        private string DecodeSymbol(int code)
        {
            string symbol = "!";

            switch(code)
            {
                case 0:
                    symbol = " ";
                    break;
                case 1:
                    symbol = "#";
                    break;
                case 3:
                    symbol = "0";
                    break;
                case 4:
                    symbol = "O";
                    break;
            }

            return symbol;
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
        }

        //Draw map on screen.
        public void DrawMap()
        {
            Console.Clear();

            string currentSymbol = string.Empty;

            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    Console.SetCursorPosition(x, y);
                    currentSymbol = DecodeSymbol(_map[x, y]);
                    Console.Write(currentSymbol);
                }
            }
        }

        //Remove any object from given cell.
        public void BlankCell(int x, int y)
        {
            _map[x, y] = 0;
        }

     

        public void UpdateCell(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(DecodeSymbol(_map[x, y]));
        }

        //Access to chosen starting point.
        public Vector2DInt GetPlayerStart()
        {
            return _playerStart;
        }


    }
}
