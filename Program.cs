
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingGeneriComplexObjects
{
    class Program
    {
        static void Main(string[] args)
        {
            var students = new List<Student> { 
                new Student
                {
                    Id = 1,
                    Name ="James Dean",
                    Scores = new TestScore{ Math=10 , Reading =32},                   
                },
                new Student
                {
                    Id = 2,
                    Name ="Angie Yang",
                    Scores = new TestScore{ Math=65 , Reading =82},
                },
                new Student
                {
                    Id = 3,
                    Name ="Zara Mane",
                    Scores = new TestScore{ Math=40 , Reading =28},
                },
                new Student
                {
                    Id = 4,
                    Name ="Carl Louis",
                    Scores = new TestScore{ Math=24 , Reading =97},
                },
            };
            printList(students);
            
            Console.WriteLine();
            
           // students = students.AsQueryable().SortByField("Name",SortDirection.DESC).ToList();
            //printList(students);
            
            Console.WriteLine();
            
            //normal Linq
            //var t = students.OrderBy(p => p.Scores.Math);
            //students = t.ToList();
            //printList(students);

            Console.WriteLine();
            var tx = students.AsQueryable().SortByField("Scores.Reading",SortDirection.DESC);
            students = tx.ToList();
            printList(students);

            Console.ReadLine();

        }


        public static void printList(List<Student> students)
        {
            foreach(var s in students)
            {
                var line = $"{s.Id } {s.Name } {s.Scores.Math } {s.Scores.Reading }";
                Console.WriteLine(line);
            }
        }
    }
}
