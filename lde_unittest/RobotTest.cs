using System.Collections.Generic;
using lde_test;
using NUnit.Framework;


namespace lde_unittest
{
    [TestFixture]
    public class RobotShould
    {
        const int x = 3;
        const int y = 3;

        private readonly string[] robotStrategies = {"E,R,F", "E,L,F", "E,L,L,F", "E,B,R,F", "E,B,B,L,F", "E,F,F", "E,F,L,F,L,F"};

        private static void Setup()
        {
           
        }

        public static IEnumerable<TestCaseData> TurnRightFacingsAndBattery
        {
            get
            {
                Setup();
                yield return new TestCaseData(Facing.North, 0, 0, 10, Facing.East, 0, 0, 8);
                yield return new TestCaseData(Facing.East, 0, 0, 8, Facing.South, 0, 0, 6);
                yield return new TestCaseData(Facing.South, 0, 0, 6, Facing.West, 0, 0, 4);
                yield return new TestCaseData(Facing.West, 0, 0, 4, Facing.North, 0, 0, 2);
                yield return new TestCaseData(Facing.North, 0, 0, 2, Facing.East, 0, 0, 0);
                yield return new TestCaseData(Facing.East, 0, 0, 0, Facing.East, 0, 0, 0);
            }
        }

        [TestCaseSource("TurnRightFacingsAndBattery")]
        public void Robot_TurnRightFromPosition_RobotSetToCorrespondingState(
            Facing initialFacing,
            int initial_location_X,
            int initial_location_Y, 
            int initialBattery,
            Facing estimatedFacing,
            int estimaded_location_X,
            int estimaded_location_Y,
            int estimatedBattery)
        {
            // Arrange
            var position = new Position(new Location(initial_location_X, initial_location_Y), initialFacing);
            var robot = new Robot(position, initialBattery, robotStrategies, 
                new FacingControl(), new MovingControl(),
                new ElementType[,] {}, new Stack<CommandType>());           

            // Act
            var command = new TurnRightCommand();
            command.ExecuteCommand(robot);

            // Assert
            Assert.AreEqual(robot.Position.Facing, estimatedFacing);
            Assert.AreEqual(robot.Battery, estimatedBattery);
            Assert.AreEqual(robot.Position.Location.X, initial_location_X);
            Assert.AreEqual(robot.Position.Location.Y, initial_location_Y);
        }

        public static IEnumerable<TestCaseData> TurnLeftFacingsAndBattery
        {
            get
            {
                Setup();
                yield return new TestCaseData(Facing.North, 0, 0, 10, Facing.West, 0, 0, 8);
                yield return new TestCaseData(Facing.West, 0, 0, 8, Facing.South, 0, 0, 6);
                yield return new TestCaseData(Facing.South, 0, 0, 6, Facing.East, 0, 0, 4);
                yield return new TestCaseData(Facing.East, 0, 0, 4, Facing.North, 0, 0, 2);
                yield return new TestCaseData(Facing.North, 0, 0, 2, Facing.West, 0, 0, 0);
                yield return new TestCaseData(Facing.West, 0, 0, 0, Facing.West, 0, 0, 0);
            }
        }

        [TestCaseSource("TurnLeftFacingsAndBattery")]
        public void Robot_TurnLeftFromPosition_RobotSetToCorrespondingState(
           Facing initialFacing,
           int initial_location_X,
           int initial_location_Y,
           int initialBattery,
           Facing estimatedFacing,
           int estimaded_location_X,
           int estimaded_location_Y,
           int estimatedBattery)
        {
            // Arrange
            var position = new Position(new Location(initial_location_X, initial_location_Y), initialFacing);
            var robot = new Robot(position, initialBattery, robotStrategies,
                new FacingControl(), new MovingControl(),
                new ElementType[,] { }, new Stack<CommandType>());

            // Act
            var command = new TurnLeftCommand();
            command.ExecuteCommand(robot);

            // Assert
            Assert.AreEqual(robot.Position.Facing, estimatedFacing);
            Assert.AreEqual(robot.Battery, estimatedBattery);
            Assert.AreEqual(robot.Position.Location.X, initial_location_X);
            Assert.AreEqual(robot.Position.Location.Y, initial_location_Y);
        }

        public static IEnumerable<TestCaseData> TakeSample
        {
            get
            {
                Setup();
                yield return new TestCaseData(Facing.North, 0, 0, 10, Facing.North, 0, 0, 2, ElementType.Fe);
                yield return new TestCaseData(Facing.West, 0, 0, 8, Facing.West, 0, 0, 0, ElementType.W);
                yield return new TestCaseData(Facing.South, 0, 0, 8, Facing.South, 0, 0, 0, ElementType.Obs);

            }
        }

