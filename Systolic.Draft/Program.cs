using Systolic.Core.Abstractions;
using Systolic.Core.Implementations;

Node<double>[] nodes = Enumerable.Repeat(0, 5)
.Select(x => new Node<double>(new string[] { "A", "X", "Y" })
{
    Operations = new Dictionary<string, Func<Dictionary<string, double>, double>>
    {
        ["Y"] = registers => registers["Y"] + registers["X"] * registers["A"],
        ["X"] = registers => registers["X"]
    }
}).ToArray();

Barrier barrier = new Barrier(nodes.Length);
for (int i = 0; i < nodes.Length; i++)
{
    nodes[i].Barrier = barrier;
    nodes[i].Links = new Dictionary<string, INode<double>>
    {
        ["X"] = i + 1 < nodes.Length ? nodes[i + 1] : null!,
        ["Y"] = i - 1 >= 0 ? nodes[i - 1] : null!
    };
}

QueueNode<double> queueNode = new QueueNode<double>(new string[] { "A", "X", "Y" });

nodes[0].Links["Y"] = queueNode;

double[,] input = new double[,]
{
    { 0,0,1,0,0 },
    { 0,4,0,2,0 },
    { 7,0,5,0,3 },
    { 0,8,0,6,0 },
    { 0,0,9,0,0 },
    { 0,0,0,0,0 },
    { 0,0,0,0,0 },
    { 0,0,0,0,0 },
    { 0,0,0,0,0 },
    { 0,0,0,0,0 }
};

double[] inputVector = { 0, 1, 0, 2, 0, 3, 0, 0, 0, 0 };

for (int i = 0; i < input.GetLength(0); i++)
{
    for (int j = 0; j < input.GetLength(1); j++)
    {
        nodes[j].SetRegister("A", input[i, j]);
    }

    nodes[0].SetRegister("X", inputVector[i]);

    foreach (var node in nodes)
    {
        node.PerformOperations();
        node.ResetRegisters();
        node.ShiftRegisters();
    }
}

Console.WriteLine(String.Join(", ", queueNode.Registers["Y"].Select(x => x.ToString())));