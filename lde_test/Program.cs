using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using lde_test.Infrastructure;
using Newtonsoft.Json.Linq;

namespace lde_test
{
    class Program
    {
        static void Main(string[] args)
        {
            var robot = CreateRobotFromInputJsonData(args);
            robot.Execute();
            JsonManager.WriteToJsonFile(robot.SolutionStepByStep.LastOrDefault(),args[1]);
        }

        private static Robot CreateRobotFromInputJsonData(string[] args)
        {
            var marsSurveillanceRobotInput = JsonManager.LoadInputJson(args[0]);
            var terrain = JObject.Parse(marsSurveillanceRobotInput.ToString())["terrain"];
            var battery = JObject.Parse(marsSurveillanceRobotInput.ToString())["battery"];
            var commands = JObject.Parse(marsSurveillanceRobotInput.ToString())["commands"];
            var initialPosition = JObject.Parse(marsSurveillanceRobotInput.ToString())["initialPosition"];

            var robotStrategies = new[]
            {"E,R,F", "E,L,F", "E,L,L,F", "E,B,R,F", "E,B,B,L,F", "E,F,F", "E,F,L,F,L,F"};

            var facingControl = new FacingControl();
            var movingControl = new MovingControl();

            // Creamos um robot con los datos de entrada del fichero json,
            // y le añadimos un direccionamiento (facingControl) y un Navegador (navigationControl)

            var terrainElementTypes = TransformTerrainToTerrainElementTypes(terrain);
            var stackCommands = TransformCommands(commands);

            var robot = lde_test.Robot.Create(
                initialPosition, battery, robotStrategies, facingControl, movingControl,
                terrainElementTypes, stackCommands);      

            return robot;
        }

        private static Stack<CommandType> TransformCommands(JToken commands)
        {
            var commandDictionary = new Dictionary<string, CommandType>
        {
            {"S", CommandType.TakeSample},
            {"F", CommandType.MoveForward},
            {"B", CommandType.MoveBackwards },
            {"L", CommandType.TurnLeft },
            {"R", CommandType.TurnRight},
            {"E", CommandType.ExtendSolarPanels }
        };


            var stackCommands = new Stack<CommandType>();
            for (int i = commands.Count() - 1; i >= 0; i--)
            {
                var command = commandDictionary[commands[i].ToString()];
                stackCommands.Push(command);
            }

            return stackCommands;
        }

        private static ElementType[,] TransformTerrainToTerrainElementTypes(JToken terrain)
        {
            int imax = terrain[0].Count();
            int jmax = terrain.Count();

            ElementType[,] terrainElementTypes = new ElementType[imax, jmax];


            for (int i = 0; i < imax; i++)
            {
                for (int j = 0; j < jmax; j++)
                {
                    var itemObject = terrain[i][j];
                    var element = (ElementType) Enum.Parse(typeof (ElementType), itemObject.ToString());
                    terrainElementTypes[j, i] = element;
                }
            }
            return terrainElementTypes;
        }
    }
}
