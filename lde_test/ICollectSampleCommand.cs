namespace lde_test
{
    // Add colected sample CommandType
    public interface ICollectSampleCommand
    {
        void TakeSample(Terrain terrain, Location location);
    }
}
