using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;
namespace DataHelpers.Tests
{
    [TestClass]
    public class CrudTestPhase1
    {
        private static UnityContainer _container;
        public static void Register()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                _container.RegisterType<UnitOfWork>(new InjectionConstructor(new AdventureWorks2014Entities()));
            }
        }
        [TestMethod]
        public void InsertTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repo = ctx.Repository<Employee2>();
                Employee2 employee = new Employee2
                {
                    FirstName = "Matthew",
                    LastName = "Farrell",
                    EmailAddress = "hacker@darkzone.com",
                    JobTitle = "Hacker"
                };
                employee.Projects.Add(new Project
                {
                    ProjectName = "FBI System Pentester",
                    StartDate = new DateTime(2010, 10, 21),
                    EndDate = new DateTime(2014, 09, 29)
                });
                var insertResult =  repo.Insert(employee);
                var saveResult = ctx.Save();
                Assert.IsTrue(insertResult.Status.IsSuccess && insertResult.Status.Errors.Count == 0
                     && saveResult.IsSuccess && saveResult.Errors.Count == 0);
            }
        }
        [TestMethod]
        public void GetTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                predicateBuilder = predicateBuilder.And(x => x.FirstName.StartsWith("Mat"))
                    .And(x => x.LastName.StartsWith("Far"));

                var getResult = repository.Get(predicateBuilder);
                Assert.IsTrue(getResult.Status.IsSuccess && getResult.Status.Errors.Count == 0);
                Assert.AreEqual(getResult.Data.ID, 294);
            }
        }
        [TestMethod]
        public void GetAllTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                predicateBuilder = predicateBuilder.And(x => x.FirstName.StartsWith("Mat"));

                var getAllResult = repository.GetAll(predicateBuilder);
                Assert.IsTrue(getAllResult.Status.IsSuccess && getAllResult.Status.Errors.Count == 0);
                Assert.AreEqual(getAllResult.Data.Count(), 2);
            }

        }
        [TestMethod]
        public void UpdateTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                predicateBuilder = predicateBuilder.And(x => x.FirstName.StartsWith("Mat"))
                    .And(x => x.LastName.StartsWith("Far"));

                var getResult = repository.Get(predicateBuilder);
                if(getResult.Status.IsSuccess)
                {
                    getResult.Data.EmailAddress = "blackhat@hackermail.com";
                    var updateResult = repository.Update(getResult.Data);
                    var saveResult = ctx.Save();
                    Assert.IsTrue(saveResult.IsSuccess && saveResult.Errors.Count == 0);
                    getResult = repository.Get(predicateBuilder);
                    if(getResult.Status.IsSuccess)
                    {
                        Assert.AreEqual(getResult.Data.EmailAddress, "blackhat@hackermail.com");
                    }
                    else
                    {
                        Assert.Fail("Failed in the last phase. [GET after UPDATE]");
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }

        }
        [TestMethod]
        public void DeleteTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>()
                    .And(x=> new int[] { 32,33 }.Contains(x.ID));

                var deleteResult = repository.Delete(predicateBuilder);
                var saveResult = ctx.Save();
                if (deleteResult.Status.IsSuccess && saveResult.IsSuccess)
                {
                    Assert.AreEqual(deleteResult.Data, 2);
                }
                else
                    Assert.Fail("Failed in the first phase of deletion");
            }
        }
        [TestMethod]
        public void DeleteByParamsTest()
        {

            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>()
                    .And(x => new int[] { 30, 31 }.Contains(x.ID));
                var listResult = repository.GetAll(predicateBuilder);
                if(listResult.Status.IsSuccess)
                {
                    var deleteResult = repository.Delete(listResult.Data.ToArray());
                    var saveResult = ctx.Save();
                    if (deleteResult.Status.IsSuccess && saveResult.IsSuccess)
                    {
                        Assert.AreEqual(deleteResult.Data, 2);
                    }
                    else
                        Assert.Fail("Failed in the first phase of deletion");
                }
                else
                    Assert.Fail("Failed in the GET ALL phase");
            }
        }
        [TestMethod]
        public void GetByRangeTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                int pageIndex = 2;
                int pageSize = 10;
                var getAllResult = repository.GetAllByPaging(pageIndex, pageSize);
                Assert.IsTrue(getAllResult.Status.IsSuccess && getAllResult.Status.Errors.Count == 0);
                Assert.AreEqual(getAllResult.Data.Count(), pageSize);
            }

        }
        [TestMethod]
        public void ExistsTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                string firstName = "Ken";
                string lastName = "Zena";
                var filter = PredicateBuilder.True<Employee2>().And(x =>
                    x.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase) &&
                    x.LastName.Equals(lastName,StringComparison.InvariantCultureIgnoreCase));
                var existsResult = repository.Exists(filter);
                if (existsResult.Status.IsSuccess)
                {
                    Assert.IsTrue(existsResult.Data);
                }
                else
                    Assert.Fail("Failed in first phase checking");
            }
        }
    }
}
