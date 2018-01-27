/*
 * QueryTests.cs
 * Testing queries for translation
 *
   Copyright 2018 Roman Lisak, Grzegorz Mrukwa

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Spectre.Database.Contexts;
using Spectre.Database.Entities;
using Spectre.Database.Utils;
using Xunit;

namespace Spectre.Database.Tests
{
    public class QueryTests : IDisposable
    {
        // Inspired by https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite
        // They will be no further refactored to use SQLite than by just supplying provider
        // since only querying is done now. In the case when more sophisticated features come,
        // it should be migrated to fully use the potential of SQLite during tests.

        private readonly Mock<DatasetsContext> _mockContext;
        private readonly DbConnection _connection;

        public QueryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            var options = new DbContextOptionsBuilder<DatasetsContext>()
                .UseSqlite(_connection)
                .Options;

            var data = new List<Dataset>
            {
                new Dataset { FriendlyName = "FriendlyName1", Hash = "Hash1", UploadNumber = "UploadNumber1"},
                new Dataset { FriendlyName = "FriendlyName2", Hash = "Hash2", UploadNumber = "UploadNumber2"},
                new Dataset { FriendlyName = "FriendlyName3", Hash = "Hash3", UploadNumber = "UploadNumber3"},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Dataset>>();
            mockSet.As<IQueryable<Dataset>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Dataset>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Dataset>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Dataset>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatasetsContext>(options);
            mockContext.Setup(c => c.Datasets).Returns(mockSet.Object);
            _mockContext = mockContext;
        }

        public void Dispose()
        {
            _connection.Close();
        }

        [Fact]
        public void HashToUploadNumber_finds_proper_uploadnumber_for_given_hash()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.HashToUploadNumberOrDefault("Hash1");

            Assert.Equal("UploadNumber1", dataset);
        }

        [Fact]
        public void FriendlyNameToUploadNumber_finds_proper_uploadnumber_for_given_friendlyname()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.FriendlyNameToUploadNumberOrDefault("FriendlyName2");

            Assert.Equal("UploadNumber2", dataset);
        }

        [Fact]
        public void HashToUploadNumer_returns_null_for_not_existing_hash()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.HashToUploadNumberOrDefault("NotExistingHash");

            Assert.Null(dataset);
        }

        [Fact]
        public void FriendlyNameToUploadNumer_returns_null_for_not_existing_friendly_name()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.FriendlyNameToUploadNumberOrDefault("NotExistingFriendlyName");

            Assert.Null(dataset);
        }

        [Fact]
        public void UploadNumberToHash_finds_proper_hash_for_given_uploadnumber()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.UploadNumberToHashOrDefault("UploadNumber3");

            Assert.Equal("Hash3", dataset);
        }

        [Fact]
        public void UploadNumberToHash_returns_null_for_not_existing_Upload_Name()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.UploadNumberToHashOrDefault("NotExistingUploadNumber");

            Assert.Null(dataset);
        }

        [Fact]
        public void HashToFriendlyName_finds_proper_friendlyname_for_given_hash()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.HashToFriendlyNameOrDefault("Hash1");

            Assert.Equal("FriendlyName1", dataset);
        }

        [Fact]
        public void HashToFriendlyName_returns_null_for_not_existing_Hash()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.HashToFriendlyNameOrDefault("NotExistingHash");

            Assert.Null(dataset);
        }

        [Fact]
        public void UploadNumberToFriendlyName_finds_proper_friendlyname_for_given_uploadnumber()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.UploadNumberToFriendlyNameOrDefault("UploadNumber2");

            Assert.Equal("FriendlyName2", dataset);
        }

        [Fact]
        public void UploadNumberToFriendlyName_returns_null_for_not_existing_uploadnumber()
        {
            var service = new DatasetDetailsFinder(_mockContext.Object);

            var dataset = service.HashToFriendlyNameOrDefault("NotExistingUploadNumber");

            Assert.Null(dataset);
        }
    }
}