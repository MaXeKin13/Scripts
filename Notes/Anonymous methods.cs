using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteScripts
{
    public class Program

    {
        public delegate bool FilterDelegate(Person p);
        static void Main(string[] args)
        {
            Person p1 = new Person() { Name = "Aiden", Age = 41 };
            Person p2 = new Person() { Name = "Sif", Age = 69 };
            Person p3 = new Person() { Name = "Walter", Age = 12 };
            Person p4 = new Person() { Name = "Anatoli", Age = 25 };

            List<Person> people = new List<Person>() { p1, p2, p3, p4 };

            //pass IsMinor as FilterDelegate, because it has the same signature
            DisplayPeople("Kids", people, IsMinor);

            //here we created a new variable called filter of type FilterDelegate.
            //then we assigned an anonymous method to it instead of an already defined method

            FilterDelegate filter = delegate (Person p) //doesnt have a name, but behaves as a method
            {
                return p.Age >= 20 && p.Age <= 30;
            };
            //semicolon above is important, as it is like creating a variable


            //can use the variable, which is an anonymous method, instead of referencing a defined method. DisplayPeople only cares if it is type FilterDelegate
            DisplayPeople("Between 20 and 30", people, filter);

            //OR
            DisplayPeople("All: ", people, delegate (Person p) { return true; });
        }

        static void DisplayPeople(string title, List<Person> people, FilterDelegate filter)
        {
            foreach (Person p in people)
            {
                if (filter(p))
                {
                    Console.WriteLine("{0}, {1} years old", p.Name, p.Age);
                }
            }
        }

        static bool IsMinor(Person p)
        {
            return p.Age < 18;
        }
        public class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

    }
}
