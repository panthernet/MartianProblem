using System;
using System.Collections.Generic;

namespace MartianProblem.Solver.Classes
{
    /// <summary>
    /// This is an example robot implementation which use advanced command actions
    /// Advanced commands is a universal way to easily add new commands to the robot
    /// It is not very fast but very flexible
    /// </summary>
    /// <typeparam name="T">Type of command</typeparam>
    public sealed class AdvancedRobot<T> : RobotBase<T>
    {
        public override int Rotation
        {
            get
            {
                if(AdvancedCommands["rotation"].TryGetValue<int>(out var result))
                    return result;
                return 0;
            }
        }

        public AdvancedRobot(int x, int y, int degree)
            : base(x, y, degree)
        {
            //add new command
            AdvancedCommands.Add("rotate", new RotateCommand(base.Rotation));
        }

        public override void RotateLeft()
        {
            //override rotate method to use new command as an example
            AdvancedCommands["rotate"].Execute(-90);
        }

        public override void RotateRight()
        {
            //override rotate method to use new command as an example
            AdvancedCommands["rotate"].Execute(90);
        }


        public override char GetOrientation()
        {
            throw new NotImplementedException();
        }

        public override void SetOrientation(char orientation)
        {
            throw new NotImplementedException();
        }

        protected override KeyValuePair<int, int> SimulateMove(int positionX, int positionY, int rotation)
        {
            throw new NotImplementedException();
        }
    }
}