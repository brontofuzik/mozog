namespace ParticleSwarmOptimization.Functions
{
    public interface ITermination
    {
        bool Terminate(int iteration, double error);
    }
}