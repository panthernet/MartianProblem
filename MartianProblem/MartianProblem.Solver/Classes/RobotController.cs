using System;
using System.Collections.Generic;
using System.Linq;
using MartianProblem.Solver.Enums;
using MartianProblem.Solver.Interfaces;

namespace MartianProblem.Solver.Classes
{
    /// <summary>
    /// Robot controller implementation
    /// </summary>
    public sealed class RobotController: IRobotController
    {
        /// <summary>
        /// List to store robots
        /// </summary>
        private readonly List<IRobot<char>> _robotsList = new List<IRobot<char>>();

        /// <summary>
        /// List to store danger zone coordinates
        /// </summary>
        private readonly List<KeyValuePair<int, int>> _dangerZones = new List<KeyValuePair<int, int>>();

        /// <summary>
        /// Default robot factory. We specify command type for robots.
        /// </summary>
        private readonly RobotFactory<char> _factory = new RobotFactory<char>();

        #region Settings & Options

        /// <summary>
        /// Maximum command length
        /// </summary>
        private const int MAX_COMMAND_LENGTH = 100;

        /// <summary>
        /// Maximum zone size on either dimension
        /// </summary>
        private const int MAX_ZONE_SIZE = 50;


        /// <summary>
        /// Predefined list of acceptable commands
        /// </summary>
        private readonly List<char> _acceptableCommands = new List<char> {'L', 'R', 'F'};

        /// <summary>
        /// Zone size for X coordinate
        /// </summary>
        private int _zoneSizeX;

        /// <summary>
        /// Zone size for Y coordinate
        /// </summary>
        private int _zoneSizeY;

        #endregion

        #region State control

        /// <summary>
        /// Indicate if controller has been initialized
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// Stores current command input state, i.e. which command to expect next
        /// </summary>
        private CommandState _state;
        
        #endregion

