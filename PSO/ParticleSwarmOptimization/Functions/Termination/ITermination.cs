namespace ParticleSwarmOptimization.Functions.Termination
{
    public interface ITermination
    {
        bool Terminate(int iteration, double error);
    }
}