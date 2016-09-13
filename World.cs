using System;
using System.Collections.Generic;
using System.IO;    

namespace CourseExam
{
    public class World : IMap
    {
        private int[,] _map;
        private int[,] _defaultWay;
        private int[,] _emptyWay;
        private int _stepCount = -1;
        private int _currentRow = 0;
        private string _name = "";
        private List<IWay> _ways = new List<IWay>();

        private delegate void nextStep(IMap way);
        private delegate void nextStepBestPosition(IMap map);
        private event nextStep OnNextStep;
        private event nextStepBestPosition OnNextStepChooseBestPos;

        const int MAX_COLUMN_SIZE = 2;
        const int COLUMN_VALUE = 0;
        const int COLUMN_INDEX = 1;

        public int[,] Map
        {
            get
            {
                return _map;
            }
        }

        public int[,] DefaultWay
        {
            get
            {
                return _defaultWay;
            }
        }

        public int[,] EmptyWay
        {
            get
            {
                return _emptyWay;
            }
        }

        public int stepCount
        {
            get
            {
                return _stepCount;
            }
        }

        public int currentRow
        {
            get
            {
                return _currentRow;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public World(string path, string name)
        {
            _name = name;
            load(path);
        }

        // Method for load map from file
        public void load(string path)
        {
            var reader = new StreamReader(path);
            var lines = new List<String>();
            var readerLine = "";
            // Calculate the total number of rows + save them to generic list
            while (readerLine != null)
            {
                _stepCount++;
                readerLine = reader.ReadLine();
                lines.Add(readerLine);
            }
            // Creating an 2D array that represanting the triangle form of map
            _map = new int[_stepCount, _stepCount];
            // For default way
            _defaultWay = new int[_stepCount, MAX_COLUMN_SIZE];
            // For init 
            _emptyWay = new int[_stepCount, MAX_COLUMN_SIZE];
            var i = 0;
            // Fill arrays
            foreach (var line in lines)
            {
                if (line == null)
                    break;
                string[] values = line.Split(' ');
                for (var j = 0; j < values.Length; j++)
                {
                    _map[i, j] = Convert.ToInt32(values[j]);
                }
                _defaultWay[i, COLUMN_VALUE] = i;
                _defaultWay[i, COLUMN_INDEX] = 0;
                _emptyWay[i, COLUMN_VALUE] = 0;
                _emptyWay[i, COLUMN_INDEX] = 0;
                i++;
            }
        }

        public void print()
        {
            for (var i = 0; i < _stepCount; i++)
            {
                for (var j = 0; j <= i; j++)
                {
                    Console.Write(_map[i, j]);
                    Console.Write('\t');
                }
                Console.WriteLine('\r');
            }
        }

        // Print highlighted indexes to show the best path
        public void printWay(IWay way)
        {
            for (var i = 0; i < _stepCount; i++)
            {
                for (var j = 0; j <= i; j++)
                {
                    if ((i == way.Coordinates[i, 0]) && (j == way.Coordinates[i, 1]))
                    {
                        highlightIndex(i, j);
                    }
                    else
                    {
                        Console.Write(_map[i, j]);
                    }
                    Console.Write('\t');
                }
                Console.WriteLine('\r');
            }
        }

        // Move to next row and call methods of subscribed way in order to calculate and choose best path
        public void stepForward()
        {
            _currentRow++;
            if ((OnNextStep != null) && (OnNextStepChooseBestPos != null))
            {
                // Next row
                OnNextStep(this);
                // Calculate path
                OnNextStepChooseBestPos(this);
            }
        }

        // Gather all ways
        public void addWayToList(IWay way)
        {
            _ways.Add(way);
            subscribeWay(way);
        }

        // If add so del should be
        public void delWayFromList(IWay way)
        {
            _ways.Remove(way);
            unSubscribeWay(way);
        }

        // Subscribe to event
        public void subscribeWay(IWay way)
        {
            OnNextStep += way.nextStep;
            OnNextStepChooseBestPos += way.chooseBestPosition;
        }

        // Unsubscribe to event
        public void unSubscribeWay(IWay way)
        {
            OnNextStep -= way.nextStep;
            OnNextStepChooseBestPos -= way.chooseBestPosition;
        }

        public void printWays()
        {
            foreach(var way in _ways)
            {
                Console.WriteLine(way.Name);
                printWay(way);
            }
        }

        // Print the best way
        public void printBestWay()
        {
            Console.WriteLine("Total number of possible ways={0}", _ways.Count);
            IWay maxWay = _ways[0];
            foreach (var way in _ways)
            {
                calculateSum(way);
                if (maxWay.Sum < way.Sum)
                {
                    maxWay = way;
                }
            }
            Console.WriteLine("The best way is {0} with sum={1}:",maxWay.Name, maxWay.Sum);
            printWay(maxWay);
        }

        public void calculateSum(IWay way)
        {
            for (var i = 0; i < _stepCount; i++)
            {
                for (var j = 0; j <= i; j++)
                {
                    if ((i == way.Coordinates[i, 0]) && (j == way.Coordinates[i, 1]))
                    {
                        way.Sum += this.Map[way.Coordinates[i, 0], way.Coordinates[i, 1]];
                    }
                }
            }
        }
    
        // Init new way and add it to the list
        public void createAddWay(string name, int[,] Coordinates, int CurrentColumn, int CurrentRow)
        {
            var additionalWay = new Way("Additional" + CurrentColumn.ToString(), Coordinates, CurrentColumn);
            additionalWay.Coordinates[currentRow, COLUMN_INDEX] = CurrentColumn;
            additionalWay.CurrentRow = currentRow;
            addWayToList(additionalWay);
        }

        private void highlightIndex(int i, int j)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(_map[i, j]);
            Console.ResetColor();
        }
    }
}
