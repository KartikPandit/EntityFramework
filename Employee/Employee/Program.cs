using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public class Map
    {
        public string id { get; set; }
        public string name { get; set; }
        public System.DateTime dob { get; set; }
        public System.DateTime doj { get; set; }
        public string location { get; set; }
        public static Map FromCsv (string csvLine)
        {
            string[] values = csvLine.Split(',');
            Map obj = new Map();
            obj.id = values[0];
            obj.name = values[2];
            string dateString = values[10];
            string[] formats = {"dd/MM/yyyy","MM/dd/yyyy", "dd-MMM-yyyy", "yyyy-MM-dd",
                   "dd-MM-yyyy", "M/d/yyyy", "dd MMM yyyy"};
            DateTime converteddob = DateTime.ParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime converteddoj = DateTime.ParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            obj.dob = converteddob;
            obj.location = Convert.ToString(values[30]);
            obj.doj = converteddoj;
            return obj;
        }
    }
    class Program
    {
        
            static void Main (string[] args)
            {
                Options();
            }
            public static void Options ()
            {
                Console.WriteLine("Select from the given optios");
                Console.WriteLine("1)Insert from CSV to DataBase");
                Console.WriteLine("2)Location");
                Console.WriteLine("3)DOB");
                Console.WriteLine("4)DOJ");
                Console.WriteLine("5)Exit");
                try
                {
                    int option = int.Parse(Console.ReadLine());



                    switch (option)
                    {

                        case 1:
                            Program.InsertCsv();
                            Options();
                            break;
                       case 2:
                            searchonlocation();
                            Options();
                            break;
                        case 3:
                            searchondob();
                            Options();
                            break;
                        case 4:
                            searchondoj();
                            Options(); 
                            break;
                        case 5:
                            return;
                        default:
                            Console.WriteLine("Select the right option");
                            Options();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter the correct value" + e);
                    Options();

                }

                Console.ReadKey();

            }
        public static void InsertCsv()
        {
            List<Map> values = File.ReadAllLines(@"C:\Users\yuhub\OneDrive\Desktop\records.csv")
                                .Skip(1)
                                .Select(s => Map.FromCsv(s))
                                .ToList();




            try
            {
                foreach (var cells in values)
                {
                    using (EmployeeEntities context = new EmployeeEntities())
                    {
                        employee emp = new employee
                        {
                            empid = cells.id,
                            empname = cells.name,
                            dob = cells.dob,
                            location = cells.location,
                            doj = cells.doj
                        };
                        context.employees.Add(emp);
                        context.SaveChanges();
                    }
                }
                Console.WriteLine("Insert Successful");
                Console.WriteLine("\n");
            }
            catch(Exception e)
            {
                Console.WriteLine("Data Already Exists");
            }
        }
        static void searchonlocation ()
        {

            Console.WriteLine("Enter the Location");
            String loc = Console.ReadLine();
            using (EmployeeEntities context = new EmployeeEntities())
            {
                try
                {
                    employee emp = context.employees.FirstOrDefault(r => r.location == loc);
                    TextInfo format = CultureInfo.CurrentCulture.TextInfo;
                    Console.WriteLine("Name: "+format.ToTitleCase(emp.empname));
                    Console.WriteLine("ID: "+format.ToTitleCase(emp.empid));
                   

                    Console.WriteLine("\n");
                    Console.WriteLine(emp.empid + "," + emp.empname + "," + emp.dob + "," + emp.doj);
                }
                catch (Exception e)
                {

                    Console.WriteLine(loc + " Not found");
                }
            }

        }
          static void searchondob ()
          {

              Console.WriteLine("Enter the dob");
            DateTime date = DateTime.Parse(Console.ReadLine());
              using (EmployeeEntities context = new EmployeeEntities())
              {
                  try
                  {
                      employee emp = context.employees.FirstOrDefault(r => DateTime.Compare(r.dob,date)<=0);
                      if (emp.empid != null)
                      {
                          Console.WriteLine(emp.empid + "," + emp.empname + "," + emp.doj + "," + emp.location);
                          Console.WriteLine("\n");
                      }
                      else
                      {
                          Console.WriteLine("No Employee found");
                          Console.WriteLine("\n");
                      }
                  }
                  
                  catch (Exception e)
                  {

                      Console.WriteLine(date + " Not found");
                      Console.WriteLine("\n");
                  }
              }
          }
        static void searchondoj ()
        {

            Console.WriteLine("Enter the doj");
            DateTime doj = DateTime.Parse(Console.ReadLine());
            

            using (EmployeeEntities context = new EmployeeEntities())
            {
                try
                {

                     employee emp = context.employees.FirstOrDefault(r => DateTime.Compare((r.doj), doj) <= 0);
                      if (emp.empid != null)
                      {

                          Console.WriteLine(emp.empid + "," + emp.empname + "," + emp.dob + "," + emp.location);
                      }
                      else
                          Console.WriteLine("No Employee Found");
                        
}
                catch (Exception e)
                {

                    Console.WriteLine(" Not found");
                }
            }
        }
    }
                        
}
        
