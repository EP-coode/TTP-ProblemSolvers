using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.Experiments;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Mutation;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemSolvers.GA.Selection;
using ProblemSolvers.ProblemSolvers.GA;

string desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

var gaParms = new GaParams(mutationFrequencyTreshold: 0.65,
    crossingFrequency: 0.8,
    mutationStrategy: new SwapCountMutation(1),
    crossingStrategy: new OrderedCrossover(),
    parentSelector: new Tournament(0.05),
    new ProblemSolverParams(50)
);

var experiment1 = new Experiment<GaParams>("medium1_test", "medium1_test", 10, 2000, gaParms);

ExperimentRunner<GaParams, GeneticAlghoritm, MinMaxAvg>.RunExperiments(
    new List<Experiment<GaParams>> { experiment1 }.ToArray(),
    new GaSolverFactory(),
    "medium_1.ttp",
    desktopLocation
);



