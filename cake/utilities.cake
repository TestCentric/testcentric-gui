//////////////////////////////////////////////////////////////////////
// GLOBALLY ACCESSIBLE UTILITY METHODS CALLED BY CAKE TASKS
//////////////////////////////////////////////////////////////////////

static void CheckTestErrors(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more tests failed, breaking the build.\n"
                              + copyError.Aggregate((x,y) => x + "\n" + y));
    }
}

static void DisplayBanner(string message)
{
    var bar = new string('-', Math.Max(message.Length, 70));
    Console.WriteLine();
    Console.WriteLine(bar);
    Console.WriteLine(message);
    Console.WriteLine(bar);
}
