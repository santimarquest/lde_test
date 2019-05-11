namespace lde_test
{
    public class Position
    {
        public Location Location { get; set; }
        public Facing Facing { get; set; }

        public Position(Location location, Facing facing)
        {
            this.Location = location;
            this.Facing = facing;
        }
    }
}