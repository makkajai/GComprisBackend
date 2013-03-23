using ServiceStack.DataAnnotations;

namespace GComprisBackend.ServiceModel.Types
{
    [Alias("boards")]
    public class Board
    {
        [Alias("board_id"), PrimaryKey]
        public int Id { get; set; }

        [Alias("name")]
        public string Name { get; set; }
    }
}
