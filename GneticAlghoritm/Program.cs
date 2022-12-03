using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.Experiments;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Mutation;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemSolvers.GA.Selection;
using ProblemSolvers.ProblemSolvers.GA;
using ProblemSolvers.ProblemSolvers.SA;
using ProblemSolvers.ProblemSolvers.SA.Cooling;
using ProblemSolvers.ProblemSolvers.TS.NeighbourHood;

// EXAPLES

string desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

var gaParms = new GaParams(
    mutationFrequencyTreshold: 0.65,
    crossingFrequency: 0.8,
    mutationStrategy: new SwapCountMutation(1),
    crossingStrategy: new OrderedCrossover(),
    parentSelector: new Tournament(0.05),
    new ProblemSolverParams(50)
);

var saParms = new SaParams(
    coolingStrategy: new ExponentialCooling(0.9995),
    initialTemperature: 1000,
    minTemperature: 0,
    neighbourGenerator: new InverseGenerator()
);

var experiment1 = new Experiment<GaParams>("medium1_GA", 10, 2000, gaParms);
var experiment2 = new Experiment<SaParams>("medium1_SA", 10, 5000, saParms);

ExperimentRunner<GaParams, GeneticAlghoritm, MinMaxAvg>.RunExperiments(
    new List<Experiment<GaParams>> { experiment1 },
    new GaSolverFactory(),
    "medium_1.ttp",
    desktopLocation
);

ExperimentRunner<SaParams, SimulatedAnnealing, SaLog>.RunExperiments(
    new List<Experiment<SaParams>> { experiment2 },
    new SaSolverFactory(),
    "medium_1.ttp",
    desktopLocation
);




