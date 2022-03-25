using System;
using System.Collections.Generic;
using System.IO;

namespace MartianProblem.Solver.Classes
{
    /// <summary>
    /// Default implementation for the robot
    /// </summary>
    /// <typeparam name="T">Command type</typeparam>
    public sealed class Robot<T>: RobotBase<T>
    {
        public Robot(int x, int y, int degree)
        : base(x, y, degree)
        {

        }

        public override char GetOrientation()
        {
            switch (Rotation)
            {
                case 0: return 'N';
                case 90: return 'E';
                case 180: return 'S';
                case 270: return 'W';
                default: throw new ArgumentOutOfRangeException(nameof(Rotation), Rotation, "Invalid rotation value");
            }
        }

        public override void SetOrientation(char orientation)
        {
            switch (orientation)
            {
                case 'N':
                    Rotation = 0;
                    return;
                case 'E':
                    Rotation = 90;
                    return;
                case 'S':
                    Rotation = 180;
                    return;
                case 'W':
                    Rotation = 270;
                    return;
                default: throw new ArgumentOutOfRangeException(nameof(Rotation), Rotation, "Invalid rotation value");
            }
        }

        /// <summary>
        /// Simulate robot move
        /// </summary>
        /// <param name="positionX">X position</param>
        /// <param name="positionY">Y position</param>
        /// <param name="rotation">Rotation value</param>
        /// <returns>New coordinates</returns>
        /// <exception cref="InvalidDataException"></exception>
        protected override KeyValuePair<int,int> SimulateMove(int positionX, int positionY, int rotation)
        {
            switch (rotation)
            {
                case 0:
                    positionY += 1;
                    break;
                case 90:
                    positionX += 1;
                    break;
                case 180:
                    positionY -= 1;
                    break;
                case 270:
                    positionX -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, $"Unknown value {rotation}");
            }

            return new KeyValuePair<int, int>(positionX, positionY);
        }
    }
}
