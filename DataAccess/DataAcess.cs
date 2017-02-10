using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caching;
namespace DataAccess
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface IPerson
    {
        //[Expire(new List<Type>().Add(typeof(IPerson)))]
        [NoCache]
        [ExpireCache(Regions = new Type[] { typeof(IPerson), typeof(IPersonInfo), typeof(IPersonProduct) })]
        List<Person> GetPerson(Person person);
        [NoCache]
        void UpdatePerson(Person person);
    }

    public interface IPersonProduct
    {
        //[Expire(new List<Type>().Add(typeof(IPerson)))]
        [ExpireCache(Regions = new Type[] { typeof(IPerson) })]
        List<Person> GetPerson(int id, string name, Guid recordId, int? catId);
    }

    public interface IPersonInfo
    {
        //[Expire(new List<Type>().Add(typeof(IPerson)))]
        [ExpireCache(Regions = new Type[] { typeof(IPerson) })]
        List<Person> GetPerson(int id, string name, Guid recordId, int? catId);
    }
    public class PersonRepository : IPerson
    {
        [NoCache]
        public List<Person> GetPerson(Person person)
        {
            List<Person> list = new List<Person>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(new Person() { Id = i, Name = "PersonName" + i.ToString() });
            }

            return list;
        }


        public void UpdatePerson(Person person)
        {
           // throw new NotImplementedException();
        }
    }


}
