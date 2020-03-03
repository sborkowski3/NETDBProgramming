using System;

public class Class1
{
	public abstract Class1()
	{
        
	}
    public abstract class Animal
    {
        public string Name { get; set; }
        public virtual void Talk()
        {
            Console.WriteLine("Hello");
        }
    }
    public class Dog : Animal
    {
        public override void Talk()
        {
            Console.WriteLine("Bark");
        }
        public class Cat : Animal
        {
            public override Talk()
            {
                Console.WriteLine("Meow");
            }
        }
    }
    public class Bird : Animal
    {
        public override Talk()
        {
            Console.WriteLine("Cheep");
        }
    }
}
