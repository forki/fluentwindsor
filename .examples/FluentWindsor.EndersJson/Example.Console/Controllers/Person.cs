namespace Example.Console.Controllers
{
    public class Person
    {
        public static readonly Person Any = new Person { Name = "Any", Age = 1 };
        public int Age { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{nameof(Age)}: {Age}, {nameof(Name)}: {Name}";
        }
    }
}