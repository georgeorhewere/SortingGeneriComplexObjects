using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingGeneriComplexObjects
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestScore Scores { get; set; }        

    }

    public class TestScore
    {
        public int Math { get; set; }
        public int Reading { get; set; }

    }
}
