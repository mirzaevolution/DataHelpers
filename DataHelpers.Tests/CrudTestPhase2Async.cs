using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;
namespace DataHelpers.Tests
{
    [TestClass]
    public class CrudTestPhase2Async
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
        public void InsertAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repo = ctx.Repository<Employee2>();
                Employee2 employee = new Employee2
                {
                    FirstName = "Matthew",
                    LastName = "Lampard",
                    EmailAddress = "blackhawk@hackermail.com",
                    JobTitle = "Hacker"
                };
                employee.Projects.Add(new Project
                {
                    ProjectName = "CIA System Pentester",
                    StartDate = new DateTime(2010, 10, 21),
                    EndDate = new DateTime(2014, 09, 29)
                });
                var insertResult = repo.InsertAsync(employee).Result;
                var saveResult = ctx.SaveAsync().Result;
                Assert.IsTrue(insertResult.Status.IsSuccess && insertResult.Status.Errors.Count == 0
                     && saveResult.IsSuccess && saveResult.Errors.Count == 0);
            }
        }
        [TestMethod]
        public void GetAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                predicateBuilder = predicateBuilder.And(x => x.FirstName.StartsWith("Mat"))
                    .And(x => x.LastName.StartsWith("Lamp"));

                var getResult = repository.GetAsync(predicateBuilder).Result;
                Assert.IsTrue(getResult.Status.IsSuccess && getResult.Status.Errors.Count == 0);
                Assert.AreEqual(getResult.Data.LastName, "Lampard");
            }
        }
        [TestMethod]
        public void GetAllAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                predicateBuilder = predicateBuilder.And(x => x.FirstName.StartsWith("Mat"));

                var getAllResult = repository.GetAllAsync(predicateBuilder).Result;
                Assert.IsTrue(getAllResult.Status.IsSuccess && getAllResult.Status.Errors.Count == 0);
                Assert.AreEqual(getAllResult.Data.Count(), 3);
            }

        }
        [TestMethod]
        public void GetAllSortAscAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                var getAllResult = repository.GetAllAsync(null, x => x.OrderBy(y => y.FirstName)).Result;
                Assert.IsTrue(getAllResult.Status.IsSuccess && getAllResult.Status.Errors.Count == 0);
                int count = getAllResult.Data.Count();
                int expected = 286;
                Assert.AreEqual(count, expected);
            }

        }
        [TestMethod]
        public void GetAllSortDescAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                var getAllResult = repository.GetAll(null, x => x.OrderByDescending(y => y.FirstName));
                Assert.IsTrue(getAllResult.Status.IsSuccess && getAllResult.Status.Errors.Count == 0);
                int count = getAllResult.Data.Count();
                int expected = 286;
                Assert.AreEqual(count, expected);
            }

        }
        [TestMethod]
        public void UpdateAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>();
                predicateBuilder = predicateBuilder.And(x => x.FirstName.StartsWith("Mat"))
                    .And(x => x.LastName.StartsWith("Lampard"));

                var getResult = repository.GetAsync(predicateBuilder).Result;
                if (getResult.Status.IsSuccess)
                {
                    getResult.Data.JobTitle = "Pentester";
                    var updateResult = repository.UpdateAsync(getResult.Data).Result;
                    var saveResult = ctx.SaveAsync().Result;
                    Assert.IsTrue(saveResult.IsSuccess && saveResult.Errors.Count == 0);
                    getResult = repository.Get(predicateBuilder);
                    if (getResult.Status.IsSuccess)
                    {
                        Assert.AreEqual(getResult.Data.JobTitle, "Pentester");
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
        public void DeleteAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>()
                    .And(x => new int[] { 35, 36 }.Contains(x.ID));

                var deleteResult = repository.DeleteAsync(predicateBuilder).Result;
                var saveResult = ctx.SaveAsync().Result;
                if (deleteResult.Status.IsSuccess && saveResult.IsSuccess)
                {
                    Assert.AreEqual(deleteResult.Data, 2);
                }
                else
                    Assert.Fail("Failed in the first phase of deletion");
            }
        }
        [TestMethod]
        public void DeleteByParamsAsyncTest()
        {

            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                var predicateBuilder = PredicateBuilder.True<Employee2>()
                    .And(x => new int[] { 37,38 }.Contains(x.ID));
                var listResult = repository.GetAllAsync(predicateBuilder).Result;
                if (listResult.Status.IsSuccess)
                {
                    var deleteResult = repository.DeleteAsync(listResult.Data.ToArray()).Result;
                    var saveResult = ctx.SaveAsync().Result;
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
        //[TestMethod]
        //public void GetByRangeAsyncTest()
        //{
        //    Register();
        //    using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
        //    {
        //        IRepository<Employee2> repository =
        //            ctx.Repository<Employee2>();
        //        int pageIndex = 2;
        //        int pageSize = 10;
        //        var getAllResult = repository.GetAllByPagingAsync(pageIndex, pageSize).Result;
        //        Assert.IsTrue(getAllResult.Status.IsSuccess && getAllResult.Status.Errors.Count == 0);
        //        Assert.AreEqual(getAllResult.Data.Count(), pageSize);
        //    }

        //}
        [TestMethod]
        public void GetByRangeOrderAscAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository =
                    ctx.Repository<Employee2>();
                int pageIndex = 2;
                int pageSize = 10;
                var getAllResult = repository.GetAllByPagingAsync(pageIndex, pageSize, x => x.OrderBy(y => y.FirstName)).Result;
                Assert.IsTrue(getAllResult.Status.IsSuccess && getAllResult.Status.Errors.Count == 0);
                int actual = getAllResult.Data.Count();
                Assert.AreEqual(actual, pageSize);
            }

        }
        [TestMethod]
        public void ExistsAsyncTest()
        {
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                string firstName = "Ken";
                string lastName = "Zena";
                var filter = PredicateBuilder.True<Employee2>().And(x =>
                    x.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase) &&
                    x.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase));
                var existsResult = repository.ExistsAsync(filter).Result;
                if (existsResult.Status.IsSuccess)
                {
                    Assert.IsTrue(existsResult.Data);
                }
                else
                    Assert.Fail("Failed in first phase checking");
            }
        }
        [TestMethod]
        public void ExecuteUpdateCommandAsyncTest()
        {
            string query = @"UPDATE Test.Employee
                             SET EmailAddress = 'ken_zena@adventure-works.com' 
                             WHERE ID = @ID";
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                var execResult = repository.ExecuteSqlCommandAsync(query, new SqlParameter("@ID", 1)).Result;
                if (execResult.Status.IsSuccess)
                {
                    Assert.AreNotEqual(execResult.Data, 0);
                }
                else
                {
                    Assert.Fail("Execution failed in the first phase");
                }
            }
        }
        [TestMethod]
        public void SqlQueryCommandAsyncTest()
        {
            string query = @"SELECT * FROM Test.Employee WHERE FirstName LIKE CONCAT(@Name,'%')";
            Register();
            using (UnitOfWork ctx = _container.Resolve<UnitOfWork>())
            {
                IRepository<Employee2> repository = ctx.Repository<Employee2>();
                var execResult = repository.SqlQueryAsync(query, new SqlParameter("@Name", "Matth")).Result;
                if (execResult.Status.IsSuccess)
                {
                    var dataActual = execResult.Data.ToList();

                    Assert.AreEqual(dataActual.Count, 3);
                }
                else
                {
                    Assert.Fail("Execution failed in the first phase");
                }
            }
        }
    }
}
