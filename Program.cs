using System;
using System.Collections.Generic;

namespace CourseExam
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initial steps
            const int START_COLUMN_INDEX = 0;
            var worlds = new List<World>();
            var mapShort = new World("Input-short.txt", "short");
            var mapLarge = new World("Input-large.txt", "large");
            worlds.Add(mapShort);
            worlds.Add(mapLarge);

            foreach(var map in worlds)
            {
                Console.WriteLine("Current map name \"{0}\":", map.Name);
                map.print();
                // At least one way should be - Base
                var baseWay = new Way("Base", map.EmptyWay, START_COLUMN_INDEX);
                map.addWayToList(baseWay);

                // Looking ways...
                for (var i = 0; i < map.stepCount; i++)
                {
                    map.stepForward();
                }
                // Print the best way found

                map.printBestWay();
            }

            Console.ReadLine();    
        }
    }
}
