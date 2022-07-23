namespace ByLearningAutoFac
{
    public interface IFoo
    {
        IBar Bar { get; }
        IBaz Baz { get; }
        int GetFooValue();
    }

    public interface IBar
    {
        string Name { get; set; }
    }
    public interface IBaz
    {
        public int Value { get; }
    }
    public class Foo : IFoo
    {
        public IBar Bar { get; set; }
        public IBaz Baz { get; set; }
        public Foo() { }
        public Foo(IBar bar)
        {
            this.Bar = bar;
        }
        public Foo(IBar bar, IBaz baz)
        {
            this.Bar = bar;
            this.Baz = baz;
        }
        public int GetFooValue()
        {
            return 0;
        }
    }
    public class Bar : IBar
    {
        private string name = "boyden";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Bar() { }
        public Bar(string name)
        {
            this.name = name;
        }
    }

    public class Baz : IBaz
    {
        public int Value
        {
            get => 217;
        }
    }

    public interface Card
    {
        int Id { get; set; }
    }
    public class BalckCard : Card
    {
        public int Id { get; set; }
    }
    public class WhiteCard : Card
    {
        public int Id { get; set; }
    }

    public interface IGeneric<T> where T : class
    {
        T Tobject { get; set; }
    }

    public class GenericClass<T> : IGeneric<T> where T : class
    {
        private T tobj;
        public T Tobject
        {
            get => tobj;
            set => tobj = value;
        }
    }
}
