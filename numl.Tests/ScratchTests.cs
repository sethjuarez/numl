using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Data;

namespace numl.Tests
{
    [TestFixture]
    public class ScratchTests
    {
        [Test]
        public void Gen_House_SQL()
        {
            House[] data = House.GetData();
            string table = "Mailer";
            string query = "INSERT INTO {0}(District, HouseType, Income, Customer, Response) VALUES('{1}', '{2}', '{3}', {4}, {5});";

            StringBuilder sb = new StringBuilder();
            foreach (House d in data)
            {
                var q = string.Format(query, table, d.District, d.HouseType, d.Income, d.PreviousCustomer ? 1 : 0, d.Response ? 1 : 0);
                sb.AppendLine(q);
            }
        }

        [Test]
        public void Gen_Student_SQL()
        {
            Student[] data = Student.GetData();
            string table = "Student";
            string query = "INSERT INTO {0}(Name, Grade, GPA, Age, Friends, Graduation) VALUES('{1}', '{2}', {3}, {4}, {5}, {6});";

            StringBuilder sb = new StringBuilder();
            foreach (Student d in data)
            {
                var q = string.Format(query, table, d.Name, d.Grade, d.GPA, d.Age, d.Friends, d.Nice ? 1 : 0);
                sb.AppendLine(q);
            }

            var qs = sb.ToString();
        }
    }
}
