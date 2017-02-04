using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FluentlyWindsor.Cachely.Tests
{
    [TestFixture]
    public class ThreadSafeDictionaryTests
    {
        [Test]
        public void ContainsKey_should_return_true_if_it_exists()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add("1", "Any Item 1");

            Assert.That(dict.ContainsKey("1"), Is.True);
            Assert.That(dict.ContainsKey("2"), Is.False);
        }

        [Test]
        public void Indexer_should_always_return_correct_values()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict["1"] = "Any Item 1";

            Assert.That(dict["1"], Is.Not.EqualTo(default(string)));
        }

        [Test]
        public void Keys_should_return_all_keys_in_Dictionary()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add("1", "Any Item 1");

            Assert.That(dict.Keys.Count, Is.EqualTo(1));
            Assert.That(dict.Keys.First(), Is.EqualTo("1"));
        }

        [Test]
        public void Add_should_always_add_the_key_und_value()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add(new KeyValuePair<string, string>("AnyKey", "AnyValue"));

            Assert.That(dict["AnyKey"], Is.EqualTo("AnyValue"));
        }

        [Test]
        public void Clear_should_get_rid_of_all_items()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add(new KeyValuePair<string, string>("AnyKey", "AnyValue"));

            dict.Clear();

            Assert.That(dict.Count, Is.EqualTo(0));
        }

        [Test]
        public void Contains_should_eval_to_true_if_exists()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add(new KeyValuePair<string, string>("AnyKey", "AnyValue"));

            Assert.That(dict.Contains(new KeyValuePair<string, string>("AnyKey", "AnyValue")), Is.True);
        }

        [Test]
        public void CopyTo_should_copy()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add(new KeyValuePair<string, string>("AnyKey", "AnyValue"));
            dict.Add(new KeyValuePair<string, string>("AnyKey1", "AnyValue1"));
            dict.Add(new KeyValuePair<string, string>("AnyKey2", "AnyValue2"));

            var items = new KeyValuePair<string, string>[3];

            dict.CopyTo(items, 0);

            Assert.That(items.Length, Is.EqualTo(3));
        }

        [Test]
        public void Remove_should_get_rid_of_key_value_pair()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add(new KeyValuePair<string, string>("AnyKey", "AnyValue"));

            Assert.That(dict.Count, Is.EqualTo(1));

            dict.Remove(new KeyValuePair<string, string>("AnyKey", "AnyValue"));

            Assert.That(dict.Count, Is.EqualTo(0));
        }

        [Test]
        public void IsReadOnly_should_always_be_false()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            Assert.That(dict.IsReadOnly, Is.False);
        }

        [Test]
        public void Dictionary_should_enumerate_correctly()
        {
            var dict = new ThreadSafeDictionary<string, string>(new Dictionary<string, string>());
            dict.Add(new KeyValuePair<string, string>("AnyKey", "AnyValue"));
            dict.Add(new KeyValuePair<string, string>("AnyKey1", "AnyValue1"));
            dict.Add(new KeyValuePair<string, string>("AnyKey2", "AnyValue2"));

            foreach (KeyValuePair<string, string> keyValue in dict)
            {
                Assert.That(keyValue.Key.StartsWith("AnyKey"), Is.True);
                Assert.That(keyValue.Value.StartsWith("AnyValue"), Is.True);
            }
        }
    }
}