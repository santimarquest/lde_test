Introduction

The exercise consists on the development of a small sample application that will illustrate your skills on the different areas involved in coding and coding good practices and patterns.
Please do read the instructions carefully before beginning coding and do not hesitate to reach your point of contact in case you have any doubts or questions.
What you should have.
The coding test includes the following files:
- Instructions.pdf: This document containing the instructions for the test.
- test_run_1.json: A sample json input for testing.
- test_sol_1.json: The expected json output for testing.
- test_run_2.json: An additional json input file for testing.
- test_sol_2.json: The expected json output for the additional json.
Note the included tests are just examples and not representative of comprehensive testing.
What you should produce and how long you’ll have.
You’ll have a full week (7 days) to complete the assignment once you receive it.
The test should take from 2 to 6 hours to develop, the week is provided so that you can adapt the completion of the test to your schedule and timetable as you like. You’re of course free to dedicate as much extra time as you want to it. Time of development will not be measured, only quality of code and good practices.
The result of the test should consist on:
- A .NET solution targeting either .NET 4.5+, .NET Standard (any version) or .NET Core (any version), including all the code required to build and execute the solution.
- It’s recommended to use Visual Studio 2017 CE or Visual Studio for Mac in order to develop the solution but any working solution will be accepted.
- The solution should produce a console application named “lde_test.exe” which takes a single input path as the input and a single output path as the output. Therefore, the solution should be either:
o lde_test.exe c:\mypath\jsonInput.json c:\output\jsonOutput.json
o dotnet lde_test.dll c:\mypath\jsonInput.json c:\output\jsonOutput.json
Please focus on producing production ready code, that is, code that does not only work but would be what you consider to be ready to be put into production.
We will value all standard good coding practices and patterns, a good design of the solution, etc. Extra characteristics like using source control, containers, exposing an API in addition to the console program are extra points but not required for the solution.
The problem
lde has recently developed a Mars Surveillance Robot with the intent of exploring the surface on Mars. Once the robot has landed on Mars, the robot will enter in listening mode for instructions which will be provided as a set of commands to execute which may include moving forward (F) or backwards (B), turning (L or R), taking samples (S), extending the solar panels for getting energy (E).
At every moment, the Robot contains an internal battery which provides a certain capacity. Each of the aforementioned commands will consume a given quantity of the battery.
- Move Forward (F):
o Consumes 3 battery units.
o Move the unit one square forward in the current facing direction
- Move Backwards (B):
o Consumes 3 battery units.
o Move the unit one square backwards from the current facing direction
- Turn Left (L):
o Consumes 2 battery units.
o Changes the facing direction 90º to the right.
- Turn Right (R):
o Consumes 2 battery units.
o Changes the facing direction 90º to the left.
- Take Sample (S):
o Consumes 8 battery units.
o Takes and stores a sample of whatever material is primary in the current location.
- Extend solar panels (E):
o Consumes 1 battery unit
o Recharges 10 battery units.
To better test the behavior of the robot, a functionality is required which will accept a set of commands, a starting position, a starting battery and a map of the surface composed as a nxm matrix of coordinates which will indicate the characteristics of the terrain in mars.
The terrain described on the map can contain the following:
- Fe: Ferrum. A deposit of iron.
- Se: Selenium. A deposit of selenium.
- W: Water. A deposit that contains water.
- Si: Silicon. A deposit that contains silicon.
- Zn: Zinc. A deposit that contains zinc.
- Obs: An obstacle cell in which the robot can’t go.
Whenever the robot detects an obstacle ahead of the execution of the command, it must automatically apply a backoff strategy instead in order to continue with the execution. The robot contains a list of backoff strategies to try in order, if the execution of 1 strategy results in hitting another obstacle, the robot will jump to the next strategy (battery will nonetheless be consumed):
1. E, R, F
2. E, L, F
3. E, L, L, F
4. E, B, R, F
5. E, B, B, L, F
6. E, F, F
7. E, F, L, F, L, F
The robot will then produce a simulation run of the results obtained containing:
- The set of cells visited.
- The set of samples collected.
- The current battery level.
Examples
Example – Input
{
"terrain": [
["Fe", "Fe", "Se"],
["W", "Si", "Obs"],
["W", "Obs", "Zn"]
],
"battery": 50,
"commands": [
"F", "S", "R", "F", "S", "R", "F", "L", "F", "S"
],
"initialPosition": {
"location" : {
"x" : 0,
"y" : 0
},
"facing" : "East"
}
}
The terrain represents a multidimensional array describing the resources on the terrain.
The array represents the terrain in a direct way, with 0 based indexes, that is, in the example above, for X = 1 and Y = 0 we have “Fe” and for X = 0 and Y = 1 we have “W”.
Battery represents the starting battery for the robot.
Commands represents the set of commands to execute.
InitialPosition represents the initial position of the robot. Location (X, Y) represents the starting position of the robot in the 0 based array as described above.
Facing represents the initial facing direction of the Robot, there are 4 possible valid values:
- North,
- East
- South
- West
Example – Output
Running the Example above will produce the following output:
{
"VisitedCells": [
{ "X": 0, "Y": 0 },
{ "X": 1, "Y": 0 },
{ "X": 1, "Y": 1 },
{ "X": 0, "Y": 1}
],
"SamplesCollected": ["Fe", "Fe", "Si", "W"],
"Battery": 14,
"FinalPosition": {
"Location": { "X": 0, "Y": 1
},
"Facing": "West"
}
}
VisitedCells fields contains all the fields that the robot has traversed on its execution without any repetitions.
SamplesCollected contains all the fields that the robot has taken samples of, including duplicates, that is, If the robot samples Water twice then it should appear twice.
Battery contains the remaining battery of the unit after the execution
FinalPosition contains the final position of the robot after the execution.