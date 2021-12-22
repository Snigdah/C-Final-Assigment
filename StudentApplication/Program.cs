using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace StudentApplication
{
    class Program
    {
        static string fileName = @"sampledata.json";

        static void Main(string[] args)
        {
            StudentList();
            Console.WriteLine("\n1. Add Student");
            Console.WriteLine("2. View Student Details");
            Console.WriteLine("3. Delete Student");
            Console.WriteLine("4. Exit");

            while (true)
            {
                int choice = -1;
                do
                {
                    Console.Write("Enter  your Choice: ");

                    int x = input(1, 4);
                    choice = x;
                    Console.WriteLine();
                } while (choice < 1 || choice > 4);


                if (choice == 1)
                {
                    AddStudent();
                }
                else if (choice == 2)
                {
                    Console.Write("\nEnter Student ID: ");
                    int studentID = Convert.ToInt32(Console.ReadLine());
                    bool y = StudentDetails(studentID , true);

                    if (y == false)
                    {
                        Console.WriteLine("Student Id Doesn't Exists");
                    }

                }
                else if (choice == 3)
                {
                    Console.Write("\nEnter Student ID For Delete: ");
                    int studentID = Convert.ToInt32(Console.ReadLine());
                    bool y = DeleteStudent(studentID);

                    if (y == false)
                    {
                        Console.WriteLine("Student Id Doesn't Exists Or Problem Deleting Student\n");
                    }

                }
                else if (choice == 4)
                {
                    break;
                }
            }
        }


        static void StudentList()
        {
            var student = ExampleObject();
            if (student != null)
            {
                Console.WriteLine("List of Student");
                for (int i = 0; i < student.Count; i++)
                {
                    Console.WriteLine("   " + student[i].ID + ". " + student[i].fName + " " + student[i].mName + " " + student[i].lName);
                }
            }
        }


        static bool StudentDetails(int sID , bool pr)
        {
            bool found = false;

            var student = ExampleObject();
            if (student != null)
            {
                for (int i = 0; i < student.Count; i++)
                {
                    if (student[i].ID != sID)
                    {
                        continue;
                    }
                    found = true;

                    if (pr)
                    {
                        Console.WriteLine("ID: " + student[i].ID);
                        Console.WriteLine("Name: " + student[i].fName + " " + student[i].mName + " " + student[i].lName);
                        Console.WriteLine("Depertment: " + student[i].Dept);
                        Console.WriteLine("Degree: " + student[i].Degree);

                        for (int j = 0; j < student[i].ComplitedSemister.Length; j++)
                        {
                            Console.WriteLine("  Id: " + student[i].ComplitedSemister[j].Id);
                            Console.WriteLine("  Name: " + student[i].ComplitedSemister[j].Name);
                            Console.WriteLine("  Instructor: " + student[i].ComplitedSemister[j].Instructor);
                            Console.WriteLine("  Credit: " + student[i].ComplitedSemister[j].Credit);
                        }
                        Console.WriteLine("SemesterCode: " + student[i].SemesterCode);
                        Console.WriteLine("Year: " + student[i].Year);
                        Console.WriteLine("\n");
                    }
                }
            }

            return found;
        }


        static void AddStudent()
        {
            int ID;
            while (true)
            {
                Console.Write("     Enter ID : ");
                ID = input(0, 9999999);
                bool exists = StudentDetails(ID, false);
                if (exists)
                {
                    Console.WriteLine("     Id Already Exists! Enter New ID.");
                }
                else
                {
                    break;
                }
            }

            Console.Write("     Enter FIrst Name : ");
            string fName = Console.ReadLine();
            Console.Write("     Enter Middle Name : ");
            string mName = Console.ReadLine();
            Console.Write("     Enter Last Name : ");
            string lName = Console.ReadLine();
            Console.Write("     Enter Depertment Name : ");
            string Dept = Console.ReadLine();
            Console.Write("     Enter Degree : ");
            string Degree = Console.ReadLine();
            Console.Write("     Enter Course Id : ");
            int Id = input(0, 9999999); ;
            Console.Write("     Enter Course Name : ");
            string Name = Console.ReadLine();
            Console.Write("     Enter Instructor Name : ");
            string Instructor = Console.ReadLine();
            Console.Write("     Enter Number of Credits : ");
            int Credit = input(0, 9999999);
            Console.Write("     Enter Semester Code (Summer / Fall / Spring ) : ");
            string SemesterCode = Console.ReadLine();
            Console.Write("     Enter Date : ");
            string Year = Console.ReadLine();

            List<students> _data = new List<students>();
            _data.Add(new students()
            {
                ID = ID,
                fName = fName,
                mName = mName,
                lName = lName,
                Dept = Dept,
                Degree = Degree,

                ComplitedSemister = new _Semister[]{
                  new _Semister()
                  {
                      Id = Id,
                      Name = Name,
                      Instructor = Instructor,
                      Credit = Credit
                  }},

                SemesterCode = SemesterCode,
                Year = Year
            });

            string json = JsonConvert.SerializeObject(_data.ToArray());
            json = json.Replace("][", ",");
            System.IO.File.AppendAllText(fileName, json);


            json = File.ReadAllText(fileName);
            json = json.Replace("][", ",");

            for(int i = 0; i < json.Length; i++)
            {
                if (json[i] == ',')
                {
                    if(json[i+1]=='{' || json[i + 2] == '{')
                    {
                        if(json[i-1]!='}' && json[i - 2] != '}')
                        {
                            json = json.Substring(0, i)+json.Substring(i+1, json.Length);
                        }
                    }
                }
            }

            System.IO.File.WriteAllText(fileName, json);
        }


        static bool DeleteStudent(int Deleted)
        {
            var json = File.ReadAllText(fileName);
            var jArray = JArray.Parse(json);

            try
            {
                var itemToRemove = (JToken)jArray.Where(a => (int)a["ID"] == Deleted).First();
                jArray.Remove(itemToRemove);

                json = jArray.ToString();
                System.IO.File.WriteAllText(fileName, json);
                Console.WriteLine("Student Id "+Deleted +" Deleted");
                return true;
            }
            catch (Exception)
            {

                return false;
            }



        }


        static List<students> ExampleObject()
        {
            // string fileName = @"c:\users\dell pc\desktop\consoleapp1\sample.json";

            if (File.Exists(fileName))
            {
                var student =
                    JsonConvert.DeserializeObject<List<students>>(File.ReadAllText(fileName));
                return student;
            }

            return null;
        }

        static int input(int f,  int t)
        {
            int value = f - 50;

            while (value < f  || value > t )
            {
                try
                {
                    value = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    value = f - 50;
                    Console.WriteLine("     Invalid Input");
                    Console.Write("     Enter Again : ");
                }
                if ((value < f || value > t) && value!= (f-50) )
                {
                    Console.WriteLine("     Out Of Range" + "("+f+"-"+t+")");
                    Console.Write("     Enter Again : ");
                }
            }
            return value;
        }
    }
}
