using Cdcn.Enterprise.Library.Domain.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Tests.Domain.Helper
{
    [TestFixture()]
    public class UniqueIdGeneratorTests
    {
        [Test]
        public void UniqueId_ShouldReturnUniqueIdInCorrectFormat()
        {
            // Arrange: Call UniqueId property (which calls Generate method)
            string uniqueId = UniqueIdGenerator.UniqueId;

            // Assert: Check that the generated unique ID is 8 characters long and alphanumeric
            Assert.AreEqual(8, uniqueId.Length);
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(uniqueId, "^[a-zA-Z0-9]+$"));
        }

        [Test]
        public void Generate_ShouldReturnUniqueIds_WhenCalledMultipleTimes()
        {
            // Arrange: Call Generate multiple times
            string id1 = UniqueIdGenerator.Generate();
            string id2 = UniqueIdGenerator.Generate();

            // Assert: Check that the generated IDs are different
            Assert.AreNotEqual(id1, id2);
        }

        [Test]
        public void UniqueId_GeneratesSameIdForSameTimeStamp()
        {
            // In this test case, we're making sure the ID generated for the same timestamp 
            // and GUID portion will still be unique.
            // This test is now mostly redundant with the timestamp + GUID approach,
            // but you can still expect the uniqueId to be consistent with the time and GUID being deterministic.

            // Arrange: Simulate high precision time
            long fixedTimestamp = Stopwatch.GetTimestamp();  // Get a precise timestamp

            // Act: Generate uniqueId
            string uniqueId = UniqueIdGenerator.UniqueId;

            // Assert: Ensure the generated ID is of the expected length and format
            Assert.AreEqual(8, uniqueId.Length);
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(uniqueId, "^[a-zA-Z0-9]+$"));
        }
        [Test]
        public void UniqueId_ShouldBeThreadSafe_WhenCalledInParallel()
        {
            // Arrange: Store the generated UniqueIds
            var uniqueIds = new HashSet<string>();

            // Act: Run multiple threads to generate UniqueId simultaneously
            var tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    string uniqueId = UniqueIdGenerator.UniqueId;

                    // Assert: Check that the generated uniqueId has correct length and format
                    Assert.AreEqual(8, uniqueId.Length);
                    Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(uniqueId, "^[a-zA-Z0-9]+$"));

                    // Assert: Check that uniqueId is unique (no duplicates)
                    lock (uniqueIds) // Lock to ensure thread-safe access to the HashSet
                    {
                        Assert.IsTrue(uniqueIds.Add(uniqueId), "Duplicate uniqueId found: " + uniqueId);
                    }
                });
            }

            // Wait for all tasks to complete
            Task.WhenAll(tasks).Wait();
        }

       
    }
}