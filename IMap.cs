namespace CourseExam
{
    public interface IMap
    {
        int[,] Map { get; }
        int[,] DefaultWay { get; }
        int[,] EmptyWay { get; }
        int stepCount { get; }
        int currentRow { get; }
        string Name { get; set; }

        void load(string path);
        void print();
        void printWay(IWay way);
        void stepForward();
        void addWayToList(IWay way);
        void delWayFromList(IWay way);
        void subscribeWay(IWay way);
        void unSubscribeWay(IWay way);
        void printWays();
        void printBestWay();
        void createAddWay(string name, int[,] Coordinates, int CurrentColumn, int CurrentRow);
        void calculateSum(IWay way);
    }
}
