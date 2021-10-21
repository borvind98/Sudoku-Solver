using System;

namespace Sudoku_Solver
{
    class Program
    {
        static int[][][] puzzleBoard = new int[9][][];
        static void Main(string[] args)
        {
            for(int i = 0; i < 9; i++){
                puzzleBoard[i] = new int[9][];
                for(int j = 0; j < 9; j++){   
                    puzzleBoard[i][j] = new int[9];
                }
            }


            for (int i = 0; i < 9; i++){
                for (int j = 0; j < 9; j++){
                    for (int k = 0; k < 9; k++){
                        puzzleBoard[i][j][k] = k+1;
                    }
                }
            }
            
            //sudoku eksempel
            setTileTo(0,1,4);
            setTileTo(0, 4, 8);

            setTileTo(1,1,7);
            setTileTo(1,6,3);

            setTileTo(2,5,6);
            setTileTo(2,8,5);

            setTileTo(3,3,7);
            setTileTo(3,4,1);
            setTileTo(3,7,9);

            setTileTo(4,0,6);
            setTileTo(4,8,8);

            setTileTo(6,0,8);
            setTileTo(6,5,5);

            setTileTo(7,3,3);
            setTileTo(7,6,9);

            setTileTo(8,3,9);
            setTileTo(8,7,1);

            printResult();
            printPuzzle();

            for (int i = 0; i < 500; i++) {
                print();
            }

        }

        static void solvePuzzle(){
            bool solved = false;
            //while(!solved){
                for (int y = 0; y < 9; y++){
                    for (int x = 0; x < 9; x++){
                        if(checkTile(y, x) == 10){
                            checkThisTile(y, x);
                        }
                        int number = checkTile(y, x);
                        if(number < 10){
                            updatePuzzle(y, x, number);
                        }
                    }
                }
                doubleCheckAllTiles();
                solved = isSolved();
            //}
        }

        static void setTileTo(int y, int x, int number){
            //setter alle tallene i [k] lik 0, utenom tallet som ruten settes lik
            for (int i = 0; i < 9; i++){
                if(i != number-1){
                    puzzleBoard[y][x][i] = 0;
                }
                else{
                    puzzleBoard[y][x][i] = number;
                }
            }
            updatePuzzle(y, x, number);
        }

        static void updatePuzzle(int y, int x, int number){
            for (int b = 0; b < 9; b++){
                if(b != y){
                    int n = checkTile(b, x);
                    if(n == 10){
                        puzzleBoard[b][x][number-1] = 0;
                    }
                }
            }
            for (int a = 0; a < 9; a++){
                if(a != x){
                    int n = checkTile(y, a);
                    if (n == 10){
                        puzzleBoard[y][a][number-1] = 0;
                    }
                }
            }
            updateBox(y, x, number);
        }

        static bool isSolved(){
            for (int i = 0; i < 9; i++){
                for (int j = 0; j < 9; j++){
                    int number = checkTile(i, j);
                    if(number == 10){
                        return false;
                    }
                }
            }
            return true;
        }

        static int[] clean(){
            int[] potential = new int[9];
            for(int i = 0; i < 9; i++){
                potential[i] = 0;
            }
            return potential;
        }

        static void checkThisTile(int y, int x){
            //sjekker rader og kolonner ut fra en rute
            for(int b = 0; b < 9; b++){
                if(b != y){
                    int number = checkTile(b, x);
                    if(number < 9){
                        puzzleBoard[y][x][number-1] = 0;
                    }
                }
            }
            for(int a = 0; a < 9; a++){
                int number = checkTile(y, a);
                if(number < 9) {
                    puzzleBoard[y][x][number-1] = 0;
                }
            }
            checkBox(y, x);
        }

        static void doubleCheckAllTiles(){
            if(doubleCheckAllRows()){
                return;
            }
            if(doubleCheckAllColums()){
                return;
            }
            if(doubleCheckAllBoxes()){
                return;
            }
        }

