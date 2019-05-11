using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace lde_test
{
    public class Robot
    {

        public RobotMemento _robotMemento;

        public Position Position { get; set; }
        public int Battery { get; set; }
        public int NextStrategy { get; set; }
        public FacingControl FacingControl { get; set; }
        public MovingControl MovingControl { get; set; }
        public string[] Strategies;
        public List<Location> VisitedCells { get; set; }
        public List<ElementType> SamplesCollected { get; set; }
        public ElementType[,] Terrain { get; set; }
        public Stack<CommandType> StackCommandTypes { get; set; }

        public List<Result> SolutionStepByStep { get; set; }

        public Robot(Position position, int battery, string[] strategies,
            FacingControl facingControl, MovingControl movingControl,
            ElementType[,] terrainElementTypes, Stack<CommandType> stackCommandTypes)
        {
            Position = position;
            Battery = (battery > 0) ? battery : 0;
            NextStrategy = 0;
            FacingControl = facingControl;
            MovingControl = movingControl;
            Strategies = strategies;
            Terrain = terrainElementTypes;
            StackCommandTypes = stackCommandTypes;
            VisitedCells = new List<Location> {Position.Location};
            SamplesCollected = new List<ElementType>();

            SolutionStepByStep = new List<Result>();

            _robotMemento = new RobotMemento(position, battery, strategies, facingControl, movingControl);
        }

        public void RevertToOriginalValues()
        {
            Position = _robotMemento.Position;
            StackCommandTypes = _robotMemento.StackCommandTypes;
            VisitedCells = _robotMemento.VisitedCells;
            SamplesCollected = _robotMemento.SamplesCollected;
            SolutionStepByStep = _robotMemento.SolutionStepByStep;
        }

        private void GetValuesBeforeCommand()
        {
            _robotMemento.Position = Position;
            _robotMemento.VisitedCells = VisitedCells;
            _robotMemento.SamplesCollected = SamplesCollected;
            _robotMemento.StackCommandTypes = StackCommandTypes;
            _robotMemento.SolutionStepByStep = SolutionStepByStep;
        }

       

        // Factory Method

        public static Robot Create(JToken initialPosition, JToken battery, string[] robotStrategies,
            FacingControl facingControl, MovingControl movingControl,
            ElementType[,] terrainElementTypes, Stack<CommandType> stackCommandTypes)
        {
            // Validate parameters, throw exceptions

            var locationObject = JObject.Parse(initialPosition.ToString())["location"];
            int x = (int) JObject.Parse(locationObject.ToString())["x"];
            int y = (int) JObject.Parse(locationObject.ToString())["y"];

            var facingObject = (JToken) JObject.Parse(initialPosition.ToString())["facing"];
            var facing = (Facing) Enum.Parse(typeof (Facing), facingObject.ToString());

            Location location = new Location(x, y);

            Position position = new Position(location, facing);

            return new Robot(position, battery.Value<int>(), robotStrategies,
                facingControl, movingControl,
                terrainElementTypes, stackCommandTypes);
        }

        public void Execute()
        {
            var executeFunctions = new Dictionary<CommandType, Action>
            {
                {CommandType.TurnRight, () => new TurnRightCommand().ExecuteCommand(this)},
                {CommandType.TurnLeft, () => new TurnLeftCommand().ExecuteCommand(this)},
                {CommandType.TakeSample, () => new TakeSampleCommand().ExecuteCommand(this)},
                {
                    CommandType.ExtendSolarPanels, () => new ExtendSolarPanelsCommand(new ConsumeBatteryCommand(),
                        new ChargeBatteryCommand()).ExecuteCommand(this)
                },
                {CommandType.MoveForward, () => new MoveForwardCommand().ExecuteCommand(this)},
                {CommandType.MoveBackwards, () => new MoveBackwardsCommand().ExecuteCommand(this)},
            };

            // Mientras queden comandos por ejecutar....

            while (StackCommandTypes.Any())
            {
                var command = StackCommandTypes.Pop();

                GetValuesBeforeCommand();
                executeFunctions[command].Invoke();

                var result = new Result(VisitedCells, SamplesCollected, Position.Location,
                    Position.Facing, Battery);

                SolutionStepByStep.Add(result);
            }
        }

      
        public Stack<CommandType> ApplyNextStrategy(Stack<CommandType> stackCommandTypes, int nextStrategy)
        {
            // Añadimos encima de la pila de comandos original, los comandos de la estrategia seleccionada. 

            if (nextStrategy < Strategies.Length)
            {
                var strategy = Strategies[nextStrategy];
                var strategies = strategy.Split(',');
                var strategyCommands = TransformCommands(strategies);

                var arrayCommands = strategyCommands.ToArray();

                for (int i = arrayCommands.Length - 1; i >= 0; i--)
                {
                    stackCommandTypes.Push(arrayCommands[i]);
                }
            }

            return stackCommandTypes;

        }

        private static Stack<CommandType> TransformCommands(string[] strategies)
        {
            var commandDictionary = new Dictionary<string, CommandType>
            {
                {"S", CommandType.TakeSample},
                {"F", CommandType.MoveForward},
                {"B", CommandType.MoveBackwards},
                {"L", CommandType.TurnLeft},
                {"R", CommandType.TurnRight},
                {"E", CommandType.ExtendSolarPanels}
            };


            var stackCommands = new Stack<CommandType>();
            for (int i = strategies.Length - 1; i >= 0; i--)
            {
                var command = commandDictionary[strategies[i]];
                stackCommands.Push(command);
            }

            return stackCommands;
        }

        public void ResetRobot()
        {
            RevertToOriginalValues();
            StackCommandTypes = ApplyNextStrategy(StackCommandTypes,NextStrategy++);
        }

        public bool IsRobotLocationOutOfRange(Robot robot)
        {
            var x = robot.Terrain.GetLength(0);
            var y = robot.Terrain.GetLength(1);

            return robot.Position.Location.X < 0 ||
                   robot.Position.Location.X == x ||
                   robot.Position.Location.Y < 0 ||
                   robot.Position.Location.Y == y;

        }

        public bool IsRobotLocationObstacle(Robot robot)
        {
            return robot.Terrain[robot.Position.Location.X, robot.Position.Location.Y] == ElementType.Obs;
        }

        public bool IsRobotLocationVisitedCell(Robot robot)
        {
            return robot.VisitedCells.Any(vc => vc.X == robot.Position.Location.X
                                                && vc.Y == robot.Position.Location.Y);
        }

        public class RobotMemento
        {
            public Position Position { get; set; }
            public int Battery { get; set; }
            public int NextStrategy { get; set; }
            public FacingControl FacingControl { get; set; }
            public MovingControl MovingControl { get; set; }
            public string[] Strategies;
            public List<Location> VisitedCells { get; set; }
            public List<ElementType> SamplesCollected { get; set; }
            public ElementType[,] Terrain { get; set; }
            public Stack<CommandType> StackCommandTypes { get; set; }

            public List<Result> SolutionStepByStep { get; set; }

            public RobotMemento(Position position, int battery, string[] strategies,
                FacingControl facingControl, MovingControl movingControl)
            {
                Position = position;
                Battery = battery;
                NextStrategy = 0;
                FacingControl = facingControl;
                MovingControl = movingControl;
                Strategies = strategies;
                StackCommandTypes = new Stack<CommandType>();
                VisitedCells = new List<Location>();
                VisitedCells.Add(Position.Location);
                SamplesCollected = new List<ElementType>();
                SolutionStepByStep = new List<Result>();
            }
        }
    }
}

    