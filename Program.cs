using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AlgFundEx2
{
    class Program
    {
        static void Main(string[] args)
        {
            P1();
            Console.ReadKey();
           
        }
        /// <summary>
        /// Se da o multime de nr. Afiseaza numerele care apartin sirului lui fibonacci in ordine descrescatoare
        /// </summary>
        private static void P1()
        {
            using(StreamReader sr = new StreamReader(@"..\..\TextFile1.txt"))
            {
                string[] data = sr.ReadLine().Split(' ');
                List<int> fibo = new List<int>();
                List<int> pos = new List<int>();
                fibo.Add(1);
                fibo.Add(1);
                pos.Add(0);
                pos.Add(0);
                foreach(string item in data)
                {
                    int n = int.Parse(item);
                    int p;
                    if (n <= fibo[fibo.Count - 1])
                    {
                        p = BinarySearch(fibo, n);
                        if(p != - 1)
                        {
                            pos[p]++;
                        }

                    }
                    else
                    {
                        int f;
                        do
                        {
                            f = fibo[fibo.Count - 1] + fibo[fibo.Count - 2];
                            fibo.Add(f);
                            pos.Add(0);
                        } while (f < n) ;
                        if (fibo[fibo.Count - 1] == n)
                            pos[fibo.Count - 1]++;

                    }
                } 
                for(int i = pos.Count - 1; i >= 0; i--)
                {
                    while(pos[i] > 0)
                    {
                        Console.Write(fibo[i] + " ");
                        pos[i]--;
                    }
                }
            }
        }
        private static int BinarySearch(List<int> l, int x)
        {
            int left = 0;
            int right = l.Count - 1;
            while(left <= right)
            {
                int middle = (left + right) / 2;
                if (x == l[middle])
                    return middle;
                else if (x < l[middle])
                    right = middle - 1;
                else if (x > l[middle])
                    left = middle + 1;
            }
            return -1;
        }
       /* private static bool IsFibonacci(int x)
        {
            return IsPerfectSquare(5 * x * x - 4) || IsPerfectSquare(5 * x * x + 4);
        }
        private static bool IsPerfectSquare(int x)
        {
            int s = (int)Math.Sqrt(x);
            return s * s == x;
        }*/
        /// <summary>
        /// Se da o multime de puncte in plan. Afiseaza aria tetraedrului minima
        /// </summary>
        private static void P3()
        {
            using(StreamReader sr = new StreamReader(@"..\..\data.txt"))
            {
                int pointsNr = int.Parse(sr.ReadLine());
                List<Point3D> l = new List<Point3D> ();
                while(!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    Point3D p = new Point3D();
                    p.X = double.Parse(data[0]);
                    p.Y = double.Parse(data[1]);
                    p.Z = double.Parse(data[2]);
                    l.Add(p);

                }
                double minArea = double.MaxValue;
                for(int i = 0; i < l.Count; i++)
                {
                    for(int j = i + 1; j < l.Count; j++)
                    {
                        for(int k = j + 1; k < l.Count; k++)
                        {
                            for(int m = k + 1; m < l.Count; m++)
                            {
                                double a = TetrahedronArea(l[i], l[j], l[k], l[m]);
                                Console.WriteLine(a);
                                if (a < minArea)
                                    minArea = a;
                            }
                        }
                    }
                }
                Console.WriteLine("aria minima: " + minArea);
            }

        }
        private static double TriangleArea(Point3D a, Point3D b, Point3D c)
        {
            return 0.5 * Math.Abs(a.X * b.Y - b.X * a.Y + a.X * c.Y - c.X * a.Y + b.X * c.Y - c.X * b.Y);
        }
        private static double TetrahedronArea(Point3D a, Point3D b, Point3D c, Point3D d)
        {
            return TriangleArea(a, b, c) +
                   TriangleArea(a, d, b) +
                   TriangleArea(a, d, c) +
                   TriangleArea(b, d, c);

        }
        /// <summary>
        /// Construieste o matrice de forma :
        /// 5 4 3 2 1 0
        /// 4 3 2 1 0 1
        /// 3 2 1 0 1 2
        /// 2 1 0 1 2 3
        /// 1 0 1 2 3 4
        /// 0 1 2 3 4 5
        /// </summary>
        private static void P4()
        {
            int n = int.Parse(Console.ReadLine());
            int[,] arr = new int[n, n];
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if (i + j <= n - 1)
                        arr[i, j] = n - i - j - 1;
                    else
                        arr[i, j] = i + j - n + 1;
                }
            }
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }


        }
        /// <summary>
        /// Se da o matrice de 2^n / 2^m. Reduce matricea pana se ajunge la un singur element
        /// reducerea matricei => o matrice 2^(n-1) / 2^(m-1), fiecare element devine media aritmetica a elementelor din jur
        /// 1 5 1 0 1 2 1 4  =>  2 1 2 2  =>  2 2 =>  2
        /// 4 0 2 1 4 1 3 1      2 1 2 0  
        /// 2 7 2 1 4 1 0 0
        /// 1 0 2 1 1 4 0 1
        /// </summary>
        private static void P2()
        {
            int n = int.Parse(Console.ReadLine());
            int m = int.Parse(Console.ReadLine());
            int[,] arr = new int[(int)Math.Pow(2, n), (int)Math.Pow(2, m)];
            Random rnd = new Random();
            for(int i = 0; i < arr.GetLength(0); i++)
            {
                for(int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = rnd.Next(10);
                    
                }
            }
            Console.WriteLine();
            Reduction(arr);
            
        }
        private static int[,] Reduction(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            int[,] res;
            if (arr.GetLength(0) == 1 && arr.GetLength(1) == 1)
                return arr;
            if(arr.GetLength(0) == 1 && arr.GetLength(1) == 2)
            {
                res = new int[1, 1];
                res[0, 0] = arr[0, 1];
                return Reduction(res);
            }
            if (arr.GetLength(0) == 2 && arr.GetLength(1) == 1)
            {
                res = new int[1, 1];
                res[0, 0] = arr[1, 0];
                return Reduction(res);
            }
            else
            {
                res = new int[arr.GetLength(0) / 2, arr.GetLength(1) / 2];
                for(int i = 0; i < arr.GetLength(0); i += 2)
                {
                    for (int j = 0; j < arr.GetLength(1); j += 2)
                    {
                        res[i / 2, j / 2] = (arr[i, j + 1] + arr[i + 1, j + 1] + arr[i + 1, j]) / 3;

                    }
                }
                return Reduction(res);
            }
        }
        
    }
}
