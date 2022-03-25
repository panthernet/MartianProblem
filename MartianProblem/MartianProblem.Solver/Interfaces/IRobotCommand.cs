namespace MartianProblem.Solver.Interfaces
{
    public interface IRobotCommand
    {
        bool Execute(params object[] args);
        bool TryGetValue<T>(out T value);
    }
}