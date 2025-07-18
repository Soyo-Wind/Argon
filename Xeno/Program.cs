namespace  Xeno;

class Program
{
    static void Main(string[] args)
    {
        string code = "⨋for≪◈∈i=∈0;,∈i<∈99,⨋stut≪(∈++i%∈3<∈1?\"Fizz\":⫗\"\")+(∈i%∈5<∈1?\"Buzz\":∈i%∈3<∈1?⫗\"\":∈i)≫;≫;";
        Console.WriteLine(Tools.aaa(code));
    }
}