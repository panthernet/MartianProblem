using System;
using MartianProblem.Solver.Enums;
using MartianProblem.Solver.Interfaces;

namespace MartianProblem.Solver.Classes
{
    /// <summary>
    /// Robot factory implementation
    /// </summary>
    /// <typeparam name="T">Type of robot command</typeparam>
    public class RobotFactory<T>: IRobotFactory<T>
    {
        /// <summary>
        /// Create robot
        /// </summary>
        /// <param name="type">Robot type</param>
        /// <param name="positionX">Initial X position</param>
        /// <param name="positionY">Initial Y position</param>
        /// <param name="orientation">Initial orientation</param>
        /// <returns>New robot</returns>
        public virtual IRobot<T> CreateRobot(RobotTypeEnum type, int positionX, int positionY, char? orientation = null)
        {
            switch (type)
            {
                case RobotTypeEnum.Default:
                    var robot = new Robot<T>(positionX, positionY, 0);
                    if(orientation.HasValue)
                        robot.SetOrientation(orientation.Value);
                    return robot;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, $"Unknown robot type: {type}");
            }
        }
    }
}