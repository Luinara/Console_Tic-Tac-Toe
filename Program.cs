using System;

namespace Tic_Tac_Toe
{
    class Program {

        static void Main(string[] args) {

            Field field = new Field();
            String command;

            Console.WriteLine("Hello, Welcome to this game of PvP Tic-Tac-Toe.");
            Console.WriteLine("Do you want to play? \nYes: type \"start\" / No: type \"exit\"");

            while (true)
            {
                command = handleInput("command");
                if (command == "exit")
                {
                    break;
                }
                else
                {
                    field.printField();
                    Console.WriteLine("It is the turn of Player X");
                    Console.WriteLine("Please enter the coordinates.The first number is the line number, the second the column number.");
                    Console.WriteLine();
                    while (field.getState().Equals(Field.State.UNFINISHED))
                    {
                        String coords = handleInput("coordinates");
                        coords = coords.Trim();
                        String[] cord = coords.Split(' ');
                        field.makeMove(int.Parse(cord[0]) - 1, Int32.Parse(cord[1]) - 1);
                        field.printField();
                        field.determineState();
                    }
                    switch (field.getState())
                    {
                        case Field.State.X_WINS:
                            Console.WriteLine("X wins this game");
                            break;
                        case Field.State.O_WINS:
                            Console.WriteLine("O wins this game");
                            break;
                        case Field.State.DRAW:
                            Console.WriteLine("The game ended in a Draw");
                            break;
                        case Field.State.IMPOSSIBLE:
                            Console.WriteLine("I guess there is a bug I didn't fix");
                            break;
                    }
                    Console.WriteLine("Please enter a command:\n\"start\" / \"exit\"");
                    field.clearBoard();
                }

            }
        }

        static string handleInput(String inputtype)
        {
            String input = Console.ReadLine();
            System.Text.RegularExpressions.Regex coordinates = new System.Text.RegularExpressions.Regex(@"\s*[1-3]\s[1-3]\s*");
            System.Text.RegularExpressions.Regex command = new System.Text.RegularExpressions.Regex(@"(start)|(exit)");
            switch (inputtype)
            {
                case "coordinates":
                    while (true)
                    {
                        if (coordinates.IsMatch(input))
                        {
                            return input;                            
                        }
                        Console.WriteLine("Pleas input the coordinates. The first number is the line number, the second the column number.");
                        input = Console.ReadLine();
                    }
                    
                case "command":
                    while (true)
                    {
                        if (command.IsMatch(input))
                        {
                            return input;
                        }
                        Console.WriteLine("Pleas input a command. Acepted comands are \"start\" and \"exit\".");
                        input = Console.ReadLine();
                    }
                default:
                    return input;
            }
            
        }
    }


    class Field
    {
        static int fieldsize = 3;
        static char[,] field = new char[fieldsize, fieldsize];
        private State state = State.UNFINISHED;
        private char turn = 'X'; 

        public enum State
        {
            X_WINS, O_WINS, DRAW, UNFINISHED, IMPOSSIBLE
        }

        public Field ()
        {
            clearBoard();
        }

        public char getTurn()
        {
            return turn;
        }

        public State getState()
        {
            return state;
        }

        public char nextTurn()
        {
            if (turn == 'X')
            {
                turn = 'O';
            }
            else
            {
                turn = 'X';
            }
            Console.WriteLine("It is now Turn of " + turn);
            return turn;
        }

        public void makeMove(int i, int j)
        {
            if (field[i,j] == '_')
            {
                field[i, j] = turn;
                nextTurn();
            }
            else
            {
                Console.WriteLine("This field is occupied. Please choose another one." );
            }
        }

        public void clearBoard()
        {
            for(int i = 0; i < fieldsize; i++)
            {
                for(int j = 0; j < fieldsize; j++)
                {
                    field[i, j] = '_';
                }
            }
            state = State.UNFINISHED;
            turn = 'X';
        }

        public void determineState()
        {
            bool b_ = false;
            bool b3X;
            bool b3O;
            bool X_O = false;


            // checks for empty cells
            for (int k = 0; k < fieldsize; k++)
            {
                for (int l = 0; l < fieldsize; l++)
                {
                    if (field[k,l] == '_')
                    {
                        b_ = true;
                        break;
                    }
                }
                if (b_)
                {
                    break;
                }
            }

            // Checks for 3 X
            b3X = threeInARow(field, 'X');

            // Checks for 3 O
            b3O = threeInARow(field, 'O');
            int j;

            //counts Xs and Os
            int Os = 0;
            int Xs = 0;
            for (int i = 0; i < fieldsize; i++)
            {
                for (j = 0; j < fieldsize; j++)
                {
                    if (field[i,j] == 'O')
                    {
                        Os++;
                    }
                    else if (field[i,j] == 'X')
                    {
                        Xs++;
                    }
                }
            }

            //checks for an impossible number of Xs and Os
            if (Xs > Os + 1 || Os > Xs + 1)
            {
                X_O = true;
            }

            if (X_O)
            {
                state = State.IMPOSSIBLE;         
            }
            else if (b3X)
            {
                if (b3O)
                {
                    state = State.IMPOSSIBLE;
                }
                else
                {
                    state = State.X_WINS;                   
                }
            }
            else if (b3O)
            {
                state = State.O_WINS;                
            }
            else if (b_)
            {
                state = State.UNFINISHED;               
            }
            else
            {
                state = State.DRAW;              
            }
        }

        private static bool threeInARow(char[,] arr, char o)
        {
            int j = 0;
            bool b3O = false;
            for (int i = 0; i < fieldsize; i++)
            {
                if ((arr[i,j] == o && arr[i,j + 1] == o && arr[i,j + 2] == o) ||
                        ((arr[j,i] == o && arr[j + 1,i] == o && arr[j + 2,i] == o)))
                {
                    b3O = true;
                    break;
                }
            }
            if ((arr[0,0] == o && arr[1,1] == o && arr[2,2] == o) ||
                    ((arr[0,2] == o && arr[1,1] == o && arr[2,0] == o)))
            {
                b3O = true;
            }
            return b3O;
        }

        public void printField()
        {
            Console.WriteLine();
            Console.WriteLine("    1 2 3 ");
            Console.WriteLine("  _________  ");
            for (int i = 0; i < fieldsize; i++)
            {
                Console.Write(i+1 + " | ");
                for (int j = 0; j < fieldsize; j++)
                {
                    Console.Write(field[i, j] + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("  |_______|  ");
            Console.WriteLine();
        }
    }

   
}
