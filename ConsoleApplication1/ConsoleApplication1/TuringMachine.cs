using System;
using System.Collections.Generic;
using System.Text;

namespace TuringSim
{
    public class TuringMachine
    {
        private int _pointerLocation = 0;
        private StringBuilder _testString = new StringBuilder("");
        private string _currentState;

        private char CurrentChar
        {
            get
            {
                return _testString[_pointerLocation];
            }
            set { _testString[_pointerLocation] = value; }
        } 
        private readonly Dictionary<string, string[]> _transactionFunction = new Dictionary<string, string[]>();

        private void InputTestString()
        {
            // read in the test string
            Console.WriteLine("input the string you want to test");
            _testString.Append(Console.ReadLine());
        }

        private void InputStartState()
        {
            // read in the start state name
            Console.WriteLine("Please input the name of the start state, defalut is 'q0'.");
            _currentState = Console.ReadLine() == "" ? "q0" : Console.ReadLine();
        }

        private void ParseTransactionInputLine(string inputLine)
        {
            // convert the string into Transaction Function
            var dictList = inputLine.Split(':');
            if (dictList.Length != 2)
            {
                Console.WriteLine("the input is not valid, make sure you have exactly one ':' in the string");
                return;
            }
            var dictKey = dictList[0];
            var dictValue = dictList[1].Split(',');
            if (dictValue.Length != 3)
            {
                Console.WriteLine("input is not valid, make sure you have exactly 2 commas after ':'");
                return;
            }

            _transactionFunction.Add(dictKey, dictValue);
        }

        private void InputTransactionFunction()
        {
            Console.WriteLine("Please input all the transactions in the following form");
            Console.WriteLine("<original state>,<read character>:<go to state>,<write character>,<movement(R or L)>");
            Console.WriteLine("Please leave extra white space");
            var continues = true;
            do
            {
                var inputString = Console.ReadLine();
                // remove all the white space

                if (!string.IsNullOrEmpty(inputString))
                {
                    ParseTransactionInputLine(inputString);
                }
                else
                {
                    Console.WriteLine("input end");
                    continues = false;
                }
            } while (continues);
        }

        public void Input()
        {
            InputTestString();
            InputStartState();
            InputTransactionFunction();
        }

        private void ExtendRight()
        {
            _testString = _testString.Append(" ");
        }

        private void ExtendLeft()
        {
            _testString = _testString.Insert(0, " ");
            _pointerLocation += 1;
        }

        private bool MoveRightValid()
        {
            return _pointerLocation < _testString.Length-1;
        }

        private bool MoveLeftValid()
        {
            return _pointerLocation > 0;
        }

        private void MoveRight()
        {
            if (MoveRightValid())
            {
                _pointerLocation += 1;
            }
            else
            {
                ExtendRight();
                _pointerLocation += 1;
            }
        }

        private void MoveLeft()
        {
            if (MoveLeftValid())
            {
                _pointerLocation -= 1;
            }
            else
            {
                ExtendLeft();
                _pointerLocation -= 1;
            }
        }

        private bool MoveOnce()
        {
            Console.WriteLine("the current state is {0}", _currentState);
            Console.WriteLine("the current char is {0}", CurrentChar);
            var current = _currentState + "," + CurrentChar;
            bool success;
            string[] result = {"", ""};
            try
            {
                result = _transactionFunction[current];
                success = true;
            }
            catch (KeyNotFoundException)
            {
                success = false;
            }
            if (success)
            {
                var goToState = result[0];
                var writeChar = result[1].ToCharArray()[0];
                var moveDirection = result[2];

                // go to state
                _currentState = goToState;
                Console.WriteLine("go to state {0}", _currentState);
                // write char
                CurrentChar = writeChar;
                Console.WriteLine("write '{0}' on the current char", writeChar);
                // move 
                switch (moveDirection)
                {
                    case "L":
                        MoveLeft();
                        Console.WriteLine("move to the right");
                        break;
                    case "R":
                        MoveRight();
                        Console.WriteLine("move to the right");
                        break;
                    default:
                        Console.WriteLine("the move direction here in the transaction function is not valid");
                        Console.WriteLine("Here is the transaction fucntion");
                        Console.WriteLine("({0}, {1}):({2}, {3}, {4})", _currentState, CurrentChar, goToState, writeChar,
                            moveDirection);
                        break;
                }
            }
            Console.WriteLine("");
            return success;
        }

        private void DisplayGraphStatus()
        {
            var pointerString = "";
            for (int i = 0; i < _pointerLocation; i++)
            {
                pointerString = pointerString + " ";
            }
            pointerString = pointerString + "|";
            Console.WriteLine(pointerString);
            Console.WriteLine(_testString);
        }

        public void Run()
        {
            var success = true;
            while (success)
            {
                DisplayGraphStatus();
                success = MoveOnce();
            }
            Console.WriteLine("the machine halts");
        }
    }
}