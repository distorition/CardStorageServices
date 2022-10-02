using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    internal class Semple033
    {
        static void Main(string[] args)
        {
            var obj1 = ProductFactory.Create<SampleProduct1>();//прмер фабричного метода
            var obj2 = ProductFactory.Create<SampleProduct2>();
        }

    }

    public abstract class Product
    {
        public abstract void PostConstructor();
    }

    public class SampleProduct1 : Product
    {
        public SampleProduct1()//чтобы пользоваться фабричным методом нам нужны конструкторы по умолчания 
        {

        }
        public override void PostConstructor()
        {
            Console.WriteLine("Product1 created");
        }
    }
    public class SampleProduct2 : Product
    {
        public SampleProduct2()
        {

        }
        public override void PostConstructor()
        {
            Console.WriteLine("Product2 created");
        }
    }

    public static class ProductFactory
    {
        public static T Create<T>() where T : Product, new ()//ограничения  new() говорит о том что что мы можем создавать обьекты только если у них есть коснутрктор по умолчанию ( а не обьявленные конструкторы с параметрами)
        {
            var t= new T();//таким образом мы создаем новый обьект 
            t.PostConstructor();
            return t;

        }
    }
}
