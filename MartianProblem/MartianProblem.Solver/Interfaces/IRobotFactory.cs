using MartianProblem.Solver.Enums;

namespace MartianProblem.Solver.Interfaces
{
    /// <summary>
    /// Robot factory interface
    /// </summary>
    /// <typeparam name="T">Command type</typeparam>
    public interface IRobotFactory<T>
    {
        /// <summary>
        /// Create robot
        /// </summary>
        /// <param name="type">Robot type</param>
        /// <param name="positionX">Initial X position</param>
        /// <param name="positionY">Initial Y position</param>
        /// <param name="orientation">Initial orientation</param>
        /// <returns>New robot</returns>
        IRobot<T> CreateRobot(RobotTypeEnum type, int positionX, int positionY, char? orientation = null);
    }
}