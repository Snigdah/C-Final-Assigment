using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApplication
{
    [Serializable]
    public class students
    {
        public int ID { get; set; }
        public string fName { get; set; }
        public string mName { get; set; }
        public string lName { get; set; }
        public string Dept { get; set; }
        public string Degree { get; set; }
        public _Semister[] ComplitedSemister { get; set; }
        public string SemesterCode { get; set; }
        public string Year { get; set; }
    }

    public class _Semister
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Instructor { get; set; }
        public int Credit { get; set; }
    }
}
