using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UtilityDAL.Test
{
    public class UnitTestCSV
    {
        public class User
        {
            public User()
            {
            }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Designation { get; set; }
            public string Location { get; set; }
        }


        [Fact]
        public void Test1()
        {
            var xx = new CSV.Dictionary<User>(nameof(User.Id));

            var data = new Bogus.Faker<User>().RuleFor(a => a.Id, a => ((int)(a.UniqueIndex * 1d / 4)).ToString())
                .RuleFor(a => a.Name, a => a.Name.FirstName())
                .RuleFor(a => a.Designation, a => a.Random.Int().ToString())
                .RuleFor(a => a.Location, a => a.Address.State())
                .Generate(100);

            xx.Clear();

            xx.Insert(data);

            var rdata = xx.Retrieve().ToArray();

            Assert.Equal(25, rdata.Length);
        }


        [Fact]
        public void Test2()
        {
            var xx = new CSV.Dictionary<User>(nameof(User.Id), new Dictionary<string, string> { { nameof(User.Name), "George" } });
            xx.Clear();
            var data = new Bogus.Faker<User>().RuleFor(a => a.Id, a => ((int)(a.UniqueIndex * 1d / 2)).ToString())
                .RuleFor(a => a.Name, a => GetName(a.UniqueIndex))
                .RuleFor(a => a.Designation, a => a.Random.Int().ToString())
                .RuleFor(a => a.Location, a => a.Address.State())
                .Generate(100);

            xx.Insert(data);

            var rdata = xx.Retrieve().ToArray();

            Assert.Equal(17, rdata.Length);
        }

        [Fact]
        public async void Test3()
        {
            var xx = new CSV.Dictionary<User>(nameof(User.Id));
            xx.Clear();
            var data = new Bogus.Faker<User>().RuleFor(a => a.Id, a => a.UniqueIndex.ToString())
                .RuleFor(a => a.Name, a => a.Name.FirstName())
                .RuleFor(a => a.Designation, a => a.Random.Int().ToString())
                .RuleFor(a => a.Location, a => a.Address.State())
                .Generate(10000);

            xx.Insert(data);
            List<User> users = new List<User>();
            var rdata =(UtilityDAL.CSV.CsvHelper.ParseAsObservable<User>("../../../Data/database.csv")).Subscribe(a =>
            {
                users.Add(a);
            }, () => { Assert.Equal(users.Count, data.Count); return; });

            while(true)
            { }
        }



        private static string GetName(int i = 0)
        {
            if (i % 3 == 0)
                return "George";
            else
                return "Mary";
        }
    }
}
