using System;
using MartianProblem.Solver.Interfaces;

namespace MartianProblem.Solver.Classes
{
    internal sealed class RotateCommand: IRobotCommand
    {
        private int _rotation;

        public RotateCommand(int rotation)
        {
            _rotation = rotation;
        }

        public bool Execute(params object[] args)
        {
            if (args.Length != 1 || !(args[0] is int))
                return false;
            var value = Convert.ToInt32(args[0]);
            _rotation += value;
            return true;
        }

        public bool TryGetValue<T>(out T value)
        {
            try
            {
                value = (T) (object) _rotation;
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }
    }
}
