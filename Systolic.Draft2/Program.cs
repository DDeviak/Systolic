using Systolic.Core.Abstractions;
using Systolic.Core.Implementations;

Node<double>[,] nodes = new Node<double>[3, 3];

for (int i = 0; i < nodes.GetLength(0); i++)
{
    for (int j = 0; j < nodes.GetLength(1); j++)
    {
        nodes[i, j] = new Node<double>(new[] { "A", "B", "C" });
        nodes[i, j].Operations["C"] = (registers) => registers["C"] + registers["A"] * registers["B"];
        nodes[i, j].Operations["A"] = (registers) => registers["A"];
        nodes[i, j].Operations["B"] = (registers) => registers["B"];
        nodes[i, j].Links["C"] = nodes[i, j];
    }
}

for (int i = 0; i < nodes.GetLength(0); i++)
{
    for (int j = 0; j < nodes.GetLength(1); j++)
    {
        if (i + 1 != nodes.GetLength(0)) nodes[i, j].Links["A"] = nodes[i + 1, j];
        if (j + 1 != nodes.GetLength(1)) nodes[i, j].Links["B"] = nodes[i, j + 1];
    }
}

double[,] matrixA = new double[3, 3]
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

double[,] matrixB = new double[3, 3]
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

double[,] formattedMatrixA = new double[,]
{
    { 1, 0, 0 },
    { 4, 2, 0 },
    { 7, 5, 3 },
    { 0, 8, 6 },
    { 0, 0, 9 },
    { 0, 0, 0 },
    { 0, 0, 0 }
};

double[,] formattedMatrixB = new double[,]
{
    { 1, 0, 0 },
    { 2, 4, 0 },
    { 3, 5, 7 },
    { 0, 6, 8 },
    { 0, 0, 9 },
    { 0, 0, 0 },
    { 0, 0, 0 }
};

for (int i = 0; i < formattedMatrixA.GetLength(0); i++)
{
    for (int ii = 0; ii < formattedMatrixA.GetLength(1); ii++)
    {
        nodes[0, ii].SetRegister("A", formattedMatrixA[i, ii]);
        nodes[ii, 0].SetRegister("B", formattedMatrixB[i, ii]);
    }

    for (int ii = 0; ii < nodes.GetLength(0); ii++)
    {
        for (int iii = 0; iii < nodes.GetLength(1); iii++)
        {
            nodes[ii, iii].PerformOperations();
        }
    }

    for (int ii = 0; ii < nodes.GetLength(0); ii++)
    {
        for (int iii = 0; iii < nodes.GetLength(1); iii++)
        {
            Console.Write("(" + String.Join(" ", nodes[ii, iii].Operations.Keys.Select(k => nodes[ii, iii].GetRegister(k))) + ") ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();

    for (int ii = 0; ii < nodes.GetLength(0); ii++)
    {
        for (int iii = 0; iii < nodes.GetLength(1); iii++)
        {
            nodes[ii, iii].ShiftRegisters();
        }
    }
}

for (int i = 0; i < nodes.GetLength(0); i++)
{
    for (int j = 0; j < nodes.GetLength(1); j++)
    {
        Console.Write(nodes[i, j].GetRegister("C") + " ");
    }
    Console.WriteLine();
}
