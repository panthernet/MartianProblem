namespace MartianProblem.Solver.Interfaces
{
    /// <summary>
    /// Robot controller interface
    /// </summary>
    public interface IRobotController
    {
        /// <summary>
        /// Run all robots from the controller
        /// </summary>
        /// <returns>True if ok, False if sequence has a failure</returns>
        bool Run();

        /// <summary>
        /// Input command into the controller
        /// </summary>
        /// <param name="command">Command text</param>
        /// <returns>True if ok, False if sequence has a failure</returns>
        bool InputCommand(string command);
    }
}