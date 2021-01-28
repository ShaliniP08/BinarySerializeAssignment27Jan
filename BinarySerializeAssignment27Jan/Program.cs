using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace BinarySerializeAssignment27Jan
{
    [Serializable] //attribute
    public class Age : IDeserializationCallback //interface
    {
        DateTime birthDate;
        DateTime todayDate;
        [NonSerialized] //attribute only for fields
        public int years;
        [NonSerialized]
        public int months;
        [NonSerialized]
        public int days;
        public Age()
        {

        } //default

        public Age(DateTime birth, DateTime today)
        {
            birthDate = birth;
            todayDate = today;
        }

        public void OnDeserialization(object sender)
        {
            TimeSpan AgeDiff = todayDate - birthDate;
            DateTime age = DateTime.MinValue + AgeDiff;

            years = AgeDiff.Days / 365;
            months = age.Month - 1;
            days = todayDate.Day - birthDate.Day;
        }
    }
    class Program
    {
        public static void DateFunc()
        {
            try
            {
                Console.WriteLine("Enter your DOB in (DD/MM/YYYY) format");
                DateTime Dob = DateTime.Parse(Console.ReadLine());

                Age a = new Age(Dob, DateTime.Now);
                FileStream fs = new FileStream(@"Age.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, a);

                fs.Seek(0, SeekOrigin.Begin);

                Age de = (Age)bf.Deserialize(fs);
                Console.Write($"\nYour age is {de.years} years, {de.months} months and {de.days} days");
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect Date.", e);
            }
        }
        static void Main(string[] args)
        {
            DateFunc();
            Console.ReadLine();
        }
    }
}