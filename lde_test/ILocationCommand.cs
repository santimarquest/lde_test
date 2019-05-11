namespace lde_test
{
    // Change position commands
    public interface ILocationCommand
    {
        Location NextLocation(Terrain terrain, Location location, ILocationCommand locationCommand);
    }
}