        [TestCaseSource("TakeSample")]
        public void Robot_TakeSampleFromPosition_RobotSetToCorrespondingState(
          Facing initialFacing,
          int initial_location_X,
          int initial_location_Y,
          int initialBattery,
          Facing estimatedFacing,
          int estimaded_location_X,
          int estimaded_location_Y,
          int estimatedBattery,
          ElementType sampleCollected)
        {
            // Arrange
            var position = new Position(new Location(initial_location_X, initial_location_Y), initialFacing);
            var robot = new Robot(position, initialBattery, robotStrategies,
                new FacingControl(), new MovingControl(),
                new[,] { {sampleCollected} }, new Stack<CommandType>());

           // Act
           var command = new TakeSampleCommand();
            command.ExecuteCommand(robot);

            // Assert
            Assert.AreEqual(robot.Position.Facing, estimatedFacing);
            Assert.AreEqual(robot.Battery, estimatedBattery);
            Assert.AreEqual(robot.Position.Location.X, estimaded_location_X);
            Assert.AreEqual(robot.Position.Location.Y, estimaded_location_Y);
            Assert.Contains(robot.Position.Location, robot.VisitedCells);
            if (sampleCollected != ElementType.Obs)
            {
                Assert.Contains(sampleCollected, robot.SamplesCollected);
            }
           

        }

        public static IEnumerable<TestCaseData> ExtendSolarPanel
        {
            get
            {
                Setup();
                yield return new TestCaseData(Facing.North, 0, 0, 10, Facing.North, 0, 0, 19);
                yield return new TestCaseData(Facing.West, 0, 0, 8, Facing.West, 0, 0, 17);
                yield return new TestCaseData(Facing.South, 0, 0, 6, Facing.South, 0, 0, 15);

            }
        }

        [TestCaseSource("ExtendSolarPanel")]
        public void Robot_ExtendSolarPanelFromPosition_RobotSetToCorrespondingState(
         Facing initialFacing,
         int initial_location_X,
         int initial_location_Y,
         int initialBattery,
         Facing estimatedFacing,
         int estimaded_location_X,
         int estimaded_location_Y,
         int estimatedBattery)
        {
            // Arrange
            var position = new Position(new Location(initial_location_X, initial_location_Y), initialFacing);
            var robot = new Robot(position, initialBattery, robotStrategies,
                new FacingControl(), new MovingControl(),
                new ElementType[,] { }, new Stack<CommandType>());

            // Act
            var command = new ExtendSolarPanelsCommand(new ConsumeBatteryCommand(), new ChargeBatteryCommand());
            command.ExecuteCommand(robot);

            // Assert
            Assert.AreEqual(robot.Position.Facing, estimatedFacing);
            Assert.AreEqual(robot.Battery, estimatedBattery);
            Assert.AreEqual(robot.Position.Location.X, initial_location_X);
            Assert.AreEqual(robot.Position.Location.Y, initial_location_Y);
            Assert.Contains(robot.Position.Location, robot.VisitedCells);

        }


        public static IEnumerable<TestCaseData> MoveForward
        {
            get
            {
                Setup();
                yield return new TestCaseData(Facing.North, 1, 1, 10, Facing.North, 1, 2, 7);
                yield return new TestCaseData(Facing.West, 1, 1, 8, Facing.West, 0, 1, 5);
                yield return new TestCaseData(Facing.South, 1, 1, 6, Facing.South, 1, 0, 3);
                yield return new TestCaseData(Facing.East, 1, 1, 4, Facing.East, 2, 1, 1);

                yield return new TestCaseData(Facing.West, 1, 1, 1, Facing.West, 1, 1, 1);
            }
        }

        [TestCaseSource("MoveForward")]
        public void Robot_MoveForwardFromPosition_RobotSetToCorrespondingState(
        Facing initialFacing,
        int initial_location_X,
        int initial_location_Y,
        int initialBattery,
        Facing estimatedFacing,
        int estimaded_location_X,
        int estimaded_location_Y,
        int estimatedBattery)
        {
            // Arrange
            var position = new Position(new Location(initial_location_X, initial_location_Y), initialFacing);
            var robot = new Robot(position, initialBattery, robotStrategies,
                new FacingControl(), new MovingControl(),
                new ElementType[x,y], new Stack<CommandType>());

            // Act
            var command = new MoveForwardCommand();
            command.ExecuteCommand(robot);

            // Assert
            Assert.AreEqual(robot.Position.Facing, estimatedFacing);
            Assert.AreEqual(robot.Battery, estimatedBattery);
            Assert.AreEqual(robot.Position.Location.X, estimaded_location_X);
            Assert.AreEqual(robot.Position.Location.Y, estimaded_location_Y);
            Assert.IsTrue(robot.VisitedCells.Contains(robot.Position.Location));

        }

