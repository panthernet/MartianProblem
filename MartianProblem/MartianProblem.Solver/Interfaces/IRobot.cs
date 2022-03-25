using System.Collections.Generic;

namespace MartianProblem.Solver.Interfaces
{
    /// <summary>
    /// Robot interface
    /// </summary>
    public interface IRobot<T>
    {
        /// <summary>
        /// Robot position on X coordinate
        /// </summary>
        int PositionX { get; }
        /// <summary>
        /// Robot position on Y coordinate
        /// </summary>
        int PositionY { get; }
        /// <summary>
        /// Robot rotation angle
        /// </summary>
        int Rotation { get; }
        /// <summary>
        /// Returns True if robot has been marked as lost
        /// </summary>
        bool IsLost { get; }
        /// <summary>
        /// Fetch next robot command from queue. Virtual method allows redefinition in children.
        /// </summary>
        /// <returns>Command string</returns>
        T FetchNextCommand();
        /// <summary>
        /// Add commands into the storage
        /// </summary>
        /// <param name="commands">Commands array</param>
        void AddCommands(T[] commands);
        /// <summary>
        /// Rotate robot right
        /// </summary>
        void RotateRight();
        /// <summary>
        /// Rotate robot left
        /// </summary>
        void RotateLeft();
        /// <summary>
        /// Simulate next robot move and return new coordinates
        /// </summary>
        /// <returns>Predicted coordinates</returns>
        KeyValuePair<int,int> SimulateMove();
        /// <summary>
        /// Perform robot movement
        /// </summary>
        void Move();
        /// <summary>
        /// Mark robot as lost
        /// </summary>
        void MarkAsLost();
        /// <summary>
        /// Returns orientation as char symbol
        /// </summary>
        char GetOrientation();
        /// <summary>
        /// Sets orientation from char symbol
        /// </summary>
        /// <param name="orientation">Orientation</param>
        void SetOrientation(char orientation);

        /// <summary>
        /// Tries to execute advanced command action by name
        /// </summary>
        /// <param name="commandName">Command name</param>
        /// <param name="args">Input arguments</param>
        bool ExecuteAction(string commandName, params object[] args);
    }
}