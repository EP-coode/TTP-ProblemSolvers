using ProblemSolvers.Logger;

namespace ProblemSolvers.Logger;


public interface ILogger<T> where T : ILogable
{
    public void Log(T loggable);
}
