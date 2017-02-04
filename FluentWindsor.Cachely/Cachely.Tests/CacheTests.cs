using System;
using System.Linq;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace FluentlyWindsor.Cachely.Tests
{
    [TestFixture]
    public class CacheTests
    {
        [Test]
        public void GetEnumerator_Should_Return_Item_Types_In_Cache()
        {
            var cache = new Cache<string>();
            cache.SetExpiry(TimeSpan.FromMinutes(1));

            cache.SetItem("1", "Any Item 1");
            cache.SetItem("2", "Any Item 2");
            cache.SetItem("3", "Any Item 3");

            Assert.That(cache.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GetEnumerator_Should_Always_Call_AsyncRemoveExpiredItems()
        {
            var mock = new Mock<Cache<string>>() { CallBase = true };
            mock.Protected().Setup("AsyncRemoveExpiredItems");

            mock.Object.GetEnumerator();

            mock.Protected().Verify("AsyncRemoveExpiredItems", Times.Once());
        }

        [Test]
        public void TryGetValue_Should_Return_Value_If_It_Exists()
        {
            var cache = new Cache<string>();
            cache.SetExpiry(TimeSpan.FromMinutes(1));

            cache.SetItem("1", "Any Item 1");

            string returnValue = null;
            bool result = cache.TryGetValue("1", out returnValue);

            Assert.That(result, Is.True);
            Assert.That(returnValue, Is.EqualTo("Any Item 1"));
        }

        [Test]
        public void TryGetValue_Should_Not_Return_Value_If_None()
        {
            var cache = new Cache<string>();
            cache.SetExpiry(TimeSpan.FromMinutes(1));
            string returnValue = null;
            bool result = cache.TryGetValue("1", out returnValue);

            Assert.That(result, Is.False);
            Assert.That(returnValue, Is.Null);
        }

        [Test]
        public void TryGetValue_Should_Always_Call_AsyncRemoveExpiredItems()
        {
            var mock = new Mock<Cache<string>>() { CallBase = true };
            mock.Protected().Setup("AsyncRemoveExpiredItems");

            string returnValue = null;
            mock.Object.TryGetValue("1", out returnValue);

            mock.Protected().Verify("AsyncRemoveExpiredItems", Times.Once());
        }

        [Test]
        public void GetItem_Should_Always_Return_Value_For_Key()
        {
            var cache = new Cache<string>();
            cache.SetExpiry(TimeSpan.FromMinutes(1));

            cache.SetItem("1", "Any Item 1");
            cache.SetItem("2", "Any Item 2");

            Assert.That(cache.GetItem("1"), Is.EqualTo("Any Item 1"));
            Assert.That(cache.GetItem("2"), Is.EqualTo("Any Item 2"));
        }

        [Test]
        public void GetItem_Should_Not_Throw_And_Return_Default_If_Key_Not_Found()
        {
            var cache = new Cache<string>();
            cache.SetExpiry(TimeSpan.FromMinutes(1)); 
            
            Assert.That(cache.GetItem("1"), Is.EqualTo(default(string)));
        }

        [Test]
        public void GetItem_Should_Always_Call_AsyncRemoveExpiredItems()
        {
            var mock = new Mock<Cache<string>>() { CallBase = true };
            mock.Protected().Setup("AsyncRemoveExpiredItems");

            mock.Object.GetItem("1");

            mock.Protected().Verify("AsyncRemoveExpiredItems", Times.Once());
        }

        [Test]
        public void ExpireItem_Should_Always_Return_Default()
        {
            var cache = new Cache<string>();
            cache.SetExpiry(TimeSpan.FromMinutes(1));

            cache.SetItem("1", "Any Item 1");

            Assert.That(cache.GetItem("1"), Is.EqualTo("Any Item 1"));

            cache.ExpireItem("1");

            Assert.That(cache.GetItem("1"), Is.EqualTo(default(string)));
        }

        [Test]
        public void ExpireItem_Should_Always_Call_AsyncRemoveExpiredItems()
        {
            var mock = new Mock<Cache<string>>() { CallBase = true };
            mock.Protected().Setup("AsyncRemoveExpiredItems");

            mock.Object.SetItem("1", "Any Item 1");
            mock.Object.ExpireItem("1");

            mock.Protected().Verify("AsyncRemoveExpiredItems", Times.AtLeastOnce());
        }

        [Test]
        public void Clear_Should_Get_Rid_Of_All_Items()
        {
            var cache = new Cache<string>();
            cache.SetExpiry(TimeSpan.FromMinutes(1));

            cache.SetItem("1", "Any Item 1");

            Assert.That(cache.GetItem("1"), Is.EqualTo("Any Item 1"));

            cache.Clear();

            Assert.That(cache.GetItem("1"), Is.EqualTo(default(string)));
        }

        
    }
}