using UtilityDAL.ViewModel;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityWpf.ViewModel;
using UtilityDAL;
using Reactive.Bindings;

namespace UtilityDAL.DemoApp
{



    public class LiteDbDummyViewModel : UtilityWpf.NPC
    {
        private LiteDB.BsonMapper mapper;

        public object NewItem { get; private set; }
        //public ReactiveProperty<DummyDbObject> NewItem { get; } = new ReactiveProperty<DummyDbObject>(
        //));

        private static System.Random r = new Random();
        public LiteDB.BsonMapper Mapper => mapper;

        public LiteDbRepo DocumentStore { get; }

        public LiteDbDummyViewModel()
        {
            mapper = LiteDB.BsonMapper.Global;
            mapper.Entity<DummyDbObject>().Id<int>(x => x.Id);
            //.Field(x => x.Name, "Name");

            Observable.Interval(TimeSpan.FromSeconds(2))
                .StartWith(1)
                .Take(20)
                .Select((_, i) =>
                new DummyDbObject { Id = i + 1, Name = RandomHelper.NextWord() }).Subscribe(_ =>
                {
                    NewItem = _;
                    OnPropertyChanged(nameof(NewItem));
                });


            DocumentStore = new LiteDbRepo("Object", UtilityDAL.Common.DbEx.GetDirectory(System.IO.Directory.GetCurrentDirectory(), "lite"));

        }

    }




    public class RandomHelper
    {

        static string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
        static string[] vowels = { "a", "e", "i", "o", "u" };

        public static string NextWord(int length = 4, Random rand = null)
        {
            rand = rand ?? new Random();

            if (length < 1) // do not allow words of zero length
                throw new ArgumentException("Length must be greater than 0");

            string word = string.Empty;

            if (rand.Next() % 2 == 0) // randomly choose a vowel or consonant to start the word
                word += consonants[rand.Next(0, 20)];
            else
                word += vowels[rand.Next(0, 4)];

            for (int i = 1; i < length; i += 2) // the counter starts at 1 to account for the initial letter
            { // and increments by two since we append two characters per pass
                string c = consonants[rand.Next(0, 20)];
                string v = vowels[rand.Next(0, 4)];

                if (c == "q") // append qu if the random consonant is a q
                    word += "qu";
                else // otherwise just append a random consant and vowel
                    word += c + v;
            }

            // the word may be short a letter because of the way the for loop above is constructed
            if (word.Length < length) // we'll just append a random consonant if that's the case
                word += consonants[rand.Next(0, 20)];

            return word;
        }
    }
}
