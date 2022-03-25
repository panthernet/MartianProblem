using System.Collections.Generic;
using MartianProblem.Solver.Interfaces;

namespace MartianProblem.Solver.Classes
{
    /// <summary>
    /// Base robot class implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RobotBase<T>: IRobot<T>
    {
        #region Properties
        /// <summary>
        /// Robot position on X coordinate
        /// </summary>
        public int PositionX { get; protected set; }
        /// <summary>
        /// Robot position on Y coordinate
        /// </summary>
        public int PositionY { get; protected set; }
        /// <summary>
        /// Robot rotation angle
        /// </summary>
        public virtual int Rotation { get; protected set; }
        /// <summary>
        /// Returns True if robot has been marked as lost
        /// </summary>
        public bool IsLost { get; private set; }

        /// <summary>
        /// Internal queue of stored commands
        /// </summary>
        protected Queue<T> Commands { get; } = new Queue<T>();

        #endregion

        #region Advanced Commands
        /// <summary>
        /// The list of advanced commands for the robot
        /// </summary>
        protected Dictionary<string, IRobotCommand> AdvancedCommands = new Dictionary<string, IRobotCommand>();

        /// <summary>
        /// Tries to execute command by name
        /// </summary>
        /// <param name="commandName">Command name</param>
        /// <param name="args">Input arguments</param>
        public bool ExecuteAction(string commandName, params object[] args)
        {
            try
            {
                return AdvancedCommands.ContainsKey(commandName) && AdvancedCommands[commandName].Execute(args);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        protected RobotBase(int x, int y, int degree)
        {
            PositionX = x;
            PositionY = y;
            Rotation = degree;
        }

        /// <summary>
        /// Add commands into the storage
        /// </summary>
        /// <param name="commands">Commands array</param>
        public void AddCommands(T[] commands)
        {
            foreach (var command in commands)
                Commands.Enqueue(command);
        }

        /// <summary>
        /// Returns orientation as char symbol
        /// </summary>
        public abstract char GetOrientation();
        /// <summary>
        /// Sets orientation from char symbol
        /// </summary>
        /// <param name="orientation">Orientation</param>
        public abstract void SetOrientation(char orientation);

        /// <summary>
        /// Rotate robot right. Virtual method allows redefinition in children.
        /// </summary>
        public virtual void RotateRight()
        {
            Rotation += 90;
            if (Rotation == 360)
                Rotation = 0;
        }

        /// <summary>
        /// Rotate robot left. Virtual method allows redefinition in children.
        /// </summary>
        public virtual void RotateLeft()
        {
            Rotation -= 90;
            if (Rotation < 0)
                Rotation = 270;
        }
        /// <summary>
        /// Simulate next robot move and return new coordinates
        /// </summary>
        /// <returns>Predicted coordinates</returns>
        public KeyValuePair<int, int> SimulateMove()
        {
            return SimulateMove(PositionX, PositionY, Rotation);
        }

        /// <summary>
        /// Perform robot movement
        /// </summary>
        public void Move()
        {
            var (key, value) = SimulateMove(PositionX, PositionY, Rotation);
            PositionX = key;
            PositionY = value;
        }

        /// <summary>
        /// Simulate robot move. Virtual method allows redefinition in children.
        /// </summary>
        /// <param name="positionX">X position</param>
        /// <param name="positionY">Y position</param>
        /// <param name="rotation">Rotation value</param>
        /// <returns>New coordinates</returns>
        protected abstract KeyValuePair<int, int> SimulateMove(int positionX, int positionY, int rotation);

        public void MarkAsLost()
        {
            IsLost = true;
        }

        /// <summary>
        /// Fetch next robot command from queue. Virtual method allows redefinition in children.
        /// </summary>
        /// <returns>Command string</returns>
        public virtual T FetchNextCommand()
        {
            return Commands.TryDequeue(out var result) ? result : default;
        }
    }
}