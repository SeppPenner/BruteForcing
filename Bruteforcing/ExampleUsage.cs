public class ExampleUsage
{
    public void Bruteforce()
    {
        IBruteforcing bruteforce = new Bruteforcing("abcdefghijklmnopqrstuvwxyz0123456789", 1, 3);

        foreach (string result in bruteforce)
        {
            var text = result;
        }
    }
}