        static bool doubleCheckAllRows(){
            int[] potential;
            //går gjennom alle rader og sjekker for unike potensielle tall
            for(int y = 0; y < 9; y++){
                potential = clean();
                for (int x = 0; x < 9; x++){
                    int number = checkTile(y, x);
                    if(number == 10){
                        for(int n = 0; n < 9; n++){
                            int pot = puzzleBoard[y][x][n];
                            if(pot != 0){
                                potential[n]++;
                            }
                        }
                    }
                }
                //sjekker hvilke ruter i en rad som har unike potensielle tall
                for (int n = 0; n < 9; n++) {
                    int counted = potential[n];
                    if (counted == 1){
                        Console.WriteLine("RAD " +(y+1) + " HAR UNIK: " + (n+1));
                        for (int x = 0; x < 9; x++) {
                            int number = puzzleBoard[y][x][n];
                            if (number != 0){
                                setTileTo(y,x, number);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        static bool doubleCheckAllColums(){
            int[] potential;
            //går gjennom alle kolonner og sjekker for unike tall
            for (int x = 0; x < 9; x++) {
                potential = clean();
                for (int y = 0; y < 9; y++) {
                    int num = checkTile(y, x);
                    if(num == 10){
                        for (int n = 0; n < 9; n++) {
                            int p = puzzleBoard[y][x][n];
                            if (p != 0) {
                                potential[n]++;
                            }
                        }
                    }
                }
                //sjekker hvilke ruter i en kolonne som har unike tall
                for (int n = 0; n < 9; n++) {
                    int counted = potential[n];
                    if (counted == 1){
                        Console.WriteLine("KOLONNE " + (x+1) +  " HAR UNIK: " +  (n+1));
                        for (int y = 0; y < 9; y++) {
                            int num = puzzleBoard[y][x][n];
                            if (num != 0){
                                setTileTo(y,x, num);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        static bool doubleCheckAllBoxes(){
            int[] potential;
            for (int y = 0; y < 9; y+=3) {
                for (int x = 0; x < 9; x+=3) {
                    potential = clean();
                    int b = (int) Math.Floor((double)y/3);
                    int a = (int) Math.Floor((double)x/3);

                    //finne midterste rute av 3*3 boksen til angitt rute
                    int yb = b*3+1;
                    int xa = a*3+1;

                    //scanne 3*3 boksen til ruta

                    for (int y2 = (yb-1); y2 < (yb+2); y2++) {
                        for (int x2 = (xa-1); x2 < (xa+2); x2++) {
                            int num = checkTile(y2, x2);
                            if(num == 10){
                                for (int n = 0; n < 9; n++) {
                                    int p = puzzleBoard[y2][x2][n];
                                    if (p != 0) {
                                        potential[n]++;
                                    }
                                }
                            }
                        }
                    }
                    for (int n = 0; n < 9; n++) {
                        int counted = potential[n];
                        if (counted == 1){
                            Console.WriteLine("BOKS " +(b*3+a+1)+" HAR UNIK: " + (n+1));
                            for (int y2 = (yb-1); y2 < (yb+2); y2++) {
                                for (int x2 = (xa-1); x2 < (xa+2); x2++) {
                                    int num = puzzleBoard[y2][x2][n];
                                    if (num != 0){
                                        setTileTo(y2,x2, num);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        static void updateBox(int y, int x, int num){
            //finne ut hvilken 3*3 boks som (x,y) ruta tilhører
            int b = y/3;
            int a = x/3;

            //finne midterste rute av 3*3 boksen til angitt rute
            int yb = b*3+1;
            int xa = a*3+1;

            //scanne 3*3 boksen til ruta

            for (int j = (yb-1); j < (yb+2); j++) {
                for (int i = (xa-1); i < (xa+2); i++) {
                    if(j != y && i != x){
                        int n = checkTile(j, i);
                        if(n == 10){
                            puzzleBoard[j][i][num-1] = 0;
                        }
                    }
                }
            }
        }

        static void checkBox(int y, int x){
            //finne ut hvilken 3*3 boks som (x,y) ruta tilhører
            int b = y/3;
            int a = x/3;

            //finne midterste rute av 3*3 boksen til angitt rute
            int yb = b*3+1;
            int xa = a*3+1;

            //scanne 3*3 boksen til ruta

            for (int j = (yb-1); j < (yb+2); j++) {
                for (int i = (xa-1); i < (xa+2); i++) {
                    if(j != y && i != x){
                        int num = checkTile(j, i);
                        if(num < 9){
                            puzzleBoard[y][x][num-1] = 0;
                        }   
                    }
                }
            }
        }

        //sjekker om en tile er fylt eller ikke
        static int checkTile(int y, int x){
            int counter = 0;
            int number = 10;
            int[] tile = puzzleBoard[y][x];
                foreach (int i in tile) {
                    if(i == 0){
                        counter++;
                    }
                    else{
                        number = i;
                    }
                }
                //returnerer 10 om cellen er tom
                if(counter < 8){
                    number = 10;
                }
                if(counter == 9){
                    number = 11;
                }
                //returnerer tallet til cellen
                return number;
        }

        //printer ut løst sudoku
        static void printResult(){
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 9; x++) {
                    Console.Write(y + "," + x +": ");
                    for (int n = 0; n < 9; n++) {
                        int num = puzzleBoard[y][x][n];
                        if(num != 0){
                            Console.Write(num + ",");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        //printer en sudoku i form av matrise
        static void printPuzzle(){
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 9; x++) {
                    int num = checkTile(y, x);
                    if(num == 10){
                        Console.Write("  ");
                    }
                    else{
                        Console.Write(num + " ");
                    }

                }
                Console.WriteLine();
                //System.out.println("-------------------------------------");
            }
        }

        static void print(){
            solvePuzzle();
            Console.WriteLine();
            printResult();
            Console.WriteLine();
            printPuzzle();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}


