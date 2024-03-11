using FourInARow.Enums;

namespace FourInARow.Models
{
    public class HumanPlayerModel
    {
        public static int Id { get; private set; } = 1;
        public string Name { get; private set; } = null;
        public int RowMoveInput { get; set; } = 0;
        public int ColumnMoveInput { get; set; } = 0;
        public eGamePieceType GamePiece { get; set; }

        public HumanPlayerModel()
        {
            string playerName = string.Format("Player {0}", Id++);

            Name = playerName;
        }
    }
}