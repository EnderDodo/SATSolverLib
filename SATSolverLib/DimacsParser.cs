namespace SATSolverLib;

public class DimacsParser
{

    public static Cnf ParseText(string text)
    {
        var lines = text.Split("\n");
        return ParseLines(lines);
    }
    
    public static Cnf ParseFile(string filePath)
    {
        var lines = File.ReadLines(filePath);
        return ParseLines(lines);
    }

    private static Cnf ParseLines(IEnumerable<string> lines)
    {
        var clauses = new List<Clause>();
        var countVars = -1;
        
        foreach (var line in lines)
        {
            if (line[0] == 'c')
                continue;

            var splitLine = line.Split();
            if (splitLine[0] == "p" && splitLine[1] == "cnf")
            {
                countVars = Convert.ToInt32(splitLine[2]);
                continue;
            }

            var literals = new List<int>();

            foreach (var literalStr in splitLine)
            {
                if (literalStr == "0")
                    break;

                var index = Convert.ToInt32(literalStr);

                if (Math.Abs(index) > countVars)
                    throw new ArgumentException($"Too large index; absolute value of {index} is greater than {countVars}");

                literals.Add(index);
            }

            clauses.Add(new Clause(literals));
        }

        return new Cnf(clauses, countVars);
    }
}