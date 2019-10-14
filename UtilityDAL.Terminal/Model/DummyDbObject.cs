using System;

namespace UtilityDAL.DemoApp
{
    public class DummyDbObject : IEquatable<DummyDbObject>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Equals(DummyDbObject other)
        {
            return Name == other.Name;
        }

        public DummyDbObject()
        {
            var r = new Random();
            Id = r.Next(1, 10);
            Name = RandomHelper.NextWord(4 + Id, r);
        }
    }
}