        public static IEnumerable<TestCaseData> MoveBackwards
        {
            get
            {
                Setup();
                yield return new TestCaseData(Facing.North, 1, 1, 10, Facing.North, 1, 0, 7);
                yield return new TestCaseData(Facing.West, 1, 1, 8, Facing.West, 2, 1, 5);
                yield return new TestCaseData(Facing.South, 1, 1, 6, Facing.South, 1, 2, 3);
                yield return new TestCaseData(Facing.East, 1, 1, 4, Facing.East, 0, 1, 1);

                yield return new TestCaseData(Facing.West, 1, 1, 1, Facing.West, 1, 1, 1);
            }
        }

        [TestCaseSource("MoveBackwards")]
        public void Robot_MoveBackwardsFromPosition_RobotSetToCorrespondingState(
       Facing initialFacing,
       int initial_location_X,
       int initial_location_Y,
       int initialBattery,
       Facing estimatedFacing,
       int estimaded_location_X,
       int estimaded_location_Y,
       int estimatedBattery)
        {
            // Arrange
            var position = new Position(new Location(initial_location_X, initial_location_Y), initialFacing);
            var robot = new Robot(position, initialBattery, robotStrategies,
                new FacingControl(), new MovingControl(),
                new ElementType[x,y], new Stack<CommandType>());

            // Act
            var command = new MoveBackwardsCommand();
            command.ExecuteCommand(robot);

            // Assert
            Assert.AreEqual(robot.Position.Facing, estimatedFacing);
            Assert.AreEqual(robot.Battery, estimatedBattery);
            Assert.AreEqual(robot.Position.Location.X, estimaded_location_X);
            Assert.AreEqual(robot.Position.Location.Y, estimaded_location_Y);
            Assert.IsTrue(robot.VisitedCells.Contains(robot.Position.Location));

        }


        public static IEnumerable<TestCaseData> Strategies
        {
            get
            {
                Setup();
                yield return new TestCaseData(Facing.North, 0, 0, 10, 0);
                yield return new TestCaseData(Facing.North, 0, 0, 10, 1);
                yield return new TestCaseData(Facing.North, 0, 0, 10, 2);
                yield return new TestCaseData(Facing.North, 0, 0, 10, 3);
            }
        }

        [TestCaseSource("Strategies")]
        public void Robot_ApplyStrategy_RobotSetToCorrespondingState(
      Facing initialFacing,
      int initial_location_X,
      int initial_location_Y,
      int initialBattery,
      int strategy)
        {
            // Arrange

            var commandDictionary = new Dictionary<string, CommandType>
        {
            {"S", CommandType.TakeSample},
            {"F", CommandType.MoveForward},
            {"B", CommandType.MoveBackwards },
            {"L", CommandType.TurnLeft },
            {"R", CommandType.TurnRight},
            {"E", CommandType.ExtendSolarPanels }
        };

            var stackCommand = new Stack<CommandType>();
            stackCommand.Push(CommandType.ExtendSolarPanels);
            stackCommand.Push(CommandType.MoveForward);

            var position = new Position(new Location(initial_location_X, initial_location_Y), initialFacing);
            var robot = new Robot(position, initialBattery, robotStrategies,
                new FacingControl(), new MovingControl(),
                new ElementType[x, y], stackCommand);

            // Act
            var stackResult = robot.ApplyNextStrategy(stackCommand, strategy);
         ;

            // Assert
            Assert.AreEqual(stackResult.Count, stackCommand.Count);

            var array = robot.Strategies[strategy].Split(',');

           for (int i = 0; i < array.Length; i++)
           {              
               var command = array[i];
               Assert.AreEqual(stackResult.Pop(), commandDictionary[command]);
           }

            Assert.AreEqual(stackResult.Pop(), CommandType.MoveForward);
            Assert.AreEqual(stackResult.Pop(), CommandType.ExtendSolarPanels);
            Assert.AreEqual(stackResult.Count, 0);

        } 
    }
}
