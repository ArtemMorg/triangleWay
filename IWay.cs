namespace CourseExam
{
    public interface IWay
    {
        int[,] Coordinates { get; set; }
        int CurrentColumn { get; set; }
        int CurrentRow { get; set; }
        string Name { get; set; }
        int Sum { get; set; }

        void nextStep(IMap way);
        void chooseBestPosition(IMap map);
    }
}
