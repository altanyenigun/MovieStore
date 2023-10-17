using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Data;

namespace MovieStoreApi.UnitTests.TestSetups
{
    public class FakeDataContext : DataContext
    {
        public FakeDataContext(DbContextOptions<DataContext> options) : base(options){}
    }
}