        /// <summary>
        /// Input command into the controller
        /// </summary>
        /// <param name="command">Command text</param>
        /// <returns>True if ok, False if sequence has a failure</returns>
        public bool InputCommand(string command)
        {
            try
            {
                if (!_isInitialized)
                {
                    //assume we've got zone init command
                    return InitializeZone(command);
                }

                switch (_state)
                {
                    case CommandState.AwaitingCommand:
                        if (!InitializeCommand(command))
                            return false;
                        else _state = CommandState.AwaitingRobot;
                        return true;
                    case CommandState.AwaitingRobot:
                        if (!InitializeRobot(command))
                            return false;
                        else _state = CommandState.AwaitingCommand;
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                LogEvent($"Unexpected exception during '{command}' command handling: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Initialize zone width and height
        /// </summary>
        /// <param name="command">Command text</param>
        /// <returns>True if ok, False if sequence has a failure</returns>
        private bool InitializeZone(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                LogEvent($"Invalid zone initialization command: {command}");
                return false;
            }

            var list = command.Split(' ');
            if (list.Length != 2 || !int.TryParse(list[0], out var x) || x > MAX_ZONE_SIZE ||
                !int.TryParse(list[1], out var y) || y > MAX_ZONE_SIZE || x<=0 || y<=0)
            {
                LogEvent($"Invalid zone initialization command: {command}");
                return false;
            }

            _zoneSizeX = x;
            _zoneSizeY = y;
            _isInitialized = true;

            return true;
        }

        /// <summary>
        /// Add and initialize new robot
        /// </summary>
        /// <param name="command">Command text</param>
        /// <returns>True if ok, False if sequence has a failure</returns>
        private bool InitializeRobot(string command)
        {
            try
            {
                //validate and parse robot init command
                if (!ParseAndValidateCommandString(command, out var positionX, out var positionY, out var rotation))
                {
                    LogEvent($"Invalid robot initialization command: {command}");
                    return false;
                }

                //we use factory to create robot so controller won't depend on exact robot implementation
                var robot = _factory.CreateRobot(RobotTypeEnum.Default, positionX, positionY, rotation);
                _robotsList.Add(robot);

                return true;
            }
            catch (Exception ex)
            {
                LogEvent($"There was an unexpected error while creating new robot: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Add new command to the last robot
        /// </summary>
        /// <param name="command">Command text</param>
        /// <returns>True if ok, False if sequence has a failure</returns>
        private bool InitializeCommand(string command)
        {
            if (!_robotsList.Any())
            {
                LogEvent("Command sequence is malformed");
                return false;
            }

            if (string.IsNullOrEmpty(command) || command.Length > MAX_COMMAND_LENGTH || !command.All(a => _acceptableCommands.Contains(a)))
            {
                LogEvent($"Invalid command: {command}");
                return false;
            }

            var robot = _robotsList.Last();
            robot.AddCommands(command.ToCharArray());
            return true;
        }

        /// <summary>
        /// Run all robots from the controller
        /// </summary>
        public bool Run()
        {
            if (!_isInitialized)
            {
                LogEvent("Controller is not initialized!");
                return false;
            }

            foreach (var robot in _robotsList)
            {
                try
                {
                    char command;
                    //work until we have commands
                    do
                    {
                        //fetch next command
                        command = robot.FetchNextCommand();
                        switch (command)
                        {
                            case 'L':
                                robot.RotateLeft();
                                break;
                            case 'R':
                                robot.RotateRight();
                                break;
                            case 'F':
                                //simulate robot move because we're cautious
                                var position = robot.SimulateMove();
                                //check if we're not going to danger zone
                                if (!CheckIfHeadingToDangerZone(position))
                                {
                                    //check if we're lost
                                    CheckAndUpdateRobotStatus(robot, position);
                                    if (!robot.IsLost)
                                        robot.Move();
                                }

                                break;
                            case default(char):
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(command), command,
                                    "Unknown robot command");
                        }
                        //exit the loop if robot is lost
                        if (robot.IsLost)
                            break;
                    } while (command != default(char));

                    LogEvent($"{robot.PositionX} {robot.PositionY} {robot.GetOrientation()}{(robot.IsLost ? " LOST" : null)}");
                }
                catch (Exception ex)
                {
                    LogEvent($"Robot has failed unexpectedly. Message: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if robot is going to move into the danger zone
        /// </summary>
        /// <param name="position">Predicted position</param>
        /// <returns>True if robot is going to visit danger zone</returns>
        private bool CheckIfHeadingToDangerZone(KeyValuePair<int, int> position)
        {
            foreach (var (key,value) in _dangerZones.ToList())
            {
                if (position.Key == key && position.Value == value)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check robot status and update it if it has been lost
        /// </summary>
        /// <param name="robot">Robot</param>
        /// <param name="position">Predicted position</param>
        private void CheckAndUpdateRobotStatus(IRobot<char> robot, KeyValuePair<int, int> position)
        {
            if (position.Key > _zoneSizeX || position.Value > _zoneSizeY || position.Key < 0
                || position.Value < 0)
            {
                //mark robot as lost
                robot.MarkAsLost();
                //add new danger zone coordinates
                _dangerZones.Add(new KeyValuePair<int, int>(position.Key, position.Value));
            }
        }

        /// <summary>
        /// Validate and parse robot actions command
        /// </summary>
        /// <param name="command">Command text</param>
        /// <param name="positionX">Output X position</param>
        /// <param name="positionY">Output Y position</param>
        /// <param name="rotation">Output rotation indicator</param>
        /// <returns>True if command is valid, otherwise return False</returns>
        private static bool ParseAndValidateCommandString(string command, out int positionX, out int positionY, out char? rotation)
        {
            positionX = 0;
            positionY = 0;
            rotation = null;

            if (string.IsNullOrEmpty(command))
                return false;

            var commands = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //input command validation
            if (commands.Length != 3 || !int.TryParse(commands[0], out positionX) ||
                !int.TryParse(commands[1], out positionY) || commands[2].Length !=1)
                return false;

            rotation = commands[2][0];
            return true;
        }

        /// <summary>
        /// Centralized log writing method. Default implementation writes to console.
        /// </summary>
        /// <param name="message">Log message</param>
        private void LogEvent(string message)
        {
            Console.WriteLine(message);
        }
    }
}
