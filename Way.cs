using System;

namespace CourseExam
{
    public class Way : IWay
    {
        private int[,] _coordinates;
        private int _currentColumn = 0;
        private int _currentRow = 0;
        private string _name = "";
        private int _sum = 0;

        const int COLUMN_VALUE = 0;
        const int COLUMN_INDEX = 1;
        const int NEIGHBOR_COUNT = 3;
        const int COLUMN_COUNT = 2;
        const int NO_HIGH_ELEMENT = -1;

        public int[,] Coordinates
        {
            get
            {
                return _coordinates;
            }
            set
            {
                _coordinates = value;
            }
        }

        public int CurrentColumn
        {
            get
            {
                return _currentColumn;
            }
            set
            {
                _currentColumn = value;
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

        public int CurrentRow
        {
            get
            {
                return _currentRow;
            }

            set
            {
                _currentRow = value;
            }
        }

        public int Sum
        {
            get
            {
                return _sum;
            }
            set
            {
                _sum = value;
            }
        }

        public Way(string name)
        {
            _name = name;
        }

        public Way(string name, int[,] Coordinates, int CurrentColumn)
        {
            _name = name;
            _coordinates = Coordinates;
            _currentColumn = CurrentColumn;
        }

        public void nextStep(IMap map)
        {
            _currentRow = map.currentRow;
        }

        public void chooseBestPosition(IMap map)
        {
            try
            {
                var current = this;
                if (current.CurrentColumn == 0)
                {
                    // Left value is less than right
                    if (map.Map[current.CurrentRow, current.CurrentColumn] < map.Map[current.CurrentRow, current.CurrentColumn + 1])
                    {
                        current.Coordinates[current.CurrentRow, COLUMN_VALUE] = current.CurrentRow;
                        current.Coordinates[current.CurrentRow, COLUMN_INDEX] = current.CurrentColumn + 1;
                        current.CurrentColumn += 1;
                    }
                    // Equal
                    else if (map.Map[current.CurrentRow, current.CurrentColumn] == map.Map[current.CurrentRow, current.CurrentColumn + 1])
                    {
                        current.Coordinates[current.CurrentRow, COLUMN_VALUE] = current.CurrentRow;
                        current.Coordinates[current.CurrentRow, COLUMN_INDEX] = current.CurrentColumn;

                        var newCurrentColumn = current.CurrentColumn + 1;
                        var newCoordinates = copyArray(current.Coordinates);

                        newCoordinates[current.CurrentRow, COLUMN_VALUE] = current.CurrentRow;
                        newCoordinates[current.CurrentRow, COLUMN_INDEX] = newCurrentColumn;
                        map.createAddWay("Additional" + newCurrentColumn.ToString(), newCoordinates, newCurrentColumn, current.CurrentRow);
                        return;
                    }
                    // Higher
                    else
                    {
                        current.Coordinates[current.CurrentRow, COLUMN_VALUE] = current.CurrentRow;
                        current.Coordinates[current.CurrentRow, COLUMN_INDEX] = current.CurrentColumn;
                    }
                    return;
                }
                else
                {
                    // Array to collect neighbor values with their indexes
                    var checkInt = new Int32[NEIGHBOR_COUNT, COLUMN_COUNT];
                    
                    // Fill it
                    for(int i=0; i<NEIGHBOR_COUNT; i++)
                    {
                        var shift = i - 1;
                        checkInt[i, COLUMN_VALUE] = map.Map[current.CurrentRow, current.CurrentColumn + shift];
                        checkInt[i, COLUMN_INDEX] = current.CurrentColumn + shift;
                    }

                    // Look for biggest value or some count of them
                    int[] indexes = findMaxIndexes(checkInt);
                    var another = false;

                    foreach(var index in indexes)
                    {
                        // If values are identical
                        if (another && (index >= 0))
                        {
                            map.createAddWay("Additional", copyArray(current.Coordinates), index, current.CurrentRow);
                            another = false;
                            continue;
                        }

                        if ((another==false) && index >= 0)
                        {
                            current.Coordinates[current.CurrentRow, COLUMN_VALUE] = current.CurrentRow;
                            current.Coordinates[current.CurrentRow, COLUMN_INDEX] = index;
                            current.CurrentColumn = index;
                            another = true;
                        }
                    }
                }
            }
           catch
            {
                // List of ways is updated. Cool!
            }
        }

        // Simple logic to find one or more high values
        private int[] findMaxIndexes(int[,] source)
        {
            int[] output = { NO_HIGH_ELEMENT, NO_HIGH_ELEMENT, NO_HIGH_ELEMENT };

            if((source[0, COLUMN_VALUE] > source[1, COLUMN_VALUE]) && (source[0, COLUMN_VALUE] > source[2, COLUMN_VALUE]))
            {
                output[0] = source[0, COLUMN_INDEX];
            } 
            else if((source[1, COLUMN_VALUE] > source[0, COLUMN_VALUE]) && (source[1, COLUMN_VALUE] > source[2, COLUMN_VALUE]))
            {
                output[1] = source[1, COLUMN_INDEX];
            }
            else if ((source[2, COLUMN_VALUE] > source[0, COLUMN_VALUE]) && (source[2, COLUMN_VALUE] > source[1, COLUMN_VALUE]))
            {
                output[2] = source[2, COLUMN_INDEX];
            }
            // If none of values is highest - some of them are equal
            else if((source[0, COLUMN_VALUE] == source[1, COLUMN_VALUE]) && (source[0, COLUMN_VALUE] == source[2, COLUMN_VALUE]))
            {
                output[0] = source[0, COLUMN_INDEX];
                output[1] = source[1, COLUMN_INDEX];
                output[2] = source[2, COLUMN_INDEX];
            }
            else if(source[0, COLUMN_VALUE] == source[1, COLUMN_VALUE])
            {
                output[0] = source[0, COLUMN_INDEX];
                output[1] = source[1, COLUMN_INDEX];
            }
            else if(source[0, COLUMN_VALUE] == source[2, COLUMN_VALUE])
            {
                output[0] = source[0, COLUMN_INDEX];
                output[2] = source[2, COLUMN_INDEX];
            }
            else if (source[1, COLUMN_VALUE] == source[2, COLUMN_VALUE])
            {
                output[1] = source[1, COLUMN_INDEX];
                output[2] = source[2, COLUMN_INDEX];
            }

            return output;
        }

        private int[,] copyArray(int[,] source)
        {
            var output = new Int32[source.GetLength(0), source.GetLength(1)];
            for(var i=0; i< output.GetLength(0); i++)
            {
                for(var j=0; j< output.GetLength(1); j++)
                {
                    output[i, j] = source[i, j];
                }
            }
            return output;
        }

    }
}
