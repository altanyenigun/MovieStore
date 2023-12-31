using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieStoreApi.Data;
using MovieStoreApi.Operation.Mapper;


namespace MovieStoreApi.UnitTests.TestSetups
{
    // testlerimiz için tanımlayacağımız ayarlar.
    public class CommonTestFixture
    {
        public FakeDataContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public IConfiguration Configuration;

        public CommonTestFixture()
        {
            // sadece testler için kullanacağımız ayrı bir fake inmemory database oluşturmalıyız.
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "MovieStoreStoreTestDB").Options;
            Context = new FakeDataContext(options);

            Context.Database.EnsureCreated(); // oluşturulduğundan emin ol
            Context.AddData();
            Context.SaveChanges();

            Mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MapperConfig>(); }).CreateMapper();// Direk olarak WebApi içerisindeki mapper configlerini kullanmasını gösteriyoruz.

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();
        }
    